namespace ASI.Wanda.DMD.Service.Handlers
{
    public class TrainMessageSettingHandler : IDMDMessageHandler
    {
        public void Handle(ASI.Wanda.DMD.Message.Message message, DMDHelper helper)
        {
            helper.UpdateTrainMessage();

            var obj = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting)
                ASI.Wanda.DMD.Message.Helper.GetJsonObject(message.JsonContent);

            var data = new ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                msg_id     = obj.msg_id,
                seatID     = obj.seatID,
                SqlCommand = obj.SqlCommand
            };
            helper.SendToAllTasks(data);
        }
    }
}
