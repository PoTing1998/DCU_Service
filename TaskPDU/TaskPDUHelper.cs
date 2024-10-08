﻿
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

namespace ASI.Wanda.DCU.TaskPDU
{
    
    public static class Constants
    {
        public const string SendPreRecordMsg                = "ASI.Wanda.CMFT.JsonObject.DMD.FromCMFT.SendPreRecordMessage";
        public const string SendInstantMsg                  = "ASI.Wanda.CMFT.JsonObject.DMD.FromCMFT.SendInstantMessage";
        public const string SendScheduleSetting             = "ASI.Wanda.CMFT.JsonObject.DMD.FromCMFT.ScheduleSetting";
        public const string SendPreRecordMessageSetting     = "ASI.Wanda.CMFT.JsonObject.DMD.FromCMFT.PreRecordMessageSetting";
        public const string SendTrainMessageSetting         = "ASI.Wanda.CMFT.JsonObject.DMD.FromCMFT.TrainMessageSetting";
        public const string SendPowerTimeSetting            = "ASI.Wanda.CMFT.JsonObject.DMD.FromCMFT.PowerTimeSetting";
        public const string SendGroupSetting                = "ASI.Wanda.CMFT.JsonObject.DMD.FromCMFT.GroupSetting";
        public const string SendParameterSetting            = "ASI.Wanda.CMFT.JsonObject.DMD.FromCMFT.ParameterSetting";

      
    }
    public class DeviceInfo
    {
        public string Station { get; set; }
        public string Location { get; set; }
        public string DeviceWithNumber { get; set; }
    }
    public class TaskPDUHelper
    {
        static string StationID = ConfigApp.Instance.GetConfigSetting("Station_ID");
        private string _mProcName;
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;
        public TaskPDUHelper(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _mProcName = mProcName;
            _mSerial = serial;
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
        public void SendMessageToDisplay(string target_du, string dbName1, string dbName2 ,out string Result ,out byte[] DataByte)
        {
            Result = "";
            DataByte = null;
            try
            {
                var deviceInfo = SplitStringToDeviceInfo(target_du);
                ASI.Lib.Log.DebugLog.Log(_mProcName + "deviceInfo", $" received a message {deviceInfo.Station}  {deviceInfo.Location}  {deviceInfo.DeviceWithNumber}");
                
                if (deviceInfo != null) 
                {
                    var message_id = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemId(deviceInfo.Station, deviceInfo.Location, deviceInfo.DeviceWithNumber);
                    // Add your code here to use message_id   
                    var message_layout = ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.SelectMessage(message_id);

                    var fontColor = ProcessMEssageColor(message_layout.font_color);
                    //取得各項參數   
                    var processor = new PacketProcessor();

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
                    Result = "成功失敗";
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, ex.ToString());
                Result = "傳送失敗";
            }

        }
        /// <summary>
        /// 左測月台碼
        /// </summary>
        public void SendMessageToDisplay2(string target_du, string dbName1, string dbName2)
        {
            var deviceInfo = SplitStringToDeviceInfo(target_du);
            var processor = new PacketProcessor();

            var textStringBody = new TextStringBody
            {
                RedColor = 0xFF,
                GreenColor = 0xFF,
                BlueColor = 0xFF,
                StringText = "萬大線"
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

                // 序列化中文訊息
                var sequence1 = CreateSequence(FireContentChinese, 1);
                var packet1 = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Display.Sequence> { sequence1 });
                serializedDataChinese = processor.SerializePacket(packet1);

                // 序列化英文訊息
                var sequence2 = CreateSequence(FireContentEnglish, 2);
                var packet2 = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Display.Sequence> { sequence2 });
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
            var startCode   = new byte[] { 0x55, 0xAA };
            var processor   = new PacketProcessor();
            var function    = new PowerControlHandler();
            var Open        = new byte[] { 0x3A, 0X00 };
            var packetOpen  = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Open);
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
        /// <param name="messageContent"></param>
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


        #region 資料庫的method   
        /// <summary>
        /// 色碼轉換成byte
        /// </summary>
        /// <param name="colorName"></param>
        /// <returns></returns>
        private  byte[] ProcessMEssageColor(string colorName)
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

        /// <summary>
        /// 找尋車站Id並且判斷是否需要關閉
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public  dmdPowerSetting PowerSetting(string stationID)
        {
            var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);
            string[] notEcoDays = stationData.not_eco_day.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (stationData.eco_mode == "ON")
            {
                // 獲取當前日期的月和日以及現在的時間（時和分）
                int currentMonth = DateTime.Now.Month;
                int currentDay = DateTime.Now.Day;
                int currentHour = DateTime.Now.Hour;
                
                // 使用 List 儲存不啟動節能模式的日期 
                var nonEcoDates = new List<(int Month, int Day)>();

                foreach (string day in notEcoDays) 
                {
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
                        ASI.Lib.Log.ErrorLog.Log(_mProcName, "無效的日期格式：" + day);
                        continue; 
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
                // 不需要做任何處理  
            }

            return null;
        }
        #endregion
    }
}
