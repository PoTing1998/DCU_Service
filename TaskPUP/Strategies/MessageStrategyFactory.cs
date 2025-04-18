using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPUP.Constants;

namespace TaskPUP.Strategies
{

    public static class MessageStrategyFactory
    {
        public static IMessageStrategy GetStrategy(
            string objectName,
            string jsonData,
            string procName,
            ASI.Lib.Comm.SerialPort.SerialPortLib serial,
            string stationId,
            Action openDisplay,
            Action closeDisplay)
        {
            switch (objectName)
            {
                case Constants.Constants.SendPreRecordMessageSetting:
                case Constants.Constants.SendInstantMsg:
                    return new DisplayMessageStrategy(procName, serial);

                case Constants.Constants.SendPowerTimeSetting:
                    return new PowerSettingStrategy(procName, serial, stationId);

                case Constants.Constants.OpenDisplay:
                    return new EcoModeStrategy(openDisplay);

                case Constants.Constants.CloseDisplay:
                    return new EcoModeStrategy(closeDisplay);

                default:
                    return null;
            }
        }
    }
}
