using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPUP.Strategies
{
    public class DisplayMessageStrategy : IMessageStrategy
    {
        private readonly string _procName;
        private readonly ASI.Lib.Comm.SerialPort.SerialPortLib _serial;

        public DisplayMessageStrategy(string procName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _procName = procName;
            _serial = serial;
        }

        public void Execute(string jsonData)
        {
            var helper = new ASI.Wanda.DCU.TaskPUP.TaskPUPHelper(_procName, _serial);
            var logData = new
            {
                Message = "收到來自TaskDMD的訊息",
                JsonData = jsonData,
                SeatID = ASI.Lib.Text.Parsing.Json.GetValue(jsonData, "seatID"),
                MsgID = ASI.Lib.Text.Parsing.Json.GetValue(jsonData, "msg_id"),
                TargetDU = ASI.Lib.Text.Parsing.Json.GetValue(jsonData, "target_du"),
                DbName1 = ASI.Lib.Text.Parsing.Json.GetValue(jsonData, "dbName1"),
                DbName2 = ASI.Lib.Text.Parsing.Json.GetValue(jsonData, "dbName2")
            };

            ASI.Lib.Log.DebugLog.Log(_procName, JsonConvert.SerializeObject(logData, Formatting.Indented));

            string result;
            helper.SendMessageToDisplay(logData.TargetDU, logData.DbName1, logData.DbName2, out result);
            ASI.Lib.Log.DebugLog.Log(_procName, $"處理結果：{result}");
        }
    }

}
