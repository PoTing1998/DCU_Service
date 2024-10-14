using ASI.Lib.Config;
using ASI.Wanda.DCU.DB.Models.DMD;
using ASI.Wanda.DCU.DB.Tables.DMD;

using Display;
using Display.DisplayMode;
using Display.Function;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static Display.DisplaySettingsEnums;

using static System.Net.Mime.MediaTypeNames;

namespace ASI.Wanda.DCU.TaskPUP
{
    public class TaskPUPHelper
    {
        private string _mProcName;
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;
        private static string _areaID = "UPF";
        private static string _deviceID = "PDU-1";
        private static string _stationID = "LG01";
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
        public TaskPUPHelper(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _mProcName = mProcName;
            _mSerial = serial;
        }
        /// <summary>
        /// 字串處理
        /// </summary>
        /// <param name="target_du"></param>
        /// <returns></returns>
        private Guid Target_du_StringHandle(string target_du)
        {
            List<Guid> messageIds = new List<Guid>();
            //value tuple
            //  List<Tuple<string, string, string>> date1 = new List<Tuple<string, string, string>>();
            var data = new List<(string, string, string)>();
            //收到字串模式target_du:["LG01_CCS_CDU-1", "LG01_CCS_CDU-2", "LG01_UPF_PDU-1", "LG08A_DPF_PDU-4"];
            target_du = target_du.Trim(new char[] { '[', ']', ' ' });
            target_du = target_du.Replace("\"", ""); // 去除所有的引號

            var items = target_du.Split(',');
            foreach (var item in items)
            {
                var parts = item.Split(new char[] { '_', '-' });

                if (parts.Length == 4)
                {
                    string stationID = parts[0];
                    string areaID = parts[1];
                    string deviceID = $"{parts[2]}-{parts[3]}";
                    // 檢查是否符合條件
                    if (stationID == _stationID && areaID == _areaID && deviceID == _deviceID)
                    {
                        // 符合條件，加入到 data 列表
                        data.Add((stationID, areaID, deviceID));
                        Guid messageId = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemId(stationID, areaID, deviceID);
                        messageIds.Add(messageId);
                    }
                }
            }
            ////value tuple
            //var data = new List<(string, string, string)>();
            ////收到字串模式target_du:["LG01_CCS_CDU-1", "LG01_CCS_CDU-2", "LG01_UPF_PDU-1", "LG08A_DPF_PDU-4"];
            //target_du = target_du.Trim(new char[] { '[', ']', ' ' });
            //target_du = target_du.Replace("\"", ""); // 去除所有的引號
            //var items = target_du.Split(',');
            //foreach (var item in items)
            //{
            //    var parts = item.Split(new char[] { '_', '-' });

            //    if (parts.Length == 4)
            //    {
            //        string stationID = parts[0];
            //        string areaID = parts[1];
            //        string deviceID = $"{parts[2]}-{parts[3]}";
            //        // 檢查是否符合條件
            //        // 檢查是否符合條件
            //        //if (string.Equals(stationID, _stationID, StringComparison.OrdinalIgnoreCase) &&
            //        //    string.Equals(areaID, _areaID, StringComparison.OrdinalIgnoreCase) &&
            //        //    string.Equals(deviceID, _deviceID, StringComparison.OrdinalIgnoreCase))

            //        //{
            //        //    ASI.Lib.Log.DebugLog.Log(_mProcName, $"Matching condition met: stationID={stationID}, areaID={areaID}, deviceID={deviceID}");
            //        //    // 符合條件，加入到 data 列表
            //        //    data.Add((stationID, areaID, deviceID));
            //        //    var messageId = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemId(stationID, areaID, deviceID);
            //        //    ASI.Lib.Log.DebugLog.Log(_mProcName, $"Message ID added: {messageId}");

            //        //}
            //        //else
            //        //{
            //        //    ASI.Lib.Log.DebugLog.Log(_mProcName, $"No match: stationID={stationID} (expected {_stationID}), areaID={areaID} (expected {_areaID}), deviceID={deviceID} (expected {_deviceID})");
            //        //}
            //    }
            //    else
            //    {
            //        ASI.Lib.Log.DebugLog.Log(_mProcName, $"Invalid item format: {item}. Expected format: stationID_areaID_deviceName-deviceNumber.");
            //    }
            //}
            return messageIds.FirstOrDefault();
        }

        #region  版型的操作

        public void judgeDbName(string name)
        {
            if ( name == "dmd_pre_record_message")
            {
                SendMessageToDisplay(name);
            }
            else if (name == "dmd_instant_message" )
            {
                SendMessageToDisplay(name);
            }
        }
        /// <summary>
        /// 一般版型控制
        /// </summary>
        /// <param name="target_du"></param>
        /// <param name="dbName1"></param>
        /// <param name="dbName2"></param>
       public void SendMessageToDisplay(string dbName1)
        {
            try
            {
                dmd_pre_record_message message_layout = new dmd_pre_record_message();
                var messageIdTest = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemId(_stationID, _areaID, _deviceID);
                if (dbName1 == "dmd_instant_message" || dbName1 == "dmd_pre_record_message")
                {
                    if (dbName1 == "dmd_pre_record_message")
                    {
                        message_layout = ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.SelectMessage(messageIdTest);
                    }
                    if (message_layout != null)
                    {
                        string color = message_layout.font_color;
                         var fontColor = ProcessColor(color);
                        //取得各項參數
                        var processor = new PacketProcessor();
                        var content = "萬大線";

                       // var fontColor = new byte[] { 0XFF, 0XFF, 0XFF };
                        var textStringBody = new TextStringBody
                        {
                            RedColor = fontColor[0],
                            GreenColor = fontColor[1],
                            BlueColor = fontColor[2],
                            StringText = content
                        };
                        var stringMessage = new StringMessage
                        {
                            StringMode = 0x2A, // TextMode (Static) 
                            StringBody = textStringBody
                        };
                        var fullWindowMessage = new FullWindow //Display version
                        {
                            MessageType = 0x71, // FullWindow message 
                            MessageLevel = (byte)message_layout.message_priority, //  level
                            MessageScroll = new ScrollInfo
                            {
                                ScrollMode = 0x64,
                                ScrollSpeed = (byte)message_layout.move_speed,
                                PauseTime = 10
                            },
                            MessageContent = new List<StringMessage> { stringMessage }
                        }; 
                        var sequence1 = new Display.Sequence
                        {
                            SequenceNo = 1,
                            Font = new FontSetting { Size = FontSize.Font24x24, Style = FontStyle.Ming },
                            Messages = new List<IMessage> { fullWindowMessage }
                        };
                        var startCode = new byte[] { 0x55, 0xAA };
                        var function = new PassengerInfoHandler(); // Use PassengerInfoHandler  
                        var packet = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Sequence> { sequence1 });
                        var serializedData = processor.SerializePacket(packet); 
                        ASI.Lib.Log.DebugLog.Log(_mProcName + " SendMessageToDisplay", "Serialized display packet: " + BitConverter.ToString(serializedData));
                        _mSerial.Send(serializedData);
                    }
                }
                else if (dbName1 == "dmd_schedule")
                {
                    var ID = ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedule.SelectScheduleISEnable();
                    var Message = ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedulePlayList.GetPlayingItemId(ID, _stationID, _deviceID);
                    message_layout = ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.SelectMessage(messageIdTest);
                    if (message_layout != null)
                    {
                        string color = message_layout.font_color;
                        //   var fontColor = ProcessColor(color);
                        //取得各項參數
                        var processor = new PacketProcessor();
                        var fontColor = new byte[] { 0XFF, 0XFF, 0XFF };
                        var textStringBody = new TextStringBody
                        {
                            RedColor = fontColor[0],
                            GreenColor = fontColor[1],
                            BlueColor = fontColor[2],
                            StringText = message_layout.message_content
                        };
                        var stringMessage = new StringMessage
                        {
                            StringMode = 0x2A, // TextMode (Static) 
                            StringBody = textStringBody
                        };
                        var fullWindowMessage = new FullWindow //Display version 
                        {
                            MessageType = 0x71, // FullWindow message 
                            MessageLevel = (byte)message_layout.message_priority, //  level
                            MessageScroll = new ScrollInfo
                            {
                                ScrollMode = 0x64,
                                ScrollSpeed = (byte)message_layout.move_speed,
                                PauseTime = 10
                            },
                            MessageContent = new List<StringMessage> { stringMessage }
                        };
                        var sequence1 = new Display.Sequence
                        {
                            SequenceNo = 1,
                            Font = new FontSetting { Size = FontSize.Font24x24, Style = FontStyle.Ming },
                            Messages = new List<IMessage> { fullWindowMessage }
                        };
                        var startCode = new byte[] { 0x55, 0xAA };
                        var function = new PassengerInfoHandler(); // Use PassengerInfoHandler 
                        var packet = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Sequence> { sequence1 });
                        var serializedData = processor.SerializePacket(packet);
                        ASI.Lib.Log.DebugLog.Log(_mProcName + " SendMessageToDisplay", "Serialized display packet: " + BitConverter.ToString(serializedData));
                        _mSerial.Send(serializedData);
                    }
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName + " SendMessageToDisplay", "錯誤內容: " + ex.ToString());
            }
        }
        /// <summary>
        /// 左測月台碼
        /// </summary>
        /// <param name="target_du"></param>
        /// <param name="dbName1"></param>
        /// <param name="dbName2"></param>
        void SendMessageToDisplay2(string target_du, string dbName1, string dbName2)
        {

            var processor = new PacketProcessor();

            var textStringBody = new TextStringBody
            {
                RedColor = 0xFF,
                GreenColor = 0xFF,
                BlueColor = 0xFF,
                StringText = "歡迎搭乘萬大線"
            };
            var stringMessage = new StringMessage
            {
                StringMode = 0x2A, // TextMode (Static)
                StringBody = textStringBody
            };
            var leftPlatform = new LeftPlatform //Display version 
            {
                MessageType = 0x72, // FullWindow message
                MessageLevel = 0x04, //  level 
                MessageScroll = new ScrollInfo { ScrollMode = 0x61, ScrollSpeed = 07, PauseTime = 10 },
                RedColor = 0xFF,
                GreenColor = 0xFF,
                BlueColor = 0xFF,
                PhotoIndex = 1,

                MessageContent = new List<StringMessage> { stringMessage }
            };
            var sequence1 = new Display.Sequence
            {
                SequenceNo = 1,
                Font = new FontSetting { Size = FontSize.Font24x24, Style = FontStyle.Ming },
                Messages = new List<IMessage> { leftPlatform }
            };

            var startCode = new byte[] { 0x55, 0xAA };
            var function = new PassengerInfoHandler(); // Use PassengerInfoHandler  
            var packet = processor.CreatePacket(startCode, new List<byte> { 0x01 }, function.FunctionCode, new List<Sequence> { sequence1 });
            var serializedData = processor.SerializePacket(packet);
            ASI.Lib.Log.DebugLog.Log(_mProcName + "送到看板上", "Serialized display packet: " + BitConverter.ToString(serializedData));
            _mSerial.Send(serializedData);
        }
        /// <summary>
        /// /緊急訊息
        /// </summary>
        /// <param name="FireContentChinese"></param>
        /// <param name="FireContentEnglish"></param>
        /// <param name="situation"></param>
        public async void SendMessageToUrgnt(string FireContentChinese, string FireContentEnglish, int situation)
        {
            try
            {
                // 設定警示的視為固定內容  
                var processor = new PacketProcessor();
                var startCode = new byte[] { 0x55, 0xAA };
                var function = new EmergencyMessagePlaybackHandler();

                // Send Chinese Message 
                var sequence1 = CreateSequence(FireContentChinese, 1);
                var packet1 = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Display.Sequence> { sequence1 });
                var serializedData1 = processor.SerializePacket(packet1);
                ASI.Lib.Log.DebugLog.Log(_mProcName + " SendMessageToUrgnt", "Serialized display packet: " + BitConverter.ToString(serializedData1));

                var temp = _mSerial.Send(serializedData1);
                ASI.Lib.Log.DebugLog.Log(" 是否傳送成功 " + _mProcName, temp.ToString());

                // Send English Message 
                var sequence2 = CreateSequence(FireContentEnglish, 2);
                var packet2 = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Display.Sequence> { sequence2 });
                var serializedData2 = processor.SerializePacket(packet2);
                ASI.Lib.Log.DebugLog.Log(_mProcName + " SendMessageToUrgnt", "Serialized display packet: " + BitConverter.ToString(serializedData2));
                _mSerial.Send(serializedData2);
                // Optional delay and turn off if situation is 84  
                if (situation == 84)
                {
                    await Task.Delay(10000); // 延遲五秒   
                    var OffMode = new byte[] { 0x02 };
                    var packetOff = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, OffMode);
                    var serializedDataOff = processor.SerializePacket(packetOff);
                    ASI.Lib.Log.DebugLog.Log(_mProcName + " 解除緊急訊息", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
                    _mSerial.Send(serializedDataOff);
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("SendMessageToUrgnt", ex);
            }
        }
        /// <summary>
        /// 找尋車站Id並且判斷是否需要關閉
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public dmdPowerSetting PowerSetting(string stationID)
        {
            try
            {
                //從資料庫讀取資料
                var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);
                string[] notEcoDays = stationData.not_eco_day.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (stationData.eco_mode == "on")
                {
                    // 獲取當前日期的月和日以及現在的時間（時和分）
                    int currentMonth = DateTime.Now.Month;
                    int currentDay = DateTime.Now.Day;
                    int currentHour = DateTime.Now.Hour; 
                    int currentMinute = DateTime.Now.Minute; 
                    // 使用 List 儲存不啟動節能模式的日期
                    var nonEcoDates = new List<(int Month, int Day)>();

                    foreach (string day in notEcoDays)
                    {
                        ASI.Lib.Log.DebugLog.Log("PowerSetting", day.ToString());
                        if (day.Length == 4)
                        {
                            int month = int.Parse(day.Substring(0, 2));
                            int dayOfMonth = int.Parse(day.Substring(2, 2));
                            nonEcoDates.Add((month, dayOfMonth));

                            // 檢查當前日期是否在不啟動節能模式的日期列表中
                            if (month == currentMonth && dayOfMonth == currentDay)
                            {
                                // 當前日期不啟動節能模式
                                continue;
                            }
                        }
                        else
                        {
                            // 處理長度不是 4 的情況，代表日期格式錯誤
                            Console.WriteLine("無效的日期格式：" + day);
                            continue;
                        }
                        // 檢查開關顯示器的時間
                        string[] autoPlayTimes = stationData.auto_play_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] autoEcoTimes = stationData.auto_eco_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        if (autoPlayTimes.Length == 1 && autoEcoTimes.Length == 1)
                        {
                            // 解析時間字串
                            int autoPlayHour = int.Parse(autoPlayTimes[0].Substring(0, 2));//開啟的時間單位 時
                            int autoPlayMinute = int.Parse(autoPlayTimes[0].Substring(3, 2));//開啟的時間單位 分
                            int autoEcoHour = int.Parse(autoEcoTimes[0].Substring(0, 2));//關閉的時間單位 時
                            int autoEcoMinute = int.Parse(autoEcoTimes[0].Substring(3, 2));//關閉的時間單位 分
                                                                                           // 判斷當前時間是否在自動播放時間範圍內 
                            if (currentHour == autoPlayHour && currentMinute == autoPlayMinute)
                            {
                                // 關閉顯示器
                                ASI.Lib.Log.DebugLog.Log(_mProcName, "關閉顯示器");
                                PowerSettingOff();
                            }
                            // 判斷當前時間是否在節能模式時間範圍內
                            else if (currentHour == autoEcoHour && currentMinute == autoEcoMinute)
                            {
                                // 開啟顯示器
                                ASI.Lib.Log.DebugLog.Log(_mProcName, "開啟顯示器");
                                PowerSettingOpen();
                            }
                            else
                            {
                                ASI.Lib.Log.DebugLog.Log(_mProcName, "當前時間不在自動播放或節能模式時間範圍內");
                            }
                        }
                    }
                }
                else
                {
                    // 不需要做任何處理
                    ASI.Lib.Log.DebugLog.Log("PowerSetting", "目前沒有開啟節能模式");
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
        public void PowerSettingOpen()
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var Open = new byte[] { 0x3A, 0X00 };
            var packetOpen = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Open);
            var serializedDataOpen = processor.SerializePacket(packetOpen);
            _mSerial.Send(serializedDataOpen);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 解除緊急訊息", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen));
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
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 解除緊急訊息", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
        }

        #region  資料庫的操作
        /// <summary>
        /// 更新DMDPreRecordMessage資料表    
        /// </summary>
        /// <returns></returns>    
        private static dmd_pre_record_message ProcessMessage(Guid messageID)
        {
            try
            {
                return dmdPreRecordMessage.SelectMSGSetting(messageID);
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("Error ProcessMessage ProcessMessage", ex);
                return null;
            }
            #endregion
        }
        /// <summary>
        /// 色碼轉換成byte
        /// </summary>
        /// <param name="colorName"></param>
        /// <returns></returns>
        public byte[] ProcessColor(string colorName)
        {
            try
            {
                var ConfigDate = ASI.Wanda.DCU.DB.Tables.System.sysConfig.SelectColor(colorName);
                ASI.Lib.Log.DebugLog.Log(_mProcName, ConfigDate.ToString());
                return DataConversion.FromHex(ConfigDate.config_value);
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("Error ProcessMessage ProcessMessageColor", ex);
                return null;
            }
        }


    }
}

