using System.Collections.Generic;

namespace ASI.Wanda.DMD.Service.Handlers
{
    public class DMDMessageHandlerFactory
    {
        private readonly Dictionary<string, IDMDMessageHandler> _handlers;

        public DMDMessageHandlerFactory()
        {
            _handlers = new Dictionary<string, IDMDMessageHandler>
            {
                { Constants.SendPreRecordMsg,        new SendPreRecordMessageHandler() },
                { Constants.SendInstantMsg,          new SendInstantMessageHandler() },
                { Constants.ScheduleSetting,         new ScheduleSettingHandler() },
                { Constants.PreRecordMessageSetting, new PreRecordMessageSettingHandler() },
                { Constants.TrainMessageSetting,     new TrainMessageSettingHandler() },
                { Constants.GroupSetting,            new GroupSettingHandler() },
                { Constants.PowerTimeSetting,        new PowerTimeSettingHandler() }
            };
        }

        public IDMDMessageHandler GetHandler(string jsonObjectName)
        {
            if (string.IsNullOrEmpty(jsonObjectName)) return null;
            return _handlers.TryGetValue(jsonObjectName, out var handler) ? handler : null;
        }
    }
}
