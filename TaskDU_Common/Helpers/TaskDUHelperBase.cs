using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Display;
using Display.DisplayMode;
using Display.Function;

using TaskDU_Common.Models;

namespace TaskDU_Common.Helpers
{
    /// <summary>
    /// CDU / SDU / PDN / PUP 共用的 Helper 基底類別（第 2 波）。
    ///
    /// 已確認可安全合併的方法收在這裡，包含：
    ///   - PowerSettingOpen / PowerSettingOff：SDU 原本用 GetPanelIDByDuAndOrientation
    ///     動態查面板 ID，CDU/PDN 是寫死 { 0x11, 0x12 }。已確認 SDU 的動態查詢寫法才是
    ///     正確行為，統一採用 SDU 版本。這會改變 CDU/PDN 既有的序列埠輸出（原本送固定
    ///     的 0x11/0x12，改成送查詢出來的實際面板 ID），套用到每個裝置前務必用現場設備
    ///     或錄製封包驗證開關顯示器功能正常。
    ///   - PowerSetting：CDU/SDU 版本彼此一致（先判斷今天是否為非節能日，再判斷目前時段
    ///     該開或該關）。PDN 原本是不同的演算法（迴圈內同時判斷非節能日與時段、用 continue
    ///     跳過），已確認四個裝置的省電時段本來就是統一的、沒有理由不同，因此判定 PDN
    ///     那版是寫歪的，統一採用 CDU/SDU 這版。套用到 PDN 時務必驗證省電排程行為正確。
    ///
    /// 以下方法「刻意不」放進來，因為比對後發現它們在四個裝置間有真正的行為差異，
    /// 貿然合併會改變現有行為，且目前還不確定哪一種才是正確行為：
    ///
    ///   - ProcessMessageColor：PDN 多了一行疑似除錯用的 log
    ///     （ASI.Lib.Log.DebugLog.Log(_mProcName + "470", colorName)）。
    ///   - CreateTextStringBody / CreateFullWindowMessage：CDU 用反射處理多種
    ///     訊息類型，SDU/PDN 是針對 dmd_pre_record_message 寫死欄位，且 PDN 的
    ///     ScrollSpeed 是動態取自 messageLayout.move_speed，SDU/CDU 是寫死 05。
    ///   - SendMessageToDisplay / CreateAndSendMessage：三個裝置對 dmd_instant_message /
    ///     dmd_train_message 的支援程度不同（CDU 有實作即時訊息，SDU/PDN 是 TODO stub），
    ///     PDN 的方法簽章也是 List&lt;string&gt; 而不是 string。
    ///
    /// 這些留在各自的 TaskXXXHelper.cs 裡，等確認每一項的正確行為後再個別搬移。
    /// </summary>
    public abstract class TaskDUHelperBase
    {
        protected readonly string _mProcName;
        protected readonly ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;

        protected TaskDUHelperBase(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _mProcName = mProcName;
            _mSerial = serial;
        }

        /// <summary>
        /// 各裝置的 DU_ID（例如 CDU 是 "LG01_CCS_CDU-1"），供 PowerSettingOpen/Off 查詢面板 ID 用。
        /// 各 TaskXXXHelper 覆寫此屬性，回傳自己原本就有的 _mDU_ID 常數。
        /// </summary>
        protected abstract string DuId { get; }

        /// <summary>
        /// 將 DMD Server 傳送過來的設備 ID 字串（例如 LG01_CCS_CDU-1）切割成 Station/Location/DeviceWithNumber。
        /// </summary>
        protected static DeviceInfo SplitStringToDeviceInfo(string deviceString)
        {
            string pattern = @"([A-Z0-9]+)_([A-Z]+)_([A-Z]+-\d+)";
            // target_du 裡沒有本 Task 負責的裝置時 deviceString 會是 null，
            // 以前直接丟給 Regex 會出現「值不能為 null。參數名稱: input」這種看不懂的錯誤
            if (string.IsNullOrWhiteSpace(deviceString))
                throw new ArgumentException("target_du 中沒有本 Task 負責類型的裝置", nameof(deviceString));

            Match match = Regex.Match(deviceString, pattern);

            if (match.Success)
            {
                return new DeviceInfo
                {
                    Station = match.Groups[1].Value,
                    Location = match.Groups[2].Value,
                    DeviceWithNumber = match.Groups[3].Value
                };
            }
            else
            {
                throw new ArgumentException("Invalid device string format", nameof(deviceString));
            }
        }

        /// <summary>
        /// 驗證輸入的目標設備單元標識符是否為空值。
        /// </summary>
        protected void ValidateInput(string targetDu)
        {
            if (string.IsNullOrEmpty(targetDu))
                throw new ArgumentNullException(nameof(targetDu), "目標設備單元標識符不能為空。");
        }

        /// <summary>
        /// 根據目標設備標識符解析設備資訊。
        /// </summary>
        protected DeviceInfo GetDeviceInfo(string targetDu)
        {
            var deviceInfo = SplitStringToDeviceInfo(targetDu);
            if (deviceInfo == null)
                throw new InvalidOperationException("無法從 targetDu 中解析設備資訊。");
            return deviceInfo;
        }

        /// <summary>
        /// 根據設備資訊從資料庫中取得當前正在播放的消息 ID。
        /// </summary>
        protected List<Guid> GetPlayingItemIds(string station, string location, string deviceId)
        {
            var messageId = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemIds(station, location, deviceId);
            if (messageId == null)
                throw new InvalidOperationException("無法從資料庫中取得正在播放的消息 ID。");
            return messageId;
        }

        /// <summary>
        /// 捕獲並處理異常，記錄相應的錯誤日誌。
        /// </summary>
        protected void HandleError(Exception ex, DisplayMessageResult result)
        {
            switch (ex)
            {
                case ArgumentNullException argEx:
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, $"參數錯誤: {argEx.Message}");
                    result.Result = "傳送失敗：參數錯誤";
                    break;
                case InvalidOperationException opEx:
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, $"操作異常: {opEx.Message}");
                    result.Result = "傳送失敗：操作異常";
                    break;
                default:
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, $"未知錯誤: {ex}");
                    result.Result = "傳送失敗：未知錯誤";
                    break;
            }
        }

        /// <summary>
        /// 建立顯示序列物件（單則全窗訊息），設定序列號、字體與消息內容。
        /// </summary>
        protected Display.Sequence CreateDisplaySequence(FullWindow fullWindowMessage)
        {
            return new Display.Sequence
            {
                SequenceNo = 1,
                Font = new FontSetting { Size = FontSize.Font24x24, Style = FontStyle.Ming },
                Messages = new List<IMessage> { fullWindowMessage }
            };
        }

        /// <summary>
        /// 根據設備資訊與顯示序列建立資料封包。
        ///
        /// 注意：原本 CDU/PDN 是 { front, back } 順序，SDU/PUP 是 { back, front } 順序，
        /// 比對後確認 front/back 的計算方式四份完全相同，只有塞進封包的順序不同——
        /// 且 SDU 自己的 PowerSettingOpen/Off 用的又是 { front, back }，內部就自相矛盾，
        /// 判斷是複製貼上時的疏漏而非刻意的硬體差異，因此統一成 { front, back }。
        /// 之後套用到 SDU/PUP 時，這是唯一會改變既有序列埠輸出的地方，套用前務必用
        /// 現場設備或錄製封包驗證。
        /// </summary>
        protected Packet CreatePacket(string duId, Display.Sequence sequence)
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(duId, false);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(duId, true);
            var processor = new PacketProcessor();
            return processor.CreatePacket(
                startCode,
                ASI.Wanda.DCU.DB.Tables.DCU.dulist.ToPanelList(front, back),
                new PassengerInfoHandler().FunctionCode,
                new List<Display.Sequence> { sequence });
        }

        /// <summary>
        /// 序列化封包並透過串口傳送封包資料。
        /// </summary>
        protected byte[] SerializeAndSendPacket(Packet packet)
        {
            var processor = new PacketProcessor();
            var serializedData = processor.SerializePacket(packet);
            string result = BitConverter.ToString(serializedData).Replace("-", " ");
            ASI.Lib.Log.DebugLog.Log(_mProcName + " SendMessageToDisplay", "Serialized display packet: " + result);
            _mSerial.Send(serializedData);
            return serializedData;
        }

        /// <summary>
        /// 顯示器的畫面開啟。面板 ID 動態查詢（採用 SDU 原本的正確寫法，
        /// 取代 CDU/PDN 原本寫死 { 0x11, 0x12 } 的行為）。
        /// </summary>
        public void PowerSettingOpen()
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var open = new byte[] { 0x3A, 0x00 };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(DuId, false);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(DuId, true);
            var packetOpen = processor.CreatePacketOff(startCode, ASI.Wanda.DCU.DB.Tables.DCU.dulist.ToPanelList(front, back), function.FunctionCode, open);
            var serializedDataOpen = processor.SerializePacket(packetOpen);
            _mSerial.Send(serializedDataOpen);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面開啟", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen));
        }

        /// <summary>
        /// 顯示器的畫面關閉。面板 ID 動態查詢（採用 SDU 原本的正確寫法，
        /// 取代 CDU/PDN 原本寫死 { 0x11, 0x12 } 的行為）。
        /// </summary>
        public void PowerSettingOff()
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var off = new byte[] { 0x3A, 0x01 };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(DuId, false);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(DuId, true);
            var packetOff = processor.CreatePacketOff(startCode, ASI.Wanda.DCU.DB.Tables.DCU.dulist.ToPanelList(front, back), function.FunctionCode, off);
            var serializedDataOff = processor.SerializePacket(packetOff);
            _mSerial.Send(serializedDataOff);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面關閉", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
        }

        /// <summary>
        /// 找尋車站節能設定並判斷目前時段是否該開啟/關閉顯示器。
        /// 統一採用 CDU/SDU 的版本（先判斷今天是否為非節能日，再判斷目前時段）。
        /// </summary>
        public ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting PowerSetting(string stationID)
        {
            var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);
            if (stationData == null)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, "無法取得車站節能設定：" + stationID);
                return null;
            }

            if (stationData.eco_mode != "ON")
                return null;

            var now = DateTime.Now;
            string todayKey = now.Month.ToString("D2") + now.Day.ToString("D2");
            int currentHour = now.Hour;

            string[] notEcoDays = stationData.not_eco_day.Split(
                new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Step 1：先判斷今天是否為非節能日（找到即跳出）
            bool isNonEcoDay = false;
            foreach (string day in notEcoDays)
            {
                if (day.Length == 4 &&
                    int.TryParse(day.Substring(0, 2), out int m) &&
                    int.TryParse(day.Substring(2, 2), out int d))
                {
                    if (m.ToString("D2") + d.ToString("D2") == todayKey)
                    {
                        isNonEcoDay = true;
                        break;
                    }
                }
                else
                {
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, "無效的日期格式：" + day);
                }
            }

            if (isNonEcoDay)
            {
                return null;
            }

            // Step 2：時間判斷在迴圈外，只執行一次
            string[] autoPlayTimes = stationData.auto_play_time.Split(
                new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] autoEcoTimes = stationData.auto_eco_time.Split(
                new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (autoPlayTimes.Length == 2 && autoEcoTimes.Length == 2 &&
                int.TryParse(autoPlayTimes[0], out int playStart) &&
                int.TryParse(autoPlayTimes[1], out int playEnd) &&
                int.TryParse(autoEcoTimes[0], out int ecoStart) &&
                int.TryParse(autoEcoTimes[1], out int ecoEnd))
            {
                if (currentHour >= playStart && currentHour <= playEnd)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, "關閉顯示器");
                    PowerSettingOff();
                }
                else if (currentHour >= ecoStart && currentHour <= ecoEnd)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, "開啟顯示器");
                    PowerSettingOpen();
                }
            }
            else
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, "自動播放時間或自動節能時間格式錯誤");
            }

            return null;
        }
    }
}
