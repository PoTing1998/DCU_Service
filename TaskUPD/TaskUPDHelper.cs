using ASI.Wanda.DCU.DB.Models.DMD;
using ASI.Wanda.DCU.DB.Tables.DMD;

using Display;
using Display.DisplayMode;
using Display.Function;

using System;
using System.Collections.Generic;

namespace ASI.Wanda.DMD.TaskUPD
{
    class DeviceInfo
    {
        public string StationID { get; set; }
        public string AreaID { get; set; }
        public string DeviceID { get; set; }
    }
    public class TaskUPDHelper
    {

        static DeviceInfo SplitStringToDeviceInfo(string input)
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
        private string _mProcName;
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;
        public TaskUPDHelper(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _mProcName = mProcName;
            _mSerial = serial;
        }

        #region  版型的操作
        public void SendMessageToDisplay(string target_du, string dbName1, string dbName2)
        {
            var deviceInfo = SplitStringToDeviceInfo(target_du);
            if (deviceInfo != null)
            {
                var message_id = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemId(deviceInfo.StationID, deviceInfo.AreaID, deviceInfo.DeviceID);
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
                var packet = processor.CreatePacket(startCode, new List<byte> { 0x23, 0x24 }, function.FunctionCode, new List<Sequence> { sequence1 });
                var serializedData = processor.SerializePacket(packet);
                ASI.Lib.Log.DebugLog.Log(_mProcName + " SendMessageToDisplay", "Serialized display packet: " + BitConverter.ToString(serializedData));

                _mSerial.Send(serializedData);
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
                return DataConversion.ByteConverter.FromHex(ASI.Wanda.DCU.DB.Tables.System.sysConfig.PickColor(colorName));
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("Error ProcessMessage ProcessMessageColor", ex);
                return null;
            }
        }




    }
}

