using ASI.Lib.Config;
using ASI.Wanda.DCU.DB.Models.DMD;
using ASI.Wanda.DCU.DB.Tables.DMD;

using Display;
using Display.DisplayMode;
using Display.Function;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskPUP
{
    public static class Constants
    {
        public const string SendPreRecordMsg = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage";
        public const string SendInstantMsg = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendInstantMessage";
        public const string SendScheduleSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.ScheduleSetting";
        public const string SendPreRecordMessageSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PreRecordMessageSetting";
        public const string SendTrainMessageSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting";
        public const string SendPowerTimeSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PowerTimeSetting";
        public const string SendGroupSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.GroupSetting";
        public const string SendParameterSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.ParameterSetting";
    }
    public class DeviceInfo
    {
        public string Station { get; set; }
        public string Location { get; set; }
        public string DeviceWithNumber { get; set; }
    }
    public class TaskPUPHelper
    {
        private string _mProcName;
        private List<string> _managedDevices;  // 此 Task 管理的所有設備
        private string _currentDeviceId;       // 當前操作的設備
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;

        public TaskPUPHelper(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial, string stationId = null)
        {
            _mProcName = mProcName;
            _mSerial = serial;

            // 從資料庫載入此 Task 負責的設備
            LoadManagedDevices(stationId);

            // 設定預設設備（第一個PDU設備）
            _currentDeviceId = _managedDevices.FirstOrDefault() ?? "LG01_UPF_PDU-1";

            ASI.Lib.Log.DebugLog.Log(_mProcName,
                $"載入 {_managedDevices.Count} 個 PDU 設備: {string.Join(", ", _managedDevices)}");
        }

        /// <summary>
        /// 從資料庫載入此 Task 負責管理的設備列表（PDU設備）
        /// </summary>
        private void LoadManagedDevices(string stationId)
        {
            _managedDevices = new List<string>();

            try
            {
                // TaskPUP 負責 PDU 設備（包含 UPF_PDU 和 DPF_PDU）
                var upfDevices = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetDeviceIdsByType("UPF_PDU", stationId);
                var dpfDevices = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetDeviceIdsByType("DPF_PDU", stationId);

                _managedDevices.AddRange(upfDevices);
                _managedDevices.AddRange(dpfDevices);

                if (_managedDevices.Count == 0)
                {
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, "警告：未找到任何 PDU 設備，使用預設值");
                    _managedDevices.Add("LG01_UPF_PDU-1");
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"載入設備列表失敗: {ex.Message}");
                // 使用預設值
                _managedDevices.Add("LG01_UPF_PDU-1");
            }
        }

        /// <summary>
        /// 檢查設備ID是否由此 Task 管理
        /// </summary>
        public bool IsDeviceManaged(string deviceId)
        {
            return _managedDevices.Contains(deviceId, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 從 target_du 字串中找出此 Task 負責的設備
        /// </summary>
        private List<string> ExtractManagedDevices(string targetDu)
        {
            var result = new List<string>();

            if (string.IsNullOrEmpty(targetDu))
                return result;

            string[] deviceStrings = targetDu.Split(',');
            foreach (var deviceString in deviceStrings)
            {
                string trimmed = deviceString.Trim();
                if (IsDeviceManaged(trimmed))
                {
                    result.Add(trimmed);
                }
            }
            return result;
        }

        /// <summary>
        /// 獲取此 Task 管理的所有設備列表
        /// </summary>
        public List<string> GetManagedDevices()
        {
            return new List<string>(_managedDevices);
        }
        private static DeviceInfo SplitStringToDeviceInfo(string deviceString)
        {
            // 正則表達式模式
            string pattern = @"([A-Z0-9]+)_([A-Z]+)_([A-Z]+-\d+)";
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

        #region  版型的操作
        public class DisplayMessageResult
        {
            public string Result { get; set; }
            public byte[] DataByte { get; set; }
        }


        /// <summary>
        /// 發送訊息至顯示器的主函式。
        /// </summary>
        /// <param name="targetDu">目標設備單元標識符。</param>
        /// <param name="dbName1">資料庫名稱 1。</param>
        /// <param name="dbName2">資料庫名稱 2。</param>
        /// <param name="result">操作結果輸出參數。</param>
        /// <param name="dataByte">封包資料輸出參數。</param>
        public void SendMessageToDisplay(string targetDu, string dbName1, string dbName2, out string result)
        {
            var results = CreateAndSendMessage(targetDu, dbName1, dbName2);
            var successCount = results.Count(r => r.Result == "成功傳送");

            result = successCount > 0 ? $"成功傳送 {successCount} 筆訊息" : "傳送失敗";

            foreach (var displayResult in results)
            {
                if (displayResult.DataByte != null)
                {
                    _mSerial.Send(displayResult.DataByte);
                }
            }
        }


        /// <summary>
        /// 創建並傳送顯示訊息，並回傳傳送結果與封包內容。
        /// </summary>
        /// <param name="targetDu">目標設備單元標識符。</param>
        /// <param name="dbName1">資料庫名稱 1。</param>
        /// <param name="dbName2">資料庫名稱 2。</param>
        /// <returns>包含操作結果與封包資料的 DisplayMessageResult 物件。</returns>
        private List<DisplayMessageResult> CreateAndSendMessage(string targetDu, string dbName1, string dbName2)
        {
            var results = new List<DisplayMessageResult>();

            try
            {
                // 驗證輸入參數
                ValidateInput(targetDu);

                // 從 target_du 中提取此 Task 負責的設備
                var devicesToProcess = ExtractManagedDevices(targetDu);

                if (devicesToProcess.Count == 0)
                {
                    // 如果 target_du 中沒有此 Task 的設備
                    ASI.Lib.Log.DebugLog.Log(_mProcName,
                        $"target_du 中無 PDU 設備，跳過處理: {targetDu}");
                    var skipResult = new DisplayMessageResult
                    {
                        Result = "無相關設備",
                        DataByte = null
                    };
                    results.Add(skipResult);
                    return results;
                }

                // 對每個設備發送訊息
                foreach (var deviceId in devicesToProcess)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"處理設備: {deviceId}");

                    var deviceInfo = GetDeviceInfo(deviceId);
                    var messageIds = GetPlayingItemIds(deviceInfo.Station, deviceInfo.Location, deviceInfo.DeviceWithNumber);

                    foreach (var messageId in messageIds)
                    {
                        var result = new DisplayMessageResult(); // 每個 messageId 的結果

                        try
                        {
                            var messageLayout = GetMessageLayout(messageId);
                            var textStringBody = CreateTextStringBody(messageLayout);
                            var fullWindowMessage = CreateFullWindowMessage(textStringBody, messageLayout);

                            var sequence = CreateDisplaySequence(fullWindowMessage);
                            var packet = CreatePacket(deviceId, sequence);  // 使用特定設備ID

                            result.DataByte = SerializeAndSendPacket(packet);
                            result.Result = "成功傳送";
                        }
                        catch (Exception ex)
                        {
                            HandleError(ex, result);
                        }

                        results.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                var result = new DisplayMessageResult { Result = "傳送失敗：未知錯誤", DataByte = null };
                HandleError(ex, result);
                results.Add(result); // 添加通用的異常結果
            }

            return results;
        }


        /// <summary>
        /// 驗證輸入的目標設備單元標識符是否為空值。
        /// </summary>
        /// <param name="targetDu">目標設備單元標識符。</param>
        private void ValidateInput(string targetDu)
        {
            if (string.IsNullOrEmpty(targetDu))
                throw new ArgumentNullException(nameof(targetDu), "目標設備單元標識符不能為空。");
        }

        /// <summary>
        /// 根據目標設備標識符解析設備資訊。
        /// </summary>
        /// <param name="targetDu">目標設備單元標識符。</param>
        /// <returns>解析後的設備資訊物件。</returns>
        private DeviceInfo GetDeviceInfo(string targetDu)
        {
            var deviceInfo = SplitStringToDeviceInfo(targetDu);
            if (deviceInfo == null)
                throw new InvalidOperationException("無法從 targetDu 中解析設備資訊。");
            return deviceInfo;
        }

        /// <summary>
        /// 根據設備資訊從資料庫中取得當前正在播放的消息 ID。 邏輯有誤 待修改
        /// </summary>
        /// <param name="deviceInfo">設備資訊物件。</param>
        /// <returns>當前播放的消息 ID。</returns> 
        private List<Guid> GetPlayingItemIds(string Station, string Location, string DeviceID)
        {
            var messageId = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemIds(Station, Location, DeviceID);
            if (messageId == null)
                throw new InvalidOperationException("無法從資料庫中取得正在播放的消息 ID。");
            return messageId;
        }

        /// <summary>
        /// 根據消息 ID 取得消息的佈局內容。
        /// </summary>
        /// <param name="messageId">消息 ID。</param>
        /// <returns>消息佈局物件。</returns>
        private dmd_pre_record_message GetMessageLayout(Guid messageId)
        {
            var messageLayout = ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.SelectMessage(messageId);
            if (messageLayout == null)
                throw new InvalidOperationException($"無法找到消息 ID 為 {messageId} 的消息佈局。");
            return messageLayout;
        }

        /// <summary>
        /// 創建文字訊息主體，包含 RGB 顏色與顯示文字內容。
        /// </summary>
        /// <param name="messageLayout">消息佈局物件。</param>
        /// <returns>TextStringBody 文字訊息主體物件。</returns>
        private TextStringBody CreateTextStringBody(dmd_pre_record_message messageLayout)
        {
            var fontColor = ProcessMessageColor(messageLayout.font_color);
            //   var fontColor = new byte[] { 0xff, 0xff, 0x00 }; 
            if (fontColor == null || fontColor.Length != 3)
                throw new InvalidOperationException("無法處理消息顏色或 RGB 值無效。");

            return new TextStringBody
            {
                RedColor = fontColor[0],
                GreenColor = fontColor[1],
                BlueColor = fontColor[2],
                StringText = messageLayout.message_content
            };
        }

        /// <summary>
        /// 創建全屏顯示的消息物件，包括消息內容、滾動資訊及優先級。
        /// </summary>
        /// <param name="textStringBody">文字訊息主體物件。</param>
        /// <param name="messageLayout">消息佈局物件。</param>
        /// <returns>FullWindow 全屏消息物件。</returns>
        private FullWindow CreateFullWindowMessage(TextStringBody textStringBody, dmd_pre_record_message messageLayout)
        {
            return new FullWindow
            {
                MessageType = 0x71,
                MessageLevel = (byte)messageLayout.message_priority,
                MessageScroll = new ScrollInfo
                {
                    ScrollMode = 0x64,
                    ScrollSpeed = 05,
                    PauseTime = 10
                },
                MessageContent = new List<StringMessage> { new StringMessage { StringMode = 0x2A, StringBody = textStringBody } }
            };
        }
        /// <summary>
        /// 建立顯示序列物件，設定序列號、字體與消息內容。
        /// </summary>
        /// <param name="fullWindowMessage">全屏消息物件。</param>
        /// <returns>顯示序列物件。</returns>
        private Display.Sequence CreateDisplaySequence(FullWindow fullWindowMessage)
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
        /// </summary>
        /// <param name="sequence">顯示序列物件。</param>
        /// <returns>資料封包物件。</returns>
        private Packet CreatePacket(string DU_ID, Display.Sequence sequence)
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(DU_ID, false);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(DU_ID, true);

            var processor = new PacketProcessor();
            return processor.CreatePacket(startCode, new List<byte> { Convert.ToByte(back), Convert.ToByte(front) }, new PassengerInfoHandler().FunctionCode, new List<Display.Sequence> { sequence });
        }

        /// <summary>
        /// 序列化封包並記錄日誌。
        /// </summary>
        /// <param name="packet">要序列化的資料封包。</param>
        /// <returns>序列化的字節陣列。</returns>
        private byte[] SerializeAndSendPacket(Packet packet)
        {
            var processor = new PacketProcessor();
            var serializedData = processor.SerializePacket(packet);
            string result = BitConverter.ToString(serializedData).Replace("-", " ");
            ASI.Lib.Log.DebugLog.Log(_mProcName + " SendMessageToDisplay", "Serialized display packet: " + result);

            // 移除重複發送：封包會在 SendMessageToDisplay 方法中統一發送
            return serializedData;
        }

        /// <summary>
        /// 捕獲並處理異常，記錄相應的錯誤日誌。
        /// </summary>
        /// <param name="ex">捕獲的異常物件。</param>
        /// <param name="result">包含操作結果的 DisplayMessageResult 物件。</param>
        private void HandleError(Exception ex, DisplayMessageResult result)
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
        /// /緊急訊息
        /// </summary>
        /// <param name="FireContentChinese">中文緊急訊息內容</param>
        /// <param name="FireContentEnglish">英文緊急訊息內容</param>
        /// <param name="situation">情境代碼</param>
        /// <param name="targetDeviceId">目標設備ID（可選，預設為第一個管理的設備）</param>
        public Tuple<byte[], byte[], byte[]> SendMessageToUrgnt(string FireContentChinese, string FireContentEnglish, int situation, string targetDeviceId = null)
        {
            byte[] serializedDataChinese = new byte[] { };
            byte[] serializedDataEnglish = new byte[] { };
            byte[] serializedDataOff = new byte[] { }; // 新增存放關閉訊息的序列化數據

            // 決定使用的設備ID
            string effectiveDeviceId = targetDeviceId ?? _currentDeviceId;

            // 驗證設備是否由此Task管理
            if (!IsDeviceManaged(effectiveDeviceId))
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"設備 {effectiveDeviceId} 不由此 Task 管理");
                return Tuple.Create(serializedDataChinese, serializedDataEnglish, serializedDataOff);
            }

            try
            {
                // 設定警示的固定內容
                var processor = new PacketProcessor();
                var startCode = new byte[] { 0x55, 0xAA };
                var function = new EmergencyMessagePlaybackHandler();
                var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(effectiveDeviceId, false);
                var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(effectiveDeviceId, true);

                // 序列化中文訊息
                var sequence1 = CreateSequence(FireContentChinese, 1);
                var packet1 = processor.CreatePacket(
                    startCode,
                    new List<byte> { Convert.ToByte(front), Convert.ToByte(back) },
                    function.FunctionCode,
                    new List<Display.Sequence> { sequence1 }
                );
                serializedDataChinese = processor.SerializePacket(packet1);

                // 發送中文訊息
                _mSerial.Send(serializedDataChinese);
                ASI.Lib.Log.DebugLog.Log(_mProcName, $"已發送緊急訊息(中文) situation={situation}: {BitConverter.ToString(serializedDataChinese)}");

                // 序列化英文訊息
                var sequence2 = CreateSequence(FireContentEnglish, 2);
                var packet2 = processor.CreatePacket(
                    startCode,
                    new List<byte> { Convert.ToByte(front), Convert.ToByte(back) },
                    function.FunctionCode,
                    new List<Display.Sequence> { sequence2 }
                );
                serializedDataEnglish = processor.SerializePacket(packet2);

                // 發送英文訊息
                _mSerial.Send(serializedDataEnglish);
                ASI.Lib.Log.DebugLog.Log(_mProcName, $"已發送緊急訊息(英文) situation={situation}: {BitConverter.ToString(serializedDataEnglish)}");

                // 如果情境為 84，執行延遲並關閉
                if (situation == 84)
                {
                    System.Threading.Thread.Sleep(10000); // 同步延遲十秒
                    var OffMode = new byte[] { 0x02 };
                    var packetOff = processor.CreatePacketOff(
                        startCode,
                        new List<byte> { 0x11, 0x12 },
                        function.FunctionCode,
                        OffMode
                    );
                    serializedDataOff = processor.SerializePacket(packetOff);

                    // 發送關閉訊息
                    _mSerial.Send(serializedDataOff);
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"已發送緊急訊息關閉指令: {BitConverter.ToString(serializedDataOff)}");
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("SendMessageToUrgnt", ex);
            }

            // 返回中文、英文和關閉訊息的序列化數據（供日誌或測試使用） 
            return Tuple.Create(serializedDataChinese, serializedDataEnglish, serializedDataOff);
        }
        /// <summary>
        /// 找尋車站Id並且判斷是否需要開關顯示器（節能模式）
        /// </summary>
        /// <param name="stationID">車站ID</param>
        /// <returns></returns>
        public dmdPowerSetting PowerSetting(string stationID)
        {
            try
            {
                //從資料庫讀取資料
                var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);

                if (stationData.eco_mode != "on")
                {
                    ASI.Lib.Log.DebugLog.Log("PowerSetting", "目前沒有開啟節能模式");
                    return null;
                }

                // 獲取當前日期和時間
                int currentMonth = DateTime.Now.Month;
                int currentDay = DateTime.Now.Day;
                int currentHour = DateTime.Now.Hour;
                int currentMinute = DateTime.Now.Minute;

                // 解析不啟動節能模式的日期列表
                string[] notEcoDays = stationData.not_eco_day.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                bool isNonEcoDay = false;

                foreach (string day in notEcoDays)
                {
                    if (day.Length != 4)
                    {
                        ASI.Lib.Log.DebugLog.Log("PowerSetting", $"無效的日期格式：{day}");
                        continue;
                    }

                    int month = int.Parse(day.Substring(0, 2));
                    int dayOfMonth = int.Parse(day.Substring(2, 2));

                    // 檢查當前日期是否在不啟動節能模式的日期列表中
                    if (month == currentMonth && dayOfMonth == currentDay)
                    {
                        isNonEcoDay = true;
                        ASI.Lib.Log.DebugLog.Log("PowerSetting", $"今天 {currentMonth:D2}/{currentDay:D2} 是不啟動節能模式的日期");
                        break;
                    }
                }

                // 如果今天是不啟動節能模式的日期，直接返回
                if (isNonEcoDay)
                {
                    return null;
                }

                // 解析自動播放和節能時間
                string[] autoPlayTimes = stationData.auto_play_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string[] autoEcoTimes = stationData.auto_eco_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (autoPlayTimes.Length != 1 || autoEcoTimes.Length != 1)
                {
                    ASI.Lib.Log.DebugLog.Log("PowerSetting", "時間設定格式錯誤");
                    return null;
                }

                // 解析時間字串
                int autoPlayHour = int.Parse(autoPlayTimes[0].Substring(0, 2));      // 開啟顯示器的時間（時）
                int autoPlayMinute = int.Parse(autoPlayTimes[0].Substring(3, 2));   // 開啟顯示器的時間（分）
                int autoEcoHour = int.Parse(autoEcoTimes[0].Substring(0, 2));       // 關閉顯示器的時間（時）
                int autoEcoMinute = int.Parse(autoEcoTimes[0].Substring(3, 2));     // 關閉顯示器的時間（分）

                // 判斷當前時間是否為自動播放時間（開啟顯示器）
                if (currentHour == autoPlayHour && currentMinute == autoPlayMinute)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"到達自動播放時間 {autoPlayHour:D2}:{autoPlayMinute:D2}，開啟顯示器");
                    PowerSettingOpen();
                }
                // 判斷當前時間是否為節能模式時間（關閉顯示器）
                else if (currentHour == autoEcoHour && currentMinute == autoEcoMinute)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"到達節能模式時間 {autoEcoHour:D2}:{autoEcoMinute:D2}，關閉顯示器");
                    PowerSettingOff();
                }
                else
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, "當前時間不在自動播放或節能模式時間範圍內");
                }

                return null;
            }
            catch (Exception ex)
            {
                // 加入詳細的錯誤信息
                ASI.Lib.Log.ErrorLog.Log($"Error ProcessMessage PowerSetting: {ex.Message}\nStack Trace: {ex.StackTrace}", ex);
                return null;
            }
        }
        #endregion
        Display.Sequence CreateSequence(string messageContent, int sequenceNo)
        {
            var textStringBody = new TextStringBody
            {
                RedColor = 0xFF,
                GreenColor = 0x00,
                BlueColor = 0x00,
                StringText = messageContent
            };
            var stringMessage = new StringMessage
            {
                StringMode = 0x2A, // TextMode (Static)  
                StringBody = textStringBody
            };
            var urgentMessage = new Urgent // Display version  
            {
                UrgntMessageType = 0x79, // message
                MessageType = 0x71,
                MessageLevel = 0x01, // level
                MessageScroll = new ScrollInfo { ScrollMode = 0x64, ScrollSpeed = 07, PauseTime = 10 },
                MessageContent = new List<StringMessage> { stringMessage }
            };
            return new Display.Sequence
            {
                SequenceNo = (byte)sequenceNo,
                IsUrgent = true,
                UrgentCommand = 0x01,
                Font = new FontSetting { Size = FontSize.Font16x16, Style = FontStyle.Ming },
                Messages = new List<IMessage> { urgentMessage }
            };
        }
        /// <summary>
        /// 顯示器的畫面開啟
        /// </summary>
        /// <param name="targetDeviceId">目標設備ID（可選，預設為第一個管理的設備）</param>
        public void PowerSettingOpen(string targetDeviceId = null)
        {
            // 決定使用的設備ID
            string effectiveDeviceId = targetDeviceId ?? _currentDeviceId;

            if (!IsDeviceManaged(effectiveDeviceId))
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"設備 {effectiveDeviceId} 不由此 Task 管理");
                return;
            }

            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var Open = new byte[] { 0x3A, 0X00 };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(effectiveDeviceId, false);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(effectiveDeviceId, true);

            var packetOpen = processor.CreatePacketOff(startCode, new List<byte> { Convert.ToByte(front), Convert.ToByte(back) }, function.FunctionCode, Open);
            var serializedDataOpen = processor.SerializePacket(packetOpen);
            _mSerial.Send(serializedDataOpen);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 解除緊急訊息", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen));
        }
        /// <summary>
        /// 顯示器的畫面關閉
        /// </summary>
        /// <param name="targetDeviceId">目標設備ID（可選，預設為第一個管理的設備）</param>
        public void PowerSettingOff(string targetDeviceId = null)
        {
            // 決定使用的設備ID
            string effectiveDeviceId = targetDeviceId ?? _currentDeviceId;

            if (!IsDeviceManaged(effectiveDeviceId))
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"設備 {effectiveDeviceId} 不由此 Task 管理");
                return;
            }

            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var Off = new byte[] { 0x3A, 0X01 };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(effectiveDeviceId, false);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(effectiveDeviceId, true);

            var packetOff = processor.CreatePacketOff(startCode, new List<byte> { Convert.ToByte(front), Convert.ToByte(back) }, function.FunctionCode, Off);
            var serializedDataOff = processor.SerializePacket(packetOff);
            _mSerial.Send(serializedDataOff);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 解除緊急訊息", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
        }

        #region  資料庫的操作
        /// <summary>
        /// 色碼轉換成byte
        /// </summary>
        /// <param name="colorName"></param>
        /// <returns></returns>
        private byte[] ProcessMessageColor(string colorName)
        {
            try
            {
                var ConfigDate = ASI.Wanda.DCU.DB.Tables.System.sysConfig.SelectColor(colorName);
                ASI.Lib.Log.DebugLog.Log(_mProcName, ConfigDate.config_value.ToString());
                return DataConversion.FromHex(ConfigDate.config_value);
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("Error ProcessMessage ProcessMessageColor", ex);
                return null;
            }
        }

        #endregion
    }
}

