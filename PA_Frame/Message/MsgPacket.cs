using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace ASI.Wanda.PA.Message
{

    public class MsgPacket
    {
        #region Constractor



        public MsgPacket() { }
        #endregion

        #region Property
        /// <summary>
        /// 啟動訊號
        /// </summary>
        private const byte cmd = 0x01;
        /// <summary>
        /// 說明接續的多少個Byte有效   不含LRC
        /// </summary>
        private byte _mtextLength;
        public byte textLength
        {
            get => _mtextLength;
            set => _mtextLength = getLength();
        }
        /// <summary>
        /// 對應車站
        /// </summary>
        public byte station;
        /// <summary>
        /// 月台狀況
        /// </summary>
        public byte platform;
        /// <summary>
        /// 列車狀況
        /// </summary>
        public byte situation;
        /// <summary>
        /// 回傳代表的樓層
        /// </summary>
        public byte floor;
        /// <summary>
        /// 火災內容
        /// </summary>
        public byte fireMsg;
        #endregion

        #region function

        public byte getLength()
        {
            byte arrByte;
            List<byte> oByteList = new List<byte>()
                        {
                           station,
                           platform,
                           situation
                        };
            int i = oByteList.Count;
            arrByte = (byte)(i & 0XFF);

            return arrByte;
        }
        /// <summary>
        /// 訊息封包格式 
        /// </summary>
        /// <returns></returns>
        public byte[] textMessage()
        {
            List<byte> list = new List<byte>();
            list.Add(cmd);
            var length = getLength();
            list.Add(length);
            list.Add(station);
            list.Add(platform);
            list.Add(situation);
            return list.ToArray();
        }

        /// <summary>
        /// 根據輸入的值判斷目前車站。 
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public byte GetStationValue(string inputValue)
        {
            if (Enum.TryParse(inputValue, out PA_Enum.station station))
            {
                return (byte)station;
            }
            // If unable to map to a station, return default value. 
            return 0x00;
        }
      
        /// <summary>
        /// 根據輸入的值判斷目前月台。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public byte GetPlatformFromValue(string inputValue)
        {
            if (Enum.TryParse(inputValue, out PA_Enum.platform platform))
            {
                return (byte)platform;
            }
            // 如果無法映射到月台，則返回默認值。 
            return 0x00;
        }

        /// <summary> 
        /// 根據輸入的值判斷目前狀況。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public byte GetSituationFromValue(string inputValue)
        {
            if (Enum.TryParse(inputValue, out PA_Enum.situation situation))
            {
                return (byte)situation;
            }
            // 如果無法映射到狀況，則返回默認值。
            return 0x00;
        }
        /// <summary>
        /// 根據輸入的值判斷所在樓層。
        /// </summary>
        /// <param name="inputValue">要判斷的值。</param>
        /// <returns>樓層的位元組表示。</returns>
        public byte GetFloorFromValue(object inputValue)
        {
            if (Enum.IsDefined(typeof(PA_Enum.floor), inputValue))
            {
                return (byte)(PA_Enum.floor)inputValue;
            }
            // 如果無法映射到樓層，則返回默認值。
            return 0x00;
        }

        /// <summary>
        /// 根據輸入的值判斷內容
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public byte GetFirMSGFromValue(object inputValue)
        {
            if (Enum.IsDefined(typeof(PA_Enum.fireMsg), inputValue))
            {
                return (byte)(PA_Enum.fireMsg)inputValue;
            }
            // 如果無法映射到內容，則返回默認值。
            return 0x00;
        }
        #endregion 

    }



}
