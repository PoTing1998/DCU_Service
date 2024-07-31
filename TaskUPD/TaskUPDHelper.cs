using ASI.Wanda.DCU.DB.Models.DMD;
using ASI.Wanda.DCU.DB.Tables.DMD;

using Display;
using Display.DisplayMode;
using Display.Function;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskUPD
{
    class DeviceInfo
    {
        public string StationID { get; set; }
        public string AreaID { get; set; }
        public string DeviceID { get; set; }
    }
    public class TaskUPDHelper
    {
        private string _mProcName;
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;
        public TaskUPDHelper(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _mProcName = mProcName;
            _mSerial = serial;
        }
        static DeviceInfo SplitStringToDeviceInfo(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return null;

                string[] parts = input.Split('_');
                if (parts.Length == 3)
                {
                    return new DeviceInfo
                    {
                        StationID = parts[0],
                        AreaID = parts[1],
                        DeviceID = parts[2]
                    };
                }
                return null;
            }
            catch (Exception ex ) 
            {

                ASI.Lib.Log.ErrorLog.Log(input + " SendMessageToDisplay", "錯誤內容: " + ex.ToString());
                return null;    
            }
            
        }
        #region  版型的操作
        /// <summary>
        /// 一般版型控制
        /// </summary>
        /// <param name="target_du"></param>
        /// <param name="dbName1"></param>
        /// <param name="dbName2"></param>
        public void SendMessageToDisplay(string target_du, string dbName1, string dbName2)
        {
            try
            {
                ///判斷使否有這個裝置
                var deviceInfo = SplitStringToDeviceInfo(target_du);
               
                //依照 dbName 判斷要讀取哪個資料庫

                if (deviceInfo != null)
                {
                    dynamic message_layout = null;
                    var message_id = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemId(deviceInfo.StationID, deviceInfo.AreaID, deviceInfo.DeviceID);
                    if (dbName1 == "dmd_pre_record_message")
                    {
                        message_layout = ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.SelectMessage(message_id);
                    }
                    else if (dbName1 == "SendInstantMessage")
                    {
                        message_layout = ASI.Wanda.DCU.DB.Tables.DMD.dmdInstantMessage.SelectMessage(message_id);
                    }

                    if (message_layout != null)
                    {
                        // Add your code here to use message_id
                        // var message_layout = ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.SelectMessage(message_id);

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
                    }
                }
                else
                {
                    ASI.Lib.Log.DebugLog.Log(deviceInfo.ToString()+ _mProcName, target_du.ToString());
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName + " SendMessageToDisplay", "錯誤內容: " +ex.ToString());
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
            var deviceInfo = SplitStringToDeviceInfo(target_du);
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

                // Optional delay and turn off if situation is 84 
                if (situation == 84)
                {
                    await Task.Delay(10000); // 延遲十秒 
                    var OffMode = new byte[] { 0x02 };
                    var packetOff2 = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, OffMode);
                    var serializedDataOff2 = processor.SerializePacket(packetOff2);
                    ASI.Lib.Log.DebugLog.Log(_mProcName + " 解除緊急訊息", "Serialized display packet: " + BitConverter.ToString(serializedDataOff2));
                    _mSerial.Send(serializedDataOff2);
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("SendMessageToUrgnt", ex);
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
        private static byte[] ProcessMEssageColor(string colorName)
        {
            try
            {
                return DataConversion.FromHex(ASI.Wanda.DCU.DB.Tables.System.sysConfig.PickColor(colorName));
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("Error ProcessMessage ProcessMessageColor", ex);
                return null;
            }
        }




    }
}

