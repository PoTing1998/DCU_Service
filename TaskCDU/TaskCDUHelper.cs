
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
using TaskDU_Common.Helpers;
using TaskDU_Common.Models;

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
    // DeviceInfo / DisplayMessageResult 已搬到 TaskDU_Common.Models，四個裝置共用。
    #endregion

    public class TaskCDUHelper : TaskDUHelperBase
    {
        #region constructor
        /// <summary>
        /// 本 Task 負責的裝置類型（CDU，例如 LG08A_CCS_CDU-1）。只比對類型，不限站別與編號：
        /// DMD Server target_du 裡凡是符合此類型的裝置都會處理。
        /// </summary>
        private static readonly Regex DeviceTypeRegex = new Regex(@"^[A-Z0-9]+_[A-Z]+_CDU-\d+$");

        private static string _currentDuId;

        /// <summary>
        /// 目前處理中的裝置 ID，由 DMD target_du 動態決定。
        /// 尚未收到任何 target_du 時（例如開機後先收到電源/排程訊息），改用本機 dulist 中第一台同類型裝置。
        /// </summary>
        public static string _mDU_ID
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentDuId)) return _currentDuId;
                try
                {
                    _currentDuId = ASI.Wanda.DCU.DB.Tables.DCU.dulist.SelectAll()
                        .Select(d => d.du_id == null ? null : d.du_id.Trim())
                        .FirstOrDefault(id => !string.IsNullOrEmpty(id) && DeviceTypeRegex.IsMatch(id));
                }
                catch { }
                return _currentDuId;
            }
        }

        /// <summary>
        /// 從 target_du（可能是 JSON 陣列字串或逗號分隔）挑出所有符合本 Task 類型的裝置。
        /// </summary>
        private static List<string> MatchDevices(IEnumerable<string> targets)
        {
            var list = new List<string>();
            if (targets == null) return list;
            foreach (var t in targets)
            {
                if (string.IsNullOrEmpty(t)) continue;
                foreach (var part in t.Split(new[] { ',', '[', ']', '"', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var id = part.Trim();
                    if (DeviceTypeRegex.IsMatch(id) && !list.Contains(id)) list.Add(id);
                }
            }
            return list;
        }

        public bool is_back = true; //顯示器的面板 正反面定義
        static string StationID = ConfigApp.Instance.GetConfigSetting("Station_ID");

        protected override string DuId => _mDU_ID;

        public TaskCDUHelper(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
            : base(mProcName, serial)
        {
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
            var results = new List<DisplayMessageResult>();
            var devices = MatchDevices(new[] { targetDu });
            foreach (var device in devices)
            {
                _currentDuId = device; // 之後的封包、面板查詢都用這台
                ASI.Lib.Log.DebugLog.Log(_mProcName, "處理裝置 " + device);
                results.AddRange(CreateAndSendMessage(device, dbName1, dbName2));
            }

            var successCount = results.Count(r => r.Result == "成功傳送");
            var failureCount = results.Count(r => r.Result != "成功傳送");
            var failedMessages = results.Where(r => r.Result != "成功傳送").ToList();

            // 統一生成回應結果
            result = successCount > 0
                ? $"成功處理 {successCount} 筆訊息，失敗 {failureCount} 筆。"
                : "所有訊息處理失敗。";

            // 每則訊息已在 CreateAndSendMessage 內透過 SerializeAndSendPacket 送出，此處不再重送（原本會重複送一次）。

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
            ASI.Lib.Log.ErrorLog.Log("信息處理錯誤", $"[Error] {message}");
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

                string[] deviceStrings = targetDu.Split(',');
                string matchedDevice = null;
                foreach (var deviceString in deviceStrings)
                {
                    string trimmedDevice = deviceString.Trim();
                    if (DeviceTypeRegex.IsMatch(trimmedDevice))
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
                else if (dbName1 == "dmd_pre_record_message")
                {
                    // 一則一則發送預錄訊息（最多五則）
                    const int MaxMessages = 5;
                    var limitedIds = messageIds.Take(MaxMessages).ToList();
                    foreach (var messageId in limitedIds)
                    {
                        var result = new DisplayMessageResult();
                        try
                        {
                            var messageLayout = GetPreRecordedMessageLayoutById(messageId);
                            var textStringBody = CreateTextStringBody(messageLayout);
                            var fullWindowMessage = CreateFullWindowMessage(textStringBody, messageLayout);
                            results.Add(SendSinglePreRecordMessage(matchedDevice, fullWindowMessage));
                        }
                        catch (Exception ex)
                        {
                            HandleError(ex, result);
                            results.Add(result);
                        }
                    }
                }
                else if (dbName1 == "dmd_train_message")
                {
                    // 批量處理列車訊息
                    var fullWindowMessages = new List<FullWindow>();
                    foreach (var messageId in messageIds)
                    {
                        var result = new DisplayMessageResult();
                        try
                        {
                            // TODO: 待決定列車訊息 layout 取得方式
                            // var messageLayout = GetTrainMessageLayoutById(messageId);
                            // var textStringBody = CreateTextStringBody(messageLayout);
                            // var fullWindowMessage = CreateFullWindowMessage(textStringBody, messageLayout);
                            // fullWindowMessages.Add(fullWindowMessage);
                        }
                        catch (Exception ex)
                        {
                            HandleError(ex, result);
                            results.Add(result);
                        }
                    }
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

        // ValidateInput / GetDeviceInfo 已移至 TaskDUHelperBase（TaskDU_Common），行為與原本完全相同。

        /// <summary>
        /// 處理即時訊息的發送邏輯，依 play_count 重複傳送（預設 3 次）。
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

                int playCount = messageLayout.play_count > 0 ? messageLayout.play_count : 3;
                for (int i = 0; i < playCount; i++)
                {
                    result.DataByte = SerializeAndSendPacket(packet);
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"即時訊息傳送 {i + 1}/{playCount}");
                }
                result.Result = "成功傳送";
            }
            catch (Exception ex)
            {
                HandleError(ex, result);
            }

            return result;
        }
        /// <summary>
        /// 單則預錄訊息發送邏輯，每則訊息各自建立 sequence 並分別發送。
        /// </summary>
        private DisplayMessageResult SendSinglePreRecordMessage(string matchedDevice, FullWindow fullWindowMessage)
        {
            var result = new DisplayMessageResult();
            try
            {
                var sequence = CreateDisplaySequence(fullWindowMessage);
                var packet = CreatePacket(_mDU_ID, sequence);
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
        /// 排程預錄訊息發送邏輯。
        /// insert / update：依 schedule_id 查出 message_id，一則一則發送（最多五則）。
        /// delete：清除畫面。
        /// </summary>
        public List<DisplayMessageResult> SendScheduleMessageToDisplay(string schedId, ASI.Wanda.DMD.Enum.SqlCommand sqlCommand)
        {
            var results = new List<DisplayMessageResult>();
            try
            {
                if (sqlCommand == ASI.Wanda.DMD.Enum.SqlCommand.delete)
                {
                    // 排程刪除 → 清除畫面
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"排程刪除，清除畫面 schedId={schedId}");
                    PowerSettingOff();
                    results.Add(new DisplayMessageResult { Result = "排程刪除，畫面已清除。" });
                    return results;
                }

                // insert / update → 取出排程內的訊息，一則一則發送
                var scheduleId = Guid.Parse(schedId);
                var messageIds = ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedulePlayList
                    .GetMessageIdsByScheduleId(scheduleId, _mDU_ID);

                if (!messageIds.Any())
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"排程 {schedId} 無對應訊息");
                    return results;
                }

                const int MaxMessages = 5;
                var limitedIds = messageIds.Take(MaxMessages).ToList();
                foreach (var messageId in limitedIds)
                {
                    var result = new DisplayMessageResult();
                    try
                    {
                        var messageLayout = GetPreRecordedMessageLayoutById(messageId);
                        var textStringBody = CreateTextStringBody(messageLayout);
                        var fullWindowMessage = CreateFullWindowMessage(textStringBody, messageLayout);
                        results.Add(SendSinglePreRecordMessage(_mDU_ID, fullWindowMessage));
                        ASI.Lib.Log.DebugLog.Log(_mProcName, $"排程訊息發送 messageId={messageId}");
                    }
                    catch (Exception ex)
                    {
                        HandleError(ex, result);
                        results.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                var result = new DisplayMessageResult { Result = "排程訊息發送失敗" };
                HandleError(ex, result);
                results.Add(result);
            }
            return results;
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
                result.Result = "成功傳送";
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
            {
                var missing = (fontColorProperty == null ? "font_color " : "")
                            + (messageContentProperty == null ? "message_content " : "")
                            + (messageContentEnProperty == null ? "message_content_en" : "");
                var msg = $"類型 {typeof(T).Name} 缺少必要屬性：{missing.Trim()}";
                ASI.Lib.Log.ErrorLog.Log("CreateTextStringBody", msg);
                throw new ArgumentException(msg);
            }

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
        // 單則 FullWindow 的 CreateDisplaySequence 已移至 TaskDUHelperBase。
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

        // CreatePacket / SerializeAndSendPacket / HandleError 已移至 TaskDUHelperBase。
        // CreatePacket 的 front/back 順序已統一為 { front, back }（與 CDU 原本行為相同，未改變）。
        // 每收到一則火警訊息就 +1；解除後延遲關閉前會比對，避免 10 秒內又來新警報卻被關掉
        private static int s_urgentGeneration = 0;

        /// <summary>
        /// 播放火警緊急訊息（內容來自 Config 的 FireAlarmMessages）。
        /// 播放次數：Urgent.PlayCount = 0xFF（無限播放），直到收到解除訊息為止。
        /// situation 81/82：警報，持續播放。
        /// situation 83/84：解除，播放解除訊息 10 秒後送出關閉緊急訊息指令。
        /// </summary>
        public Tuple<byte[], byte[], byte[]> SendMessageToUrgnt(string FireContentChinese, string FireContentEnglish, int situation)
        {
            byte[] serializedDataChinese = new byte[] { };
            byte[] serializedDataEnglish = new byte[] { };
            byte[] serializedDataOff = new byte[] { };

            try
            {
                var processor = new PacketProcessor();
                var startCode = new byte[] { 0x55, 0xAA };
                var function = new EmergencyMessagePlaybackHandler();
                var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, false);
                var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, true);
                var panelIds = ASI.Wanda.DCU.DB.Tables.DCU.dulist.ToPanelList(front, back);
                // 中文（上行）
                var packet1 = processor.CreatePacket(startCode, panelIds, function.FunctionCode,
                    new List<Display.Sequence> { CreateSequence(FireContentChinese, 1) });
                serializedDataChinese = processor.SerializePacket(packet1);

                // 英文（下行）
                var packet2 = processor.CreatePacket(startCode, panelIds, function.FunctionCode,
                    new List<Display.Sequence> { CreateSequence(FireContentEnglish, 2) });
                serializedDataEnglish = processor.SerializePacket(packet2);

                // 關閉緊急訊息指令（解除時使用）
                serializedDataOff = processor.SerializePacket(
                    processor.CreatePacketOff(startCode, panelIds, function.FunctionCode, new byte[] { 0x02 }));

                int generation = System.Threading.Interlocked.Increment(ref s_urgentGeneration);

                _mSerial.Send(serializedDataChinese);
                _mSerial.Send(serializedDataEnglish);
                ASI.Lib.Log.DebugLog.Log(_mProcName, $"火警緊急訊息播放（{situation}，無限次）：{FireContentChinese} / {FireContentEnglish}");

                bool isClear = situation == 83 || situation == 84;
                if (isClear)
                {
                    var serial = _mSerial;
                    var offBytes = serializedDataOff;
                    var procName = _mProcName;
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(10000);
                        if (System.Threading.Volatile.Read(ref s_urgentGeneration) != generation)
                        {
                            ASI.Lib.Log.DebugLog.Log(procName, "解除後 10 秒內收到新的火警訊息，取消關閉");
                            return;
                        }
                        serial.Send(offBytes);
                        ASI.Lib.Log.DebugLog.Log(procName, $"火警解除（{situation}），已關閉緊急訊息");
                    });
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("SendMessageToUrgnt", ex);
            }

            return Tuple.Create(serializedDataChinese, serializedDataEnglish, serializedDataOff);
        }
        // PowerSettingOpen / PowerSettingOff 已移至 TaskDUHelperBase。
        // 行為變更：原本 CDU 這兩個方法是送固定面板位元組 { 0x11, 0x12 }，
        // 現在改成跟 SDU 一樣動態查詢面板 ID（已確認 SDU 的寫法才是正確行為）。
        // 上線前請用現場設備驗證開關顯示器功能正常。
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
                PlayCountCommand = 0x80, // 播放次數指令
                PlayCount = 0xFF,        // 0xFF = 無限播放，直到收到解除（關閉）指令
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
        // SplitStringToDeviceInfo 已移至 TaskDUHelperBase。

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
        // GetPlayingItemIds 已移至 TaskDUHelperBase。

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

        // PowerSetting 已移至 TaskDUHelperBase（採用 CDU/SDU 版本的演算法，與 CDU 原本行為相同，未改變）。

        #endregion
    }
}
