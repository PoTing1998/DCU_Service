using System;
using ASI.Lib.Process;

namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// 真實送出：透過委派送回 DMD Server，並以 MSMQ 送給其他 Task。
    /// </summary>
    public class RealDMDOutbound : IDMDOutbound
    {
        private readonly Func<ASI.Wanda.DMD.Message.Message, int> _sendToDmd;

        /// <param name="sendToDmd">實際送回 DMD Server 的方法，例如 msg => dmdApi.Send(msg)</param>
        public RealDMDOutbound(Func<ASI.Wanda.DMD.Message.Message, int> sendToDmd)
        {
            _sendToDmd = sendToDmd;
        }

        public void SendToDMD(ASI.Wanda.DMD.Message.Message message)
        {
            if (_sendToDmd == null)
            {
                ASI.Lib.Log.ErrorLog.Log("DMD_Service", "SendToDMD 未設定 DMD 送出委派，訊息未送出");
                return;
            }
            _sendToDmd(message);
        }

        public void SendToTask(string queueName, int msgType, int msgID, string jsonData)
        {
            try
            {
                var msg = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD(new MSGFrameBase("TaskDMD", queueName));
                msg.MessageType = msgType;
                msg.MessageID   = msgID;
                msg.JsonData    = jsonData;
                ASI.Lib.Process.ProcMsg.SendMessage(msg);
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskDMD", ex);
            }
        }
    }
}
