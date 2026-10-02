using System;
using TaskDU_Common.Constants;

namespace TaskDU_Common.Protocol
{
    /// <summary>
    /// CDU / SDU / PDN / PUP 共用的「來自 TaskPA 訊息」解析與回覆邏輯。
    /// 取代原本在 ProcTaskCDU / ProcTaskSDU / ProTaskPDN / TaskPUP.PAMessage 裡
    /// 幾乎逐字重複的 ProcessDataBytes / ProcessByteAtIndex2 / HandleCase01 / HandleCase15 / HexStringToBytes。
    ///
    /// 註：原本四份程式碼中 HandleCase15 的錯誤訊息文字語言不一致（CDU 是中文，SDU/PDN/PUP 是英文），
    /// 這裡統一採用中文，屬於刻意的統一（僅影響 log 文字，不影響通訊行為）。
    /// </summary>
    public class PAMessageHandler
    {
        private readonly string _procName;
        private readonly ASI.Lib.Comm.SerialPort.SerialPortLib _serial;
        private readonly Action<string, string, int> _sendUrgentMessage;

        /// <param name="procName">呼叫端的 _mProcName，用於 log 標記。</param>
        /// <param name="serial">呼叫端持有的序列埠物件，HandleCase01 需要用它回傳 ACK。</param>
        /// <param name="sendUrgentMessage">
        /// 呼叫端 Helper 類別（TaskCDUHelper/TaskSduHelper/TaskPDNHelper/TaskPUPHelper）
        /// 的 SendMessageToUrgnt(string, string, int) 方法。四個 Helper 目前簽章相同，
        /// 用委派傳入可以在不改動 Helper 類別的前提下重用這段邏輯。
        /// </param>
        public PAMessageHandler(string procName, ASI.Lib.Comm.SerialPort.SerialPortLib serial, Action<string, string, int> sendUrgentMessage)
        {
            _procName = procName;
            _serial = serial;
            _sendUrgentMessage = sendUrgentMessage;
        }

        /// <summary>
        /// 將十六進位字串轉換為位元組陣列。
        /// </summary>
        public static byte[] HexStringToBytes(string hex)
            => ASI.Lib.Msg.Parsing.ByteArray.HexStringToBytes(hex);

        /// <summary>
        /// 處理緊急（火警廣播）訊息。
        /// </summary>
        public void ProcessDataBytes(byte[] dataBytes)
        {
            byte dataByteAtIndex8 = dataBytes[8];
            switch (dataByteAtIndex8)
            {
                case 0x81:
                    _sendUrgentMessage(FireAlarmMessages.CheckChinese, FireAlarmMessages.CheckEnglish, 81);
                    break;
                case 0x82:
                    _sendUrgentMessage(FireAlarmMessages.EmergencyChinese, FireAlarmMessages.EmergencyEnglish, 82);
                    break;
                case 0x83:
                    _sendUrgentMessage(FireAlarmMessages.ClearedChinese, FireAlarmMessages.ClearedEnglish, 83);
                    break;
                case 0x84:
                    _sendUrgentMessage(FireAlarmMessages.DetectorChinese, FireAlarmMessages.DetectorEnglish, 84);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log(_procName + " ", $"{_procName} unknown byte value at index 9: {dataByteAtIndex8:X2}");
                    break;
            }
        }

        /// <summary>
        /// 依 dataBytes[2] 分派 ACK / 錯誤處理。
        /// </summary>
        public void ProcessByteAtIndex2(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            byte dataByte2 = dataBytes[2];

            switch (dataByte2)
            {
                case 0x01:
                    HandleCase01(dataBytes, sRcvTime, sJsonData);
                    break;
                case 0x06:
                    break;
                case 0x15:
                    HandleCase15(dataBytes, sRcvTime, sJsonData);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log($"{_procName} 收到來自 PA 的未知錯誤消息", sJsonData);
                    break;
            }
        }

        private void HandleCase01(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            dataBytes[2] = 0x06;
            Array.Resize(ref dataBytes, dataBytes.Length - 1);
            byte newLRC = ASI.Lib.Msg.Parsing.ByteArray.CalculateLRC(dataBytes);
            Array.Resize(ref dataBytes, dataBytes.Length + 1);
            dataBytes[dataBytes.Length - 1] = newLRC;
            _serial.Send(dataBytes); // 回傳 ACK 給 PA 設備
        }

        private void HandleCase15(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            string errorLog;
            switch (dataBytes[4])
            {
                case 0x01:
                    errorLog = "表示數據包長度錯誤";
                    break;
                case 0x02:
                    errorLog = "表示 LRC 錯誤";
                    break;
                case 0x03:
                    errorLog = "表示其他錯誤";
                    break;
                default:
                    errorLog = "表示未知錯誤";
                    break;
            }

            ASI.Lib.Log.DebugLog.Log($"{_procName} 在 {sRcvTime} 收到來自 TaskPA 的錯誤消息：{errorLog}", sJsonData);
        }
    }
}
