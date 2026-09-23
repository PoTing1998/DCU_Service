namespace ASI.Wanda.DMD.Service.Handlers
{
    public class PreRecordMessageSettingHandler : IDMDMessageHandler
    {
        public void Handle(ASI.Wanda.DMD.Message.Message message, DMDHelper helper)
        {
            var obj = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PreRecordMessageSetting)
                ASI.Wanda.DMD.Message.Helper.GetJsonObject(message.JsonContent);

            var data = new ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PreRecordMessageSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                seatID     = obj.seatID,
                msg_id     = obj.msg_id,
                SqlCommand = obj.SqlCommand
            };

            helper.UpdateConfig();
            helper.UpdateDCUPlayList();
            helper.UpdateDCUPreRecordMessage();
            helper.SendToAllTasks(data);
        }
    }
}
