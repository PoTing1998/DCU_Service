namespace ASI.Wanda.DMD.Service.Handlers
{
    public class GroupSettingHandler : IDMDMessageHandler
    {
        public void Handle(ASI.Wanda.DMD.Message.Message message, DMDHelper helper)
        {
            helper.UpdateGroup();

            var obj = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.GroupSetting)
                ASI.Wanda.DMD.Message.Helper.GetJsonObject(message.JsonContent);

            var data = new ASI.Wanda.DMD.JsonObject.DCU.FromDMD.GroupSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                group_id   = obj.group_id,
                seatID     = obj.seatID,
                SqlCommand = obj.SqlCommand
            };
            helper.SendToAllTasks(data);
        }
    }
}
