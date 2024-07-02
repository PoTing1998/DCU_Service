using ASI.Lib.DB;
using ASI.Wanda.PA.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static ASI.Wanda.PA.PA_Enum;

namespace ASI.Wanda.PA.Message
{


    public class Message : MsgPacket
    {

        #region Constractor
        /// <summary>
        /// 更改建構式
        /// </summary>
        public Message(MessageType TYP )
        {
            this.TYP = TYP;
        }
        /// <summary>
        /// 拆解回傳封包使用
        /// </summary>
        public Message()
        {

        }
       
        #endregion

        #region Property
       

        /// <summary>
        /// 0x10
        /// </summary>
        public byte DLE = 0x10;
        /// <summary>
        /// 文字開頭
        /// </summary>
        public byte STX = 0X02;

        /// <summary>
        /// SEQ
        /// </summary>
        public byte SEQ;
        /// 訊息代碼
        /// 訊息
        /// 正確
        /// 錯誤
        /// </summary>
        public enum MessageType : byte
        {
            msg = 0x01,
            correct = 0x06,
            error = 0x15
        }
        /// <summary>
        /// 錯誤訊息代碼
        /// 長度錯誤
        /// LRC錯誤
        /// 其他錯誤
        /// </summary>
        public enum Error_type : byte
        {
            length_error = 0x01,
            lrc_error = 0x02,
            other_error = 0x03
        }
        /// <summary>
        /// 封包代碼
        /// </summary>
        public MessageType TYP;
        /// <summary>
        /// 接收到錯誤代碼
        /// </summary>
        public Error_type Error_Type;


        /// <summary>
        /// 取得或設定長度TEXT的長度(數字) length)。
        /// 設定後可取得MessageLength
        /// </summary>
        public byte [] LEN
        {
            get
            {
                try
                {
                    int length = textMessage().Length;
                    return BitConverter.GetBytes((ushort)length); 
                }
                catch (Exception)
                {
                    throw;
                }
            }
            set
            {

            }
        }
        /// 傳送整體封包內容
        /// </summary>
        public byte[] textContent;

        public byte LRC { get; set; } = new byte();
        /// <summary>
        /// Message的byte[]原始資料   
        /// </summary>
        public byte[] DataByteArray = null;
        /// <summary>
        /// 文字結尾
        /// </summary>
        public byte ETX = 0X03;
        #endregion

        #region Function
        /// <summary>
        /// 根據輸入的值判斷目前狀況。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public byte GetErrorTypeFromValue(object inputValue)
        {
            if (Enum.IsDefined(typeof(Error_type), inputValue))
            {
                return (byte)(Error_type)inputValue;
            }
            // 如果無法映射到狀況，則返回默認值。
            return 0x00;
        }
        // 改成資料型態是byte的作法  
        
        /// <summary>
        /// 訊息封包的LRC 
        /// </summary>
        /// <returns></returns>
        public byte GetMsgLRC()
        {
            List<byte> oTempByteList = new List<byte>
            {
                DLE,
                STX,
                (byte)TYP,
                SEQ,
                LEN[0]
            };
            oTempByteList.AddRange(textMessage());
            byte bLRC = GetLRC(oTempByteList.ToArray());
            return bLRC;
        }

        /// <summary>
        /// 計算LRC
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static private byte GetLRC(byte[] text)
        {
            byte xor = 0;
            // if no data then done 
            if (text.Length <= 0)
                return 0;
            // incorporate remaining bytes into the value
            for (int i = 0; i < text.Length; i++)
                xor ^= text[i];
            return xor;
        }
        #endregion
    }
}
