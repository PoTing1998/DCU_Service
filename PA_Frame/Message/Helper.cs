using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Lib.DB;
using ASI.Wanda.PA.Message;

namespace ASI.Wanda.PA.Message
{
    public class Helper
    {
        /// <summary>
        /// 將ASI.Wanda.PA.Message.Message物件轉成byte[]封包
        /// </summary>
        /// <param name="PAmessage"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static byte[] Pack(Message PAmessage)
        {
            byte[] arrReturn = null;
            List<byte> oByteList = new List<byte>();
            try
            {
                oByteList.Add(PAmessage.DLE);
                oByteList.Add(PAmessage.STX);
                oByteList.Add((byte)PAmessage.TYP);
                oByteList.Add(PAmessage.SEQ);
                oByteList.Add(PAmessage.LEN[0]);
                PAmessage.textContent = PAmessage.textMessage();
                oByteList.AddRange(PAmessage.textContent);
                oByteList.Add(PAmessage.LRC);
                oByteList.Add(PAmessage.DLE);
                oByteList.Add(PAmessage.ETX);
                arrReturn = oByteList.ToArray();
                return arrReturn;
            }
            catch (Exception)
            {
                return arrReturn;
            }

        }


        /// <summary>
        /// 將byte[]封包轉換成ASI.Wanda.PA.Message.Message物件 
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        public static ASI.Wanda.PA.Message.Message UnPack(byte[] dataBytes)
        {
            Message oReturn = new PA.Message.Message();
            try
            {
                ///判斷識別碼 
                oReturn.TYP = (Message.MessageType)dataBytes[2];

                if (oReturn.TYP == Message.MessageType.msg)
                {
                    oReturn.LEN = ASI.Lib.Msg.Parsing.ByteArray.Capture(dataBytes, 4, 1);
                    oReturn.floor = oReturn.GetFloorFromValue(ASI.Lib.Msg.Parsing.ByteArray.Capture(dataBytes, 7, 1));
                    oReturn.fireMsg = oReturn.GetFirMSGFromValue(ASI.Lib.Msg.Parsing.ByteArray.Capture(dataBytes, 8, 1));
                }
                else if (oReturn.TYP == Message.MessageType.error)
                {
                    var Error = oReturn.GetErrorTypeFromValue(ASI.Lib.Msg.Parsing.ByteArray.Capture(dataBytes,4,1));
                    oReturn.LEN = ASI.Lib.Msg.Parsing.ByteArray.Capture(dataBytes, 5, 1);
                   
                }
                return oReturn;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary> 
        /// 將Packet的byte[]部分轉成ASI.Wanda.PA.Message.Packet物件
        /// 判別訊息中的內容
        /// </summary>
        /// <param name="packetData"></param>
        /// <returns></returns>
        public static MsgPacket GetMsgPacket(byte[] packetData)
        {
            MsgPacket msgPacket = new MsgPacket();
            try
            {   
                msgPacket.floor = msgPacket.GetFloorFromValue(packetData[2]);
                msgPacket.fireMsg = msgPacket.GetFirMSGFromValue(packetData[3]);

                return msgPacket;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }


}
