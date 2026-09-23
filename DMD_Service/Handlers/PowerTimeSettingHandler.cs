namespace ASI.Wanda.DMD.Service.Handlers
{
    public class PowerTimeSettingHandler : IDMDMessageHandler
    {
        public void Handle(ASI.Wanda.DMD.Message.Message message, DMDHelper helper)
        {
            helper.UpdatePowerSetting();

            var obj = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PowerTimeSetting)
                ASI.Wanda.DMD.Message.Helper.GetJsonObject(message.JsonContent);

            var data = new ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PowerTimeSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                seatID     = obj.seatID,
                SqlCommand = obj.SqlCommand
            };
            helper.SendToAllTasks(data);
        }
    }
}
