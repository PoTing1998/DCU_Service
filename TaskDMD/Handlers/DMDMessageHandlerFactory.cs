using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDMD.Handlers
{
    public class DMDMessageHandlerFactory
    {
        private readonly Dictionary<string, IDMDMessageHandler> _handlers;

        public DMDMessageHandlerFactory()
        {
            _handlers = new Dictionary<string, IDMDMessageHandler>
        {
            { ASI.Wanda.DMD.TaskDMD.Constants.SendPreRecordMsg, new SendPreRecordMessageHandler() },
            { ASI.Wanda.DMD.TaskDMD.Constants.SendInstantMsg, new SendInstantMessageHandler() },
            { ASI.Wanda.DMD.TaskDMD.Constants.ScheduleSetting, new ScheduleSettingHandler() },
            { ASI.Wanda.DMD.TaskDMD.Constants.PreRecordMessageSetting, new PreRecordMessageSettingHandler() },
            { ASI.Wanda.DMD.TaskDMD.Constants.TrainMessageSetting, new TrainMessageSettingHandler() },
            { ASI.Wanda.DMD.TaskDMD.Constants.GroupSetting, new GroupSettingHandler() },
            { ASI.Wanda.DMD.TaskDMD.Constants.PowerTimeSetting, new PowerTimeSettingHandler() }
           
        };
        }

        public IDMDMessageHandler GetHandler(string jsonObjectName)
        {
            IDMDMessageHandler handler;
            return _handlers.TryGetValue(jsonObjectName, out handler) ? handler : null;
        }
    }


}
