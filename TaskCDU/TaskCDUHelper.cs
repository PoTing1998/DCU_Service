
using Display.DisplayMode;
using Display.Function;
using Display;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Wanda.DCU.DB.Models.DMD;
using ASI.Wanda.DCU.DB.Tables.DMD;
using System.Text.RegularExpressions;
using ASI.Lib.Config;
using System.IO;

namespace ASI.Wanda.DCU.TaskCDU
{
    #region constructor

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




    #endregion
    public class TaskCDUHelper
    {
        #region constructor
        private string _mProcName;
        private const string Pattern = @"LG01_CCS_CDU-1"; // 定義要篩選的模式
        public const string _mDU_ID = "LG01_CCS_CDU-1";
        public bool is_back = true; //顯示器的面板 正反面定義
        static string StationID = ConfigApp.Instance.GetConfigSetting("Station_ID");
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;

        public TaskCDUHelper(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _mProcName = mProcName;
            _mSerial = serial;
        }

        public class DisplayMessageResult
        {
            public string Result { get; set; }
            public byte[] DataByte { get; set; }
        }


        #endregion

        #region  版型的操作    

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
            var failureCount = results.Count(r => r.Result != "成功傳送");
            var failedMessages = results.Where(r => r.Result != "成功傳送").ToList();

            // 統一生成回應結果
            result = successCount > 0
                ? $"成功處理 {successCount} 筆訊息，失敗 {failureCount} 筆。"
                : "所有訊息處理失敗。";

            try
            {
                // 整理所有成功的資料，組合成一筆訊息
                var combinedData = CombineMessages(results.Where(r => r.Result == "成功傳送").ToList());

                if (combinedData != null && combinedData.Length > 0)
                {
                    // 傳送組合後的訊息
                    _mSerial.Send(combinedData);
                }
                else
                {
                    LogError("組合後的訊息為空，未能發送。");
                }
            }
            catch (Exception ex)
            {
                // 捕捉發送時的例外並記錄
                LogError($"組合訊息傳送失敗: {ex.Message}");
            }

            // 可選：記錄失敗訊息的詳細資訊  
            if (failedMessages.Any())
            {
                foreach (var failed in failedMessages)
                {
                    LogError($"處理失敗的訊息 : {failed.Result}");
                }
            }

        }
        // 新增的輔助方法：將多筆資料組合成一筆訊息 
        private byte[] CombineMessages(List<DisplayMessageResult> successfulResults)
        {
            try
            {
                // 假設每筆資料的 DataByte 是 byte[]，這裡進行合併  
                using (var memoryStream = new MemoryStream())
                { 
                        foreach (var result in successfulResults)
                        {
                            if (result.DataByte != null)
                            {
                                memoryStream.Write(result.DataByte, 0, result.DataByte.Length);
                            }
                        }
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                LogError($"組合訊息時發生錯誤: {ex.Message}");
                return null;
            }
        }
        // 假設有個錯誤日誌方法 
        private void LogError(string message)
        {
            // 替換成你的日誌框架或存檔邏輯
           ASI.Lib.Log.ErrorLog.Log( "信息處理錯誤" ,$"[Error] {message}");
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
            var fullWindowMessages = new List<FullWindow>(); // 用於儲存所有生成的 FullWindowMessage 
            try
            {
                // 驗證輸入參數  
                ValidateInput(targetDu);

                string[] deviceStrings = targetDu.Split(',');
                string matchedDevice = null;
                foreach (var deviceString in deviceStrings)
                {
                    string trimmedDevice = deviceString.Trim();
                    if (Regex.IsMatch(trimmedDevice, Pattern))
                    {
                        matchedDevice = trimmedDevice; 
                        break;
                    }
                }
                var deviceInfo = GetDeviceInfo(matchedDevice);
                var messageIds = GetPlayingItemIds(deviceInfo.Station, deviceInfo.Location, deviceInfo.DeviceWithNumber);

                if (dbName1 == "dmd_instant_message" && messageIds.Count == 1)
                {
                    // 專門處理即時訊息的邏輯
                    results.Add(SendInstantMessage(matchedDevice, messageIds.First())); 
                }
                else
                { 
                    // 批量處理預錄訊息  
                    foreach (var messageId in messageIds)
                    {
                        var result = new DisplayMessageResult(); // 每個 messageId 的結果  
                        try
                        {
                            if (dbName1 == "dmd_pre_record_message")
                            {
                                var messageLayout = GetPreRecordedMessageLayoutById(messageId);

                                // 創建文字內容和完整視窗訊息  
                                var textStringBody = CreateTextStringBody(messageLayout);//訊息內容 
                                var fullWindowMessage = CreateFullWindowMessage(textStringBody, messageLayout); //版型

                                // 將 FullWindowMessage 加入集合 
                                fullWindowMessages.Add(fullWindowMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            HandleError(ex, result); 
                        }

                        results.Add(result);
                    }
                    
                    // 統一創建並發送 sequence 最多五則  
                    if (fullWindowMessages.Any()) 
                    {
                        results.Add(SendBatchMessage(matchedDevice, fullWindowMessages));
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
        /// 處理即時訊息的發送邏輯。   
        /// </summary>
        private DisplayMessageResult SendInstantMessage(string matchedDevice, Guid messageId)
        {
            var result = new DisplayMessageResult();

            try
            {
                var messageLayout = GetInstantMessageLayoutById(messageId);
                var textStringBody = CreateTextStringBody(messageLayout);  
                var fullWindowMessage = CreateFullWindowMessage(textStringBody, messageLayout);

                var instantSequence = CreateDisplaySequence(fullWindowMessage);
                var DUID = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDs(matchedDevice);
                var packet = CreatePacket(_mDU_ID, instantSequence);

                result.DataByte = SerializeAndSendPacket(packet);
                result.Result = "成功傳送";
            }
            catch (Exception ex)
            {
                HandleError(ex, result);
            }

            return result;
        }
        /// <summary>
        /// 處理批量訊息的發送邏輯。  
        /// </summary>
        private DisplayMessageResult SendBatchMessage(string matchedDevice, List<FullWindow> fullWindowMessages)
        {
            var result = new DisplayMessageResult();

            try
            {
                var sequence = CreateDisplaySequence(fullWindowMessages);
                var DUID = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDs(matchedDevice);
                var packet = CreatePacket(_mDU_ID, sequence);
                result.DataByte = SerializeAndSendPacket(packet);
                result.Result = "成功發送所有訊息。";
            }
            catch (Exception ex)
            {
                HandleError(ex, result);
            }

            return result;
        }
        /// <summary>
        /// 創建文字訊息主體，包含 RGB 顏色與顯示文字內容。
        /// </summary>
        /// <typeparam name="T">消息佈局物件的類型。</typeparam>
        /// <param name="messageLayout">消息佈局物件。</param>
        /// <returns>TextStringBody 文字訊息主體物件。</returns>
        private TextStringBody CreateTextStringBody<T>(T messageLayout) where T : class
        {
            // 使用反射獲取屬性 依照資料庫名稱
            var fontColorProperty = typeof(T).GetProperty("font_color");
            var messageContentProperty = typeof(T).GetProperty("message_content");
            var messageContentEnProperty = typeof(T).GetProperty("message_content_en");

            if (fontColorProperty == null || messageContentProperty == null || messageContentEnProperty == null)
                ASI.Lib.Log.ErrorLog.Log( "資料庫取的相關資料" ,$"類型 {typeof(T).Name} 缺少必要屬性。");

            // 提取屬性值 
            var fontColor = (string)fontColorProperty.GetValue(messageLayout);
            var messageContent = (string)messageContentProperty.GetValue(messageLayout) ?? string.Empty;
            var messageContentEn = (string)messageContentEnProperty.GetValue(messageLayout) ?? string.Empty;

            // 處理顏色
            var rgbValues = ProcessMessageColor(fontColor);
            if (rgbValues == null || rgbValues.Length != 3)
                throw new InvalidOperationException("無法處理消息顏色或 RGB 值無效。");

            return new TextStringBody
            {
                RedColor = rgbValues[0],
                GreenColor = rgbValues[1],
                BlueColor = rgbValues[2],
                StringText = messageContent + messageContentEn
            };
        }

        private FullWindow CreateFullWindowMessage<T>(TextStringBody textStringBody, T messageLayout) where T : class
        {
            // 讀取 資料庫的檔案
            var messagePriorityProperty = typeof(T).GetProperty("message_priority");
            //  var ScrollMode = typeof(T).GetProperty("move_mode");
            //  var ScrollSpeed = typeof(T).GetProperty("move_speed");
            //  var interval =typeof(T).GetProperty("Interval");
            if (messagePriorityProperty == null)
                throw new ArgumentException("The message layout does not contain a 'message_priority' property.");
            var messagePriority = (int)messagePriorityProperty.GetValue(messageLayout);
            // var ScrollMode = (int)ScrollSeed.GetValue(ScrollMode);
            // var scrollSpped = (int)ScrollSeed.GetValue(ScrollSpeed);
            // var PauseTime = (int)ScrollSeed.GetValue(interval);
            return new FullWindow
            {
                MessageType = 0x71,
                MessageLevel = (byte)messagePriority,
                MessageScroll = new ScrollInfo
                {
                    ScrollMode = 0x64,
                    ScrollSpeed = 05,
                    PauseTime = 10
                },
                MessageContent = new List<StringMessage>
        {
            new StringMessage { StringMode = 0x2A, StringBody = textStringBody }
        }
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
        /// 建立顯示序列物件，設定序列號、字體與消息內容。 多則訊息
        /// </summary>
        /// <param name="fullWindowMessage">全屏消息物件。</param>
        /// <returns>顯示序列物件。</returns>
        private Display.Sequence CreateDisplaySequence(List<FullWindow> fullWindowMessages)
        {
            return new Display.Sequence
            {
                SequenceNo = 1,
                Font = new FontSetting { Size = FontSize.Font24x24, Style = FontStyle.Ming },
                Messages = fullWindowMessages.Cast<IMessage>().ToList() // 正確處理 IMessage 接口
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
            return processor.CreatePacket(startCode, new List<byte> { 0x17, 0x18 }, new PassengerInfoHandler().FunctionCode, new List<Display.Sequence> { sequence });
        }
        /// <summary>
        /// 序列化封包並透過串口傳送封包資料。
        /// </summary>
        /// <param name="packet">要傳送的資料封包。</param>
        /// <returns>序列化的字節陣列。</returns> 
        private byte[] SerializeAndSendPacket(Packet packet)
        {
            var processor = new PacketProcessor();   
            var serializedData = processor.SerializePacket(packet);
            string result = BitConverter.ToString(serializedData).Replace("-", " ");
            ASI.Lib.Log.DebugLog.Log(_mProcName + " SendMessageToDisplay", "Serialized display packet: " + result);
            // _mSerial.Send(serializedData); 
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
        /// 處理訊息
        /// </summary>
        /// <param name="FireContentChinese"></param>
        /// <param name="FireContentEnglish"></param>
        /// <param name="situation"></param>
        /// <returns></returns>
        public async Task<Tuple<byte[], byte[], byte[]>> SendMessageToUrgnt(string FireContentChinese, string FireContentEnglish, int situation)
        {
            byte[] serializedDataChinese = new byte[] { };
            byte[] serializedDataEnglish = new byte[] { };
            byte[] serializedDataOff = new byte[] { };  // 新增存放關閉訊息的序列化數據

            try
            {
                // 設定警示的視為固定內容   
                var processor = new PacketProcessor();
                var startCode = new byte[] { 0x55, 0xAA }; 
                var function = new EmergencyMessagePlaybackHandler();
                var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, false);
                var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, true);
                // 序列化中文訊息   
                var sequence1 = CreateSequence(FireContentChinese, 1);
                var packet1 = processor.CreatePacket(startCode, new List<byte> { Convert.ToByte(front), Convert.ToByte(back) }, function.FunctionCode, new List<Display.Sequence> { sequence1 });
                serializedDataChinese = processor.SerializePacket(packet1);

                // 序列化英文訊息   
                var sequence2 = CreateSequence(FireContentEnglish, 2);
                var packet2 = processor.CreatePacket(startCode, new List<byte> { Convert.ToByte(front), Convert.ToByte(back) }, function.FunctionCode, new List<Display.Sequence> { sequence2 });
                serializedDataEnglish = processor.SerializePacket(packet2);

                // Optional delay and turn off if situation is 84  
                if (situation == 84)
                {
                    await Task.Delay(10000); // 延遲十秒 
                    var OffMode = new byte[] { 0x02 };
                    var packetOff = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, OffMode);
                    serializedDataOff = processor.SerializePacket(packetOff);
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("SendMessageToUrgnt", ex);
            }

            // 返回中文、英文和關閉訊息的序列化數據   
            return Tuple.Create(serializedDataChinese, serializedDataEnglish, serializedDataOff);
        }

        /// <summary>
        /// 顯示器的畫面開啟
        /// </summary>
        public void PowerSettingOpen()
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var Open = new byte[] { 0x3A, 0X00 };
            var packetOpen = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Open);
            var serializedDataOpen = processor.SerializePacket(packetOpen);
            _mSerial.Send(serializedDataOpen);
            ASI.Lib.Log.DebugLog.Log(_mProcName + "顯示畫面開啟", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen));
        }
        /// <summary>
        /// 顯示器的畫面關閉 
        /// </summary>
        public void PowerSettingOff()
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();  
            var function = new PowerControlHandler();
            var Off = new byte[] { 0x3A, 0X01 }; 
            var packetOff = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Off);
            var serializedDataOff = processor.SerializePacket(packetOff);
            _mSerial.Send(serializedDataOff);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面關閉", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
        }
        #endregion
        /// <summary>
        /// 建立緊急訊息的封包  放入訊息內容以及上下排 
        /// </summary>
        /// <param name="messageContent">訊息內容</param>
        /// <param name="sequenceNo"></param>
        /// <returns></returns>
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
        /// 將CMFT傳送過來的資料字串切割
        /// </summary>
        /// <param name="deviceString"></param>
        /// <returns></returns>
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


        #region 資料庫的method   
        /// <summary>
        /// 色碼轉換成byte
        /// </summary>
        /// <param name="colorName">顯示顏色</param>
        /// <returns></returns>
        private byte[] ProcessMessageColor(string colorName)
        {
            try
            {
                var ConfigDate = ASI.Wanda.DCU.DB.Tables.System.sysConfig.SelectColor(colorName);

                return DataConversion.FromHex(ConfigDate.config_value);
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("Error ProcessMessage ProcessMessageColor", ex);
                return null;
            }
        }
        /// <summary>
        /// 根據設備資訊從資料庫中取得當前正在播放的消息 ID。 
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
        private dmd_pre_record_message GetPreRecordedMessageLayoutById(Guid messageId)
        {
            var messageLayout = ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.SelectMessage(messageId);
            if (messageLayout == null)
                throw new InvalidOperationException($"無法找到消息 ID 為 {messageId} 的消息佈局。");
            return messageLayout;
        }

        /// <summary>
        /// 根據消息 ID 取得消息的佈局內容。
        /// </summary>
        /// <param name="messageId">消息 ID。</param>
        /// <returns>消息佈局物件。</returns>
        private dmd_instant_message GetInstantMessageLayoutById(Guid messageId)
        {
            var messageLayout = ASI.Wanda.DCU.DB.Tables.DMD.dmdInstantMessage.SelectMessage(messageId);
            if (messageLayout == null)
                throw new InvalidOperationException($"無法找到消息 ID 為 {messageId} 的消息佈局。");
            return messageLayout;
        }



        /// <summary>
        /// 找尋車站Id並且判斷是否需要關閉 
        /// </summary>
        /// <param name="messageID">車站</param>
        /// <returns></returns> 
        public dmdPowerSetting PowerSetting(string stationID)
        {
            var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);
            string[] notEcoDays = stationData.not_eco_day.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (stationData.eco_mode == "ON")
            {
                // 正在使用的月日及時分 
                var currentDate = DateTime.Now;
                int currentMonth = currentDate.Month;
                int currentDay = currentDate.Day;
                int currentHour = currentDate.Hour;

                // 使用 List 儲存不啟動節能模式的日期 
                var nonEcoDates = new List<(int Month, int Day)>();

                foreach (string day in notEcoDays)
                {
                    if (day.Length == 4)
                    {
                        int month = int.Parse(day.Substring(0, 2));
                        int dayOfMonth = int.Parse(day.Substring(2, 2));
                        nonEcoDates.Add((month, dayOfMonth));
                    }
                    else
                    {
                        // 處理長度不是 4 的情況，代表日期格式錯誤 
                        ASI.Lib.Log.ErrorLog.Log(_mProcName, "無效的日期格式：" + day);
                        continue;
                    }

                    // 檢查當前日期是否在不啟動節能的日期列表中 
                    if (nonEcoDates.Any(d => d.Month == currentMonth && d.Day == currentDay))
                    {
                        // 當前日期不啟動節能模式 
                        return null;
                    }

                    // 檢查開關顯示器的時間      
                    string[] autoPlayTimes = stationData.auto_play_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] autoEcoTimes = stationData.auto_eco_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    if (autoPlayTimes.Length == 2 && autoEcoTimes.Length == 2)
                    {
                        int autoPlayStartHour = int.Parse(autoPlayTimes[0]);
                        int autoPlayEndHour = int.Parse(autoPlayTimes[1]);
                        int autoEcoStartHour = int.Parse(autoEcoTimes[0]);
                        int autoEcoEndHour = int.Parse(autoEcoTimes[1]);

                        if (currentHour >= autoPlayStartHour && currentHour <= autoPlayEndHour)
                        {
                            // 關閉顯示器  
                            ASI.Lib.Log.DebugLog.Log(_mProcName, "關閉顯示器");
                            PowerSettingOff(); 
                        }
                        else if (currentHour >= autoEcoStartHour && currentHour <= autoEcoEndHour)
                        {
                            // 開啟顯示器
                            ASI.Lib.Log.DebugLog.Log(_mProcName, "開啟顯示器");
                            PowerSettingOpen();
                        }
                    }
                }
            }
            else
            {
                
            }

            return null;
        }



        #endregion
    }
}
