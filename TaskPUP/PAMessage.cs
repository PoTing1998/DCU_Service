using ASI.Lib.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPUP.Constants;

namespace TaskPUP.PAMessage
{
    public  class PAMessage
    {
        private string _mProcName;
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;
        int mSEQ ;
        public PAMessage(string mProcName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _mProcName = mProcName;
            _mSerial = serial;
        }
        /// <summary> 
    /// 回傳給廣播
    /// </summary>
        void SendBroadcastMessage(ASI.Wanda.PA.Message.Message.MessageType messageType, byte station, byte platForm, byte situation)
        {
            ///組成封包內容
            ASI.Wanda.PA.Message.Message oPAMsg = new ASI.Wanda.PA.Message.Message(messageType);

            oPAMsg.station = station;
            oPAMsg.platform = platForm;
            oPAMsg.situation = situation;
            oPAMsg.SEQ = (byte)mSEQ++;
            oPAMsg.LRC = oPAMsg.GetMsgLRC();

            var arrPacketByte = ASI.Wanda.PA.Message.Helper.Pack(oPAMsg);
            string sHexString = ASI.Lib.Text.Parsing.String.BytesToHexString(arrPacketByte, " ");
            //MSMQ 回傳給TaskPA
            SendToTaskPA(2, 10, sHexString);
        }
        /// <summary> 
        /// 回傳給TaskPA  
        /// </summary>
        /// <param name="msgType"></param>   
        /// <param name="msgID"></param>    
        /// <param name="jsonData"></param> 
        static public void SendToTaskPA(int msgType, int msgID, string ContentDataBytes)
        {
            try
            {

                ASI.Wanda.DCU.ProcMsg.MSGFromTaskPUP MSGFromTaskPUP = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskPUP(new MSGFrameBase("taskPUP", "dcuservertaskpa"));

                MSGFromTaskPUP.MessageType = msgType;
                MSGFromTaskPUP.MessageID = msgID;
                MSGFromTaskPUP.JsonData = ContentDataBytes;
                ASI.Lib.Process.ProcMsg.SendMessage(MSGFromTaskPUP);

                // 假設 contentDataBytes 需要序列化為十六進制字符串  
                string serializedContent = ASI.Lib.Text.Parsing.Json.SerializeObject(ContentDataBytes);
                var msg = new ASI.Wanda.DCU.Message.Message(ASI.Wanda.DCU.Message.Message.eMessageType.Command, 01, serializedContent);
                msg.JsonContent = serializedContent;
                ASI.Lib.Log.DebugLog.Log("Sent packet to TaskPA", msg.JsonContent);

            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskPA", ex);
            }
        }

        /// <summary>
        /// 計算LRC 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>   
        private byte CalculateLRC(byte[] text)
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
        /// <summary>
        /// 處理廣播不同播放內容
        /// </summary>
        /// <param name="dataBytes"></param>
        public void ProcessDataBytes(byte[] dataBytes)
        {
            byte dataByteAtIndex8 = dataBytes[8];
            var taskPUPHelper = new ASI.Wanda.DCU.TaskPUP.TaskPUPHelper(_mProcName, _mSerial);
            switch (dataByteAtIndex8)
            {
                case 0x81:
                    taskPUPHelper.SendMessageToUrgnt(FireAlarmMessages.CheckChinese, FireAlarmMessages.CheckEnglish, 81);
                    break;
                case 0x82:
                    taskPUPHelper.SendMessageToUrgnt(FireAlarmMessages.EmergencyChinese, FireAlarmMessages.EmergencyEnglish, 82);
                    break;
                case 0x83:
                    taskPUPHelper.SendMessageToUrgnt(FireAlarmMessages.ClearedChinese, FireAlarmMessages.ClearedEnglish, 83);
                    break;
                case 0x84:
                    taskPUPHelper.SendMessageToUrgnt(FireAlarmMessages.DetectorChinese, FireAlarmMessages.DetectorEnglish, 84);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log(_mProcName + " ", $"{_mProcName} unknown byte value at index 9: {dataByteAtIndex8.ToString("X2")}");
                    break;
            }
        }
        public void ProcessByteAtIndex2(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            byte dataByte2 = dataBytes[2];
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} dataByte2: ", dataByte2.ToString("X2"));
            switch (dataByte2)
            {
                case 0x01:
                    HandleCase01(dataBytes, sRcvTime, sJsonData);
                    break;
                case 0x06:
                    ASI.Lib.Log.DebugLog.Log($"{_mProcName} received a correct message from TaskPA", sJsonData);
                    break;
                case 0x15:
                    HandleCase15(dataBytes, sRcvTime, sJsonData);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log($"{_mProcName} received an unknown error message from PA", sJsonData);
                    break;
            }
        }
        public void HandleCase01(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} processing 0x01 case", sJsonData);
            dataBytes[2] = 0x06;
            Array.Resize(ref dataBytes, dataBytes.Length - 1); // Remove the last byte
            byte newLRC = CalculateLRC(dataBytes);
            Array.Resize(ref dataBytes, dataBytes.Length + 1); // Add a byte back
            dataBytes[dataBytes.Length - 1] = newLRC;
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} replied to TaskPA message at {sRcvTime}", sJsonData);
        }
        public void HandleCase15(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} processing 0x15 case", sJsonData);
            string errorLog;
            switch (dataBytes[4])
            {
                case 0x01:
                    errorLog = "Indicates packet data length error";
                    break;
                case 0x02:
                    errorLog = "Indicates LRC error";
                    break;
                case 0x03:
                    errorLog = "Indicates other errors";
                    break;
                default:
                    errorLog = "Indicates unknown error";
                    break;
            }
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} received an error message from TaskPA: {errorLog} at {sRcvTime}", sJsonData);
        }

        /// <summary>
        /// 將十六進位字串轉換為位元組陣列。
        /// </summary>
        /// <param name="hex">要轉換的十六進位字串。</param>
        /// <returns>代表該十六進位字串的位元組陣列。</returns>
        public  byte[] HexStringToBytes(string hex)
        {
            var mProcName = "HexStringToBytes";
            try
            {
                // 移除所有空格和無效字符
                hex = hex.Replace(" ", "").Replace("\"", "").ToUpper();

                // 打印原始的十六進位字串
                ASI.Lib.Log.DebugLog.Log(mProcName, "HexStringToBytes 轉換前的字串: " + hex);

                if (hex.Length % 2 != 0)
                {
                    throw new ArgumentException("十六進位字串的長度必須是偶數");
                }
                // 確保所有字符都是有效的十六進位字符
                foreach (char c in hex)
                {
                    if (!Uri.IsHexDigit(c))
                    {
                        throw new ArgumentException($"十六進位字串包含無效字符: {c}");
                    }
                }

                // 創建一個字節陣列，長度為字串長度的一半
                byte[] bytes = new byte[hex.Length / 2];

                // 循環遍歷十六進位字串，將每對十六進位字符轉換為一個字節
                for (int i = 0; i < hex.Length; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                }

                return bytes;
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, "HexStringToBytes 轉換錯誤: " + ex.ToString());
                return null;
            }
        }
    }
}
