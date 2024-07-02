using DataConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskPDU
{
    public  class PDUHelper
    {

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

        static public byte[] CalculateChecksumForSingleRow(byte[] data, byte[] dataContent)
        {
            var dataWithChecksum = ByteConverter.AppendCheckSum(data, dataContent, out byte checksum);
            return dataWithChecksum;
        }
        #region 資料庫的method
      
        #endregion
    }
}
