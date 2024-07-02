using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskUPDU
{
 public   class UPDUHelper
    {
        /// <summary>
        /// 傳送port到看板上
        /// </summary>
        /// <param name="ports"></param>
        /// <param name="dataToSend"></param>
        static public void SendDataToAvailableSerialPorts(List<ASI.Lib.Comm.SerialPort.SerialPortLib> ports, byte[] dataToSend)
        {
            foreach (var port in ports)
            {
                // 判斷 SerialPortLib 是否開啟  
                if (port.IsOpen)
                {
                    // 在開啟的 SerialPortLib 上傳送資料 
                    port.Send(dataToSend);
                }
                else
                {
                    // 在這裡處理 SerialPortLib 未開啟的情況，例如顯示錯誤訊息或進行其他處理 
                    ASI.Lib.Log.ErrorLog.Log("錯誤信息", "SerialPort尚未開啟");
                }
            }
        }
        /// <summary>
        /// 計算CheckSum是否正確
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataContent"></param>
        /// <returns></returns>
        static public byte[] CalculateChecksumForSingleRow(byte[] data, byte[] dataContent)
        {
            var dataWithChecksum = DataConversion.Converter.AppendCheckSum(data, dataContent, out byte checksum);
            return dataWithChecksum;
        }
        #region 資料庫的method
        /// <summary>
        /// 取出關於廣播的內容  
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IEnumerable<ASI.Wanda.DCU.DB.Tables.PA.paContent> paContents(string ID)
        {
            try
            {
                var convertedList = ASI.Wanda.DCU.DB.Tables.PA.paContent.SelectContent(ID);
                var temp = ASI.Wanda.DCU.DB.Tables.PA.paContent.SelectAll();

                return convertedList.Cast<DCU.DB.Tables.PA.paContent>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
