using System;
using System.Collections.Generic;
using System.Linq;
using DuFrame.MSG;
using static DuFrame.DUEnum;
namespace DuFrame.MSGHelper
{
    public class MessageGenerator
    {
        private readonly byte[] MessageEnd = new byte[] { 0x1E };
        public DuFrame.MSG.StringMode mstringMode;
        public Message message;
        public MessageGenerator(object message, object mstringMode)
        {
            this.message = (Message)message;

            this.mstringMode = (DuFrame.MSG.StringMode)mstringMode;
        }

        #region public Method

        /// <summary>
        /// 計算文字內容的長度，包括 MSGend 代碼
        /// </summary>
        /// <param name="messageData">要計算長度的文字數據</param>
        /// <returns>計算結果的字节数組</returns>
        private byte[] CalculateTextLength(byte[] messageData)
        {
            byte[] messageWithEnd = messageData.Concat(MessageEnd).ToArray();
            short messageLength = (short)messageWithEnd.Length;
            return BitConverter.GetBytes(messageLength);
        }

        /// <summary>
        /// 獲取圖片長度的一半  
        /// </summary>
        /// <param name="isUp">是否為上半部分圖片</param>
        /// <returns>圖片長度的字节数組</returns>
        public byte[] GetHalfPictureLength()
        {
            short length = (short)(message.PhotographDataContent.Length / 8 / 2);
            return BitConverter.GetBytes(length);
        }

        /// <summary>
        /// 視窗格式 1顯示(全視窗)，一般訊息設定。
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateFullWindowPacket()
        {
            var packet = new List<byte>();
            //依照多則訊息
            for (int i = 0; i < mstringMode.contentText.Count; i++)
            {
                if (!string.IsNullOrEmpty(mstringMode.contentText[i]))
                {
                    var messageContent = GetMessageContent(i);

                    var messageLength = CalculateTextLength(messageContent);

                    AppendDataToByteList(packet,WindowDisplayMode.FullWindow, messageLength, messageContent, MessageEnd);
                }
            }

            return packet.ToArray();
        }

        /// <summary>
        /// 視窗格式  2顯示(左側月台碼)，一般訊息設定。 
        /// </summary>
        /// <returns></returns>
        public byte[] GeneratePlatformMessageWindowPacket()
        {
            var packet = new List<byte>();
            packet.Add((byte)WindowActionCode.PlatformPictureMessageDisplay);
            packet.AddRange(message.Platformphoto);
            packet.AddRange(message.Platformphotoindex);
            for (int i = 0; i < mstringMode.contentText.Count; i++)
            {
                if (!string.IsNullOrEmpty(mstringMode.contentText[i]))
                {
                    var messageContent = GetMessageContent(i);
                    var messageLength = CalculateTextLength(messageContent);
                    AppendDataToByteList(packet,WindowDisplayMode.LeftSide, messageLength, messageContent, MessageEnd);
                }
            }
            return packet.ToArray();
        }
        /// <summary> 
        /// 視窗格式 3顯示(左側月台碼，右側時間顯示)，一般訊息設定。 
        /// </summary> 
        public byte[] GeneratePlatformTimeMessageWindowPacket()
        {
            var packet = new List<byte>();
            packet.Add((byte)WindowActionCode.PlatformPictureMessageDisplay);
            packet.AddRange(message.Platformphoto);
            packet.AddRange(message.Platformphotoindex);
            packet.Add((byte)WindowActionCode.SplitWindowTimeMessageSetting);
            packet.AddRange(message.TimeColor);
            packet.AddRange(message.TimeStart);
            packet.AddRange(message.TimeEnd);
            for (int i = 0; i < mstringMode.contentText.Count; i++)
            {
                if (!string.IsNullOrEmpty(mstringMode.contentText[i]))
                {
                    var messageContent = GetMessageContent(i);
                     var messageLength = CalculateTextLength(messageContent);
                    AppendDataToByteList(packet,WindowDisplayMode.LeftAndRight, messageLength, messageContent, MessageEnd);
                }
            }
            return packet.ToArray();
        }
        /// <summary>                                             
        /// 視窗格式 視窗格式 4顯示(右側時間顯示)，一般訊息效果顯示。   
        /// </summary> 
        /// <returns></returns>
        public byte[] GenerateTimeMessageEffectWindowPacket()
        {
            var packet = new List<byte>();

            packet.Add((byte)WindowActionCode.SplitWindowTimeMessageSetting);
            packet.AddRange(message.TimeColor);
            packet.AddRange(message.TimeStart);
            packet.AddRange(message.TimeEnd);
            for (int i = 0; i < mstringMode.contentText.Count; i++)
            {
                if (!string.IsNullOrEmpty(mstringMode.contentText[i]))
                {
                    var messageContent = GetMessageContent(i);
                    var messageLength = CalculateTextLength(messageContent);
                    AppendDataToByteList(packet,WindowDisplayMode.RightSide, messageLength, messageContent, MessageEnd);
                }
            }
            return packet.ToArray();
        }
        /// <summary>
        /// 時鐘模式
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateClockMessageEffectWindowPacket(int row)
        {
            var packet = new List<byte>();
            if (message.Clock == clock.digitalClock)//數位時鐘 
            {
                AddDigitalClockToPacket(packet);
            }
            else //類比時鐘
            {
                AddAnalogClockToPacket(packet, row);
            }
            byte[] FirstMessageContent = MessageFormattingHelper.GetStaticTextMessage(
                mstringMode.Level, mstringMode.ScrollMode, mstringMode.ScrollSpeed, 
                mstringMode.ScrollPauseTime, mstringMode.StringColor, mstringMode.contentText, 0);

            byte[] MessageLength = CalculateTextLength(FirstMessageContent);
            AppendDataToByteList(packet,WindowDisplayMode.RightSide, MessageLength, FirstMessageContent, MessageEnd);
            return packet.ToArray(); 
        }
        private void AddDigitalClockToPacket(List<byte> packet)
        {
            packet.Add((byte)WindowActionCode.ClockDisplay);
            packet.AddRange(message.ClockColor);
            packet.Add((byte)DUEnum.clock.digitalClock);
            AddTextSettingsToPacket(packet);
        }
        private void AddAnalogClockToPacket(List<byte> packet, int row)
        {
            getClockByte(message.PhotographDataContent);
            packet.Add((byte)WindowActionCode.ClockDisplay);
            packet.AddRange(message.ClockColor);
            packet.Add((byte)DUEnum.clock.analogClock);
            
            message.PhotographLength = GetHalfPictureLength();
            packet.AddRange(message.PhotographLength);

            if (row == 1 || row == 2)
            {
                packet.AddRange(row == 1 ? message.PhotographDataContentUp : message.PhotographDataContentDown);
                AddTextSettingsToPacket(packet);
            }
        }

        private void AddTextSettingsToPacket(List<byte> packet)
        {
            packet.Add((byte)WindowActionCode.TextSettings);
            packet.Add((byte)message.FontSize);
            packet.Add((byte)message.FontStyle);
        }
        /// <summary>
        /// 列車動態位置訊息
        /// </summary>
        /// <returns></returns>
        public byte[] TrainDynamicLocationMessage()
        {
            List<byte> packet = new List<byte>();
            byte[] MessageContent = MessageFormattingHelper.GetPreRecordedPhoto(
                mstringMode.Level , mstringMode.ScrollMode, mstringMode.ScrollSpeed, mstringMode.ScrollPauseTime
               , mstringMode.StringColor, mstringMode.GraphicStartIndex , mstringMode.GraphicNumber , mstringMode.GraphicStartIndex2
               , mstringMode.GraphicNumber2, mstringMode.GraphicColor, mstringMode.FirstStation , mstringMode.FirstStationMode
               , mstringMode.SecondStation, mstringMode.SecondStationMode, mstringMode.LocalStation , mstringMode.LocalStationMode
               );
            byte[] MessageLength = CalculateTextLength(MessageContent);
            AppendDataToByteList(packet, WindowDisplayMode.TrainDynamic, MessageLength, MessageContent, MessageEnd);
            return packet.ToArray();

        }
        /// <summary>
        /// 上排月台碼加上時間 
        /// </summary>
        public byte[] GenerateTopPlatformAndTimeDisplayData()
        {
            bool topSwitch = message.TopPlatformSwitch;
            var LeftSwitchMode = topSwitch ? new byte[] { 0x31 } : new byte[] { 0x30 };
            bool rightSwitch = message.TopClockSwitch;
            var RightSwitchMode = rightSwitch ? new byte[] { 0x31 } : new byte[] { 0x30 };

            var packet = new List<byte>();
            packet.Add((byte)(WindowActionCode.PictureOnLeft));
            packet.AddRange(LeftSwitchMode);
            packet.AddRange(message.TopPlatformColor);
            packet.AddRange(message.TopPlatformIndex);
            packet.AddRange(RightSwitchMode);
            packet.Add((byte)(WindowActionCode.SplitWindowTimeMessageSetting));
            packet.AddRange(message.TopClockColor);
            packet.AddRange(message.TopClockStartTime);
            packet.AddRange(message.TopClockEndTime);

            for (int i = 0; i < mstringMode.contentText.Count; i++)
            {
                if (!string.IsNullOrEmpty(mstringMode.contentText[i]))
                {
                    var messageContent = GetMessageContent(i);
                    var messageLength = CalculateTextLength(messageContent);
                    AppendDataToByteList(packet, WindowDisplayMode.RightSide, messageLength, messageContent, MessageEnd);
                }
            }
            return packet.ToArray();
        }
        /// <summary>
        /// 下排則左側標準時間右側為時間倒數
        /// </summary>
        public byte[] GenerateDownPlatformAndTimeDisplayData()
        {
            bool downSwitch = message.BottomClockSwitch;
            var switchMode = downSwitch ? new byte[] { 0x31 } : new byte[] { 0x30 };
            bool RightdownSwitch = message.BottomRightTimeSwitch;
            var RightSwitchMode = RightdownSwitch ? new byte[] { 0x31 } : new byte[] { 0x30 };

            var packet = new List<byte>();
            packet.Add((byte)(WindowActionCode.PictureDownLeft));
            packet.AddRange(switchMode);
            packet.AddRange(message.BottomLeftClockColor);
            packet.AddRange(RightSwitchMode);
            packet.Add((byte)(WindowActionCode.SplitWindowTimeMessageSetting));
            packet.AddRange(message.BottomRightClockColor);
            packet.AddRange(message.BottomRightClockStart);
            packet.AddRange(message.BottomRightClockEnd);
            for (int i = 0; i < mstringMode.contentText.Count; i++)
            {
                if (!string.IsNullOrEmpty(mstringMode.contentText[i]))
                {
                    var messageContent = GetMessageContent(i);
                    var messageLength = CalculateTextLength(messageContent);
                    AppendDataToByteList(packet, WindowDisplayMode.RightSide, messageLength, messageContent, MessageEnd);
                }
            }
            return packet.ToArray(); 
        }
        /// <summary>
        /// 緊急訊息封包
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateEmergencyPacket()
        {
            var packet = new List<byte>();
            packet.Add((byte)WindowActionCode.ClearScreen);
            packet.Add((byte)WindowActionCode.WarningLightAction);
            packet.AddRange(message.EmergencyLampStatus);
            packet.Add((byte)WindowActionCode.EmergencyMessagePlaybackCount);
            packet.AddRange(message.EmergencyTimes);

            for (int i = 0; i < mstringMode.contentText.Count; i++)
            {
                if (!string.IsNullOrEmpty(mstringMode.contentText[i]))
                {
                    var messageContent = GetMessageContent(i);
                    var messageLength = CalculateTextLength(messageContent);
                    AppendDataToByteList(packet, WindowDisplayMode.FullWindow, messageLength, messageContent, MessageEnd);
                }
            }
            return packet.ToArray();
        }

        #endregion

        #region private Mehod
        /// <summary>
        /// 訊息封包組成
        /// </summary>
        private void AppendDataToByteList(List<byte> byteList, WindowDisplayMode version, byte[] dataLength, byte[] data, byte[] endMarker)
        {
            byteList.Add((byte)version);
            byteList.AddRange(dataLength);
            byteList.AddRange(data);
            byteList.AddRange(endMarker);
        }
        /// <summary>
        /// 一般文字內容的組成格式
        /// </summary>
        private byte[] GetMessageContent(int index)
        {
            return MessageFormattingHelper.GetStaticTextMessage(
                 mstringMode.Level
                , mstringMode.ScrollMode
                , mstringMode.ScrollSpeed
                , mstringMode.ScrollPauseTime
                , mstringMode.StringColor
                , mstringMode.contentText
                , index
                 );
        }


        /// <summary>
        /// 位移轉換
        /// </summary>
        /// <param name="byte0"></param>
        /// <param name="byte1"></param>
        /// <param name="byte2"></param>
        /// <param name="byte3"></param>
        /// <param name="byte4"></param>
        /// <param name="byte5"></param>
        /// <param name="byte6"></param>
        /// <param name="byte7"></param>
        /// <returns></returns>
        byte zipData(bool byte0, bool byte1, bool byte2, bool byte3, bool byte4, bool byte5, bool byte6, bool byte7)
        {
            byte byteItem = 0x00;
            byte byteData = 0x01;
            if (byte7) byteItem = (byte)(byteItem | (byteData << 7));
            byteData = 0x01;
            if (byte6) byteItem = (byte)(byteItem | (byteData << 6));
            byteData = 0x01;
            if (byte5) byteItem = (byte)(byteItem | (byteData << 5));
            byteData = 0x01;
            if (byte4) byteItem = (byte)(byteItem | (byteData << 4));
            byteData = 0x01;
            if (byte3) byteItem = (byte)(byteItem | (byteData << 3));
            byteData = 0x01;
            if (byte2) byteItem = (byte)(byteItem | (byteData << 2));
            byteData = 0x01;
            if (byte1) byteItem = (byte)(byteItem | (byteData << 1));
            byteData = 0x01;
            if (byte0) byteItem = (byte)(byteItem | (byteData));
            return byteItem;
        }
        //取得圖片的byteArray
        void getClockByte(byte[] bytes)
        {
            try
            {
                ///讀取圖檔後的1維陣列
                bool[] bits = new bool[bytes.Length];
                for (int ii = 0; ii < bytes.Length; ii++)
                {
                    if (bytes[ii] != 0) { bits[ii] = true; }
                }
                byte[,] bytes1 = new byte[6, 48];
                ///直排的6個byte
                for (int i = 0; i < 6; i++)
                {   ///橫排的48byte
                    for (int j = 0; j < 48; j++)
                    {
                        int area = i * 384;
                        int[] index = new int[8];
                        ///每八個直排的處理成一個byte
                        for (int k = 0; k < 8; k++)
                        {
                            index[k] = area + k * 48 + j;
                        }
                        ///把處理完的值塞入2維陣列 
                        bytes1[i, j] = zipData(
                             bits[index[0]]
                            , bits[index[1]]
                            , bits[index[2]]
                            , bits[index[3]]
                            , bits[index[4]]
                            , bits[index[5]]
                            , bits[index[6]]
                            , bits[index[7]]);
                        ;
                    }
                }
                ///將圖檔的內容分為上下 
                message.PhotographDataContentUp = new byte[bytes1.Length / 2];
                message.PhotographDataContentDown = new byte[bytes1.Length / 2];
                int index2 = 0;
                int index3 = 0;
                for (int a = 0; a < 48; a++)
                {
                    for (int b = 0; b < 6; b++)
                    {
                        if (b < 3)
                        {
                            message.PhotographDataContentUp[index2] = bytes1[b, a];
                            index2++;
                        }
                        else
                        {
                            message.PhotographDataContentDown[index3] = bytes1[b, a];
                            index3++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


    }

}


