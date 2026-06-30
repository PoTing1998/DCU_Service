using System;

namespace TaskPUP.Strategies
{
    public class ScheduleMessageStrategy : IMessageStrategy
    {
        private readonly string _procName;
        private readonly ASI.Lib.Comm.SerialPort.SerialPortLib _serial;

        public ScheduleMessageStrategy(string procName, ASI.Lib.Comm.SerialPort.SerialPortLib serial)
        {
            _procName = procName;
            _serial = serial;
        }

        public void Execute(string jsonData)
        {
            try
            {
                var schedId    = ASI.Lib.Text.Parsing.Json.GetValue(jsonData, "sched_id");
                var sqlCmdStr  = ASI.Lib.Text.Parsing.Json.GetValue(jsonData, "SqlCommand");

                ASI.Lib.Log.DebugLog.Log(_procName,
                    $"處理排程訊息 schedId={schedId} SqlCommand={sqlCmdStr}");

                var helper = new ASI.Wanda.DCU.TaskPUP.TaskPUPHelper(_procName, _serial);
                helper.SendScheduleMessageToDisplay(schedId, sqlCmdStr);
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_procName, $"ScheduleMessageStrategy 執行失敗: {ex.Message}");
            }
        }
    }
}
