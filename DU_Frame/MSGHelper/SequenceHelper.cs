using ASI.Lib.Process;

using DuFrame.MSG;

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DuFrame.DUEnum;

namespace DuFrame.MSGHelper
{

    public class SequenceHelper
    {
        public Sequence sequence;

        public SequenceHelper(object sequence)
        {
            this.sequence = (Sequence)sequence;
        }
        // Sequence結束代碼
        protected readonly byte[] mSequenceEnd = new byte[] { 0x1D };

        #region public Method
        //文字基本架構封包格式  
        // 丟給上一層packet 
        public byte[] GetSequence(WindowDisplayMode _currentWindowType)
        {
            var oByteList = new List<byte>();
            try
            {
                //要加判斷是否開關緊急訊息  
                if (sequence.message.Emergency == WindowActionCode.EmergencyMessagePlaybackCount)
                {
                    if (sequence.message.emergencyMode == DUEnum.EmergencyCommand.Off)
                    {
                        oByteList.Add((byte)sequence.message.emergencyMode);
                    }
                    else
                    {
                        oByteList.Add((byte)sequence.message.emergencyMode);
                        oByteList.AddRange(OneByte(sequence.SequenceNo));
                        sequence.SequenceLength = GetMsgLength(_currentWindowType);
                        oByteList.AddRange(sequence.SequenceLength);
                        oByteList.AddRange(sequence.SequenceContent);
                    }
                }
                else
                {
                    oByteList.AddRange(OneByte(sequence.SequenceNo));
                    sequence.SequenceLength = GetMsgLength(_currentWindowType);
                    oByteList.AddRange(sequence.SequenceLength);
                    oByteList.AddRange(sequence.SequenceContent);
                }
                return oByteList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //處理多個版型
        public byte[] GetSequenceMore(List<MSG.Message> messages)
        {
            var oByteList = new List<byte>();
            try
            {
                oByteList.AddRange(OneByte(sequence.SequenceNo));
                var result = new List<byte>();
                foreach (MSG.Message m in messages)
                {
                    sequence.message = m;
                    result.AddRange(GetMessageContent(sequence.message.Version));
                }
                var bytes = new List<byte>
                {  
                    (byte)DUEnum.WindowActionCode.TextSettings,
                    (byte)sequence.message.FontSize,
                    (byte)sequence.message.FontStyle
                };
                var content = bytes.Concat(result).ToArray();
                content = content.Concat(mSequenceEnd).ToArray();
                sequence.SequenceLength = BitConverter.GetBytes((short)content.Length);
                sequence.SequenceContent = content;
                oByteList.AddRange(sequence.SequenceLength);
                oByteList.AddRange(sequence.SequenceContent);
            }
            catch (Exception) 
            {
                throw;
            }
            return oByteList.ToArray();
        }

        #endregion

        #region private Method
        // 獲取不同視窗內容的 byte 長度
        private byte[] GetMsgLength(WindowDisplayMode window)
        {
            if (sequence.message.Clock == DUEnum.clock.analogClock || sequence.message.Clock == DUEnum.clock.digitalClock)
            {
                var MsgContent = new List<byte>(GetMessageContent(window));
                sequence.SequenceContent = MsgContent.Concat(mSequenceEnd).ToArray();
            }
            else
            {
                var bytes = new List<byte>()
                {
                    //暫時移除 command的代碼 
                    //(byte)DUEnum.WindowActionCode.ClearScreen,  
                    (byte)DUEnum.WindowActionCode.TextSettings,
                    (byte)sequence.message.FontSize,
                    (byte)sequence.message.FontStyle
                };
                //取得內容的byte 且加上 End以及結合前面的command
                var MsgContent = new List<byte>(bytes.Concat(GetMessageContent(window)));
                sequence.SequenceContent = MsgContent.Concat(mSequenceEnd).ToArray();
            }

            return BitConverter.GetBytes((short)sequence.SequenceContent.Length);
        }


        // 獲取不同視窗內容的 byte 
        private byte[] GetMessageContent(WindowDisplayMode window)
        {
            var oMessage = new MessageGenerator(sequence.message, sequence.message.mstringMode);
            
            switch (window)
            { 
                case WindowDisplayMode.FullWindow: 
                    return (sequence.message.Emergency == WindowActionCode.EmergencyMessagePlaybackCount) ?
                        oMessage.GenerateEmergencyPacket() : oMessage.GenerateFullWindowPacket();
                case WindowDisplayMode.LeftSide:
                    return oMessage.GeneratePlatformMessageWindowPacket(); 
                case WindowDisplayMode.LeftAndRight:
                    return oMessage.GeneratePlatformTimeMessageWindowPacket();
                case WindowDisplayMode.RightSide:
                    return Determine(window);
                case WindowDisplayMode.TrainDynamic:
                    return oMessage.TrainDynamicLocationMessage();
                default:
                    return new byte[] { };
            }
        }
        /// <summary>
        /// 用於判斷 74類型的版型 
        /// </summary>
        private byte[] Determine(WindowDisplayMode windon)
        {
            var oMessage = new MessageGenerator(sequence.message, sequence.message.mstringMode);
            byte[] data;

            switch (windon)
            {
                case WindowDisplayMode.RightSide:
                    if (sequence.message.Clock == DUEnum.clock.analogClock || sequence.message.Clock == DUEnum.clock.digitalClock)
                        data = oMessage.GenerateClockMessageEffectWindowPacket(sequence.SequenceNo);
                    else if (sequence.message.TopCommandType == DUEnum.WindowActionCode.PictureOnLeft)
                        data = oMessage.GenerateTopPlatformAndTimeDisplayData();
                    else if (sequence.message.BottomCommandType == DUEnum.WindowActionCode.PictureDownLeft)
                        data = oMessage.GenerateDownPlatformAndTimeDisplayData();
                    else
                        data = oMessage.GenerateTimeMessageEffectWindowPacket();
                    break;
                default:
                    data = new byte[] { };
                    break;
            }
            return data;
        }
        public byte[] OneByte(int I)
        {
            byte[] arrByte = new byte[1];
            int Temp = I;
            //固定為1個byte，所以需先轉成short再轉成byte[]
            arrByte[0] = (byte)(Temp & 0xFF);
            return arrByte;
        }
        #endregion 
    }
}
