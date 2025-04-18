using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPUP.Strategies
{
    public class PowerSettingStrategy : IMessageStrategy
    {
        private readonly string _procName;
        private readonly ASI.Lib.Comm.SerialPort.SerialPortLib _serial;
        private readonly string _stationId;

        public PowerSettingStrategy(string procName, ASI.Lib.Comm.SerialPort.SerialPortLib serial, string stationId)
        {
            _procName = procName;
            _serial = serial;
            _stationId = stationId;
        }

        public void Execute(string jsonData)
        {
            var helper = new ASI.Wanda.DCU.TaskPUP.TaskPUPHelper(_procName, _serial);
            helper.PowerSetting(_stationId);
        }
    }

}
