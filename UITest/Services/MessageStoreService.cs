using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace UITest.Services
{
    /// <summary>單筆訊息資料</summary>
    public class MessageEntry
    {
        public int    Index   { get; set; }
        public int    Length  { get; set; }
        public string Content { get; set; } = "";
    }

    /// <summary>上下行訊息清單的 XML 根節點</summary>
    public class MessageStore
    {
        public List<MessageEntry> UpMessages   { get; set; } = new List<MessageEntry>();
        public List<MessageEntry> DownMessages { get; set; } = new List<MessageEntry>();
    }

    /// <summary>
    /// 管理上下行訊息清單的載入與儲存。
    /// 資料存放於 display_messages.xml（與 display_settings.xml 同目錄）。
    /// </summary>
    public static class MessageStoreService
    {
        private static readonly string StorePath =
            Path.Combine(Application.StartupPath, "display_messages.xml");

        private const int MaxRows = 33;   // 索引 0~32

        // ── 公開 API ──────────────────────────────────────────────────────

        /// <summary>從磁碟載入；若檔案不存在則傳回預設示範資料。</summary>
        public static MessageStore Load()
        {
            if (File.Exists(StorePath))
            {
                try
                {
                    var xs = new XmlSerializer(typeof(MessageStore));
                    using (var sr = new StreamReader(StorePath, Encoding.UTF8))
                        return (MessageStore)xs.Deserialize(sr);
                }
                catch { /* 損毀時使用預設 */ }
            }
            return CreateDefault();
        }

        /// <summary>儲存至磁碟。</summary>
        public static void Save(MessageStore store)
        {
            var xs = new XmlSerializer(typeof(MessageStore));
            using (var sw = new StreamWriter(StorePath, false, Encoding.UTF8))
                xs.Serialize(sw, store);
        }

        /// <summary>確保清單恰好有 MaxRows 列（補空列或截斷）。</summary>
        public static void PadToMaxRows(List<MessageEntry> list)
        {
            while (list.Count < MaxRows)
                list.Add(new MessageEntry { Index = list.Count, Content = "" });
            if (list.Count > MaxRows)
                list.RemoveRange(MaxRows, list.Count - MaxRows);

            // 重建索引
            for (int i = 0; i < list.Count; i++)
                list[i].Index = i;
        }

        // ── 私有 ──────────────────────────────────────────────────────────

        private static MessageStore CreateDefault()
        {
            var store = new MessageStore();

            var defaultMsgs = new[]
            {
                "萬大線中英文abcdeABCDE123456",
                "歡迎搭乘台北捷運萬大線",
                "零一二三四五六七八九十",
                "零一二三四五六七八九十零一二三四",
                "abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdef",
                "abcdefghijklmnopqrstuvwxyz012345",
                "0123456789ab",
                "倒數測試",
                "自動關機測試",
                "數位時鐘",
                "配合市府防疫政策，5/15起暫停開放動物園、貓空纜車、兒童新樂園等設施",
                "Cooperating with the city governments epidemic prevention policy",
                "1分鐘空白語音測試",
                "1 minute blank voice test",
                "您好，請往車廂內部移動，將後背包提於手上或置於前方",
                "Hello, please move inside the compartment, put the backpack in front of you",
                "發現異常狀況時，冷靜勿當慌，勿奔跑推擠，將正確的訊息傳遞出去",
                "When discovering abnormal conditions, dont panic, dont run to push each other",
                "您好，上下車、搭乘電扶梯、上下樓梯或行走時，不要使用手機",
                "Hello, do not use your mobile phone when you get on and off, take the escalator",
            };

            for (int i = 0; i < MaxRows; i++)
            {
                string content = i < defaultMsgs.Length ? defaultMsgs[i] : "";
                var entry = new MessageEntry
                {
                    Index   = i,
                    Content = content,
                    Length  = string.IsNullOrEmpty(content) ? 0 : Encoding.Default.GetByteCount(content)
                };
                store.UpMessages.Add(entry);
                store.DownMessages.Add(new MessageEntry
                {
                    Index   = i,
                    Content = content,
                    Length  = entry.Length
                });
            }

            return store;
        }
    }
}
