using ASI.Lib.Process;
using ASI.Wanda.DMD.ProcMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ASI.Wanda.PA.Message.Message;
using static ASI.Wanda.PA.PA_Enum;

namespace ASI.Wanda.DCU.TaskPA
{
    public class PAHelper
    {

        /// <summary> 
        /// 將PA的資料 傳給UPD看板 
        /// </summary>
        /// <param name="msgType"></param>   
        /// <param name="msgID"></param>    
        /// <param name="jsonData"></param> 
        static public void SendToTaskDMD(int msgType, int msgID, string ContentDataBytes)
        {
            try
            {
                ASI.Wanda.DMD.ProcMsg.MSGFromTaskPA MSGFromTaskPA = new ASI.Wanda.DMD.ProcMsg.MSGFromTaskPA(new MSGFrameBase("TaskPA", "dcuservertaskUPD"));
                MSGFromTaskPA.MessageType = msgType;
                MSGFromTaskPA.MessageID = msgID;
                MSGFromTaskPA.JsonData = ContentDataBytes;
                ASI.Lib.Process.ProcMsg.SendMessage(MSGFromTaskPA); 
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskPA", ex);
            }
        }
        static private byte calculateLRC(byte[] text)
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

  


    }


}
