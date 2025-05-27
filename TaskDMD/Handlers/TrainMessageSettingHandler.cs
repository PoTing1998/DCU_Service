using ASI.Wanda.DMD.TaskDMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDMD.Handlers
{
   public  class TrainMessageSettingHandler : IDMDMessageHandler
    {
        public void Handle(ASI.Wanda.DMD.Message.Message message, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> helper)
        {

            helper.UpDateDMDTrainMessage();
            var obj = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting)
                ASI.Wanda.DMD.Message.Helper.GetJsonObject(message.JsonContent);
            var data = new ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                msg_id = obj.msg_id,
                seatID = obj.seatID,
                SqlCommand = obj.SqlCommand
            };
            SendToAllTasks(helper, data);
        }
        private void SendToAllTasks<T>(TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper, T messageObject)
        {
            try
            {
                var serializedMessage = new ASI.Wanda.DCU.Message.Message(
                    ASI.Wanda.DCU.Message.Message.eMessageType.Command,
                    01,
                    ASI.Lib.Text.Parsing.Json.SerializeObject(messageObject)
                );

                DMDHelper.SendToTaskPUP(2, 1, serializedMessage.JsonContent);
                DMDHelper.SendToTaskCDU(2, 1, serializedMessage.JsonContent);
                DMDHelper.SendToTaskSDU(2, 1, serializedMessage.JsonContent);
                DMDHelper.SendToTaskPDN(2, 1, serializedMessage.JsonContent);
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("SendToAllTasks", $"序列化消息時發生錯誤: {ex.Message}");
            }
        }
    }
}
