using System;
using System.Collections.Generic;

namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// 一筆 DMD 訊息的處理結果，供 UI 顯示。
    /// </summary>
    public class DMDProcessResult
    {
        public DateTime ReceivedTime { get; set; } = DateTime.Now;
        public ASI.Wanda.DMD.Message.Message.eMessageType MessageType { get; set; }
        public int MessageID { get; set; }
        public int MessageLength { get; set; }
        public string HexContent { get; set; }
        public string JsonContent { get; set; }
        public string JsonObjectName { get; set; }

        /// <summary>負責處理的處理器名稱（找不到為空字串）</summary>
        public string HandlerName { get; set; } = "";

        public bool IsSimulated { get; set; }
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; }

        /// <summary>處理過程中執行（或模擬執行）的動作</summary>
        public List<string> Actions { get; } = new List<string>();

        /// <summary>JsonObjectName 去掉命名空間的短名稱</summary>
        public string ShortObjectName
        {
            get
            {
                if (string.IsNullOrEmpty(JsonObjectName)) return "";
                int i = JsonObjectName.LastIndexOf('.');
                return i >= 0 ? JsonObjectName.Substring(i + 1) : JsonObjectName;
            }
        }
    }
}
