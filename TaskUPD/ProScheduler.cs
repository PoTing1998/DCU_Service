using System;
using System.Timers;

namespace TaskUPD
{
    public class ProScheduler
    {

        private readonly Timer _timer;
        private readonly ASI.Wanda.DCU.TaskUPD.TaskUPDHelper _taskUPDHelper;

        public ProScheduler(ASI.Wanda.DCU.TaskUPD.TaskUPDHelper taskUPDHelper)
        {
            _taskUPDHelper = taskUPDHelper;

            // 设置定时器，单位为毫秒（600000毫秒 = 10分鐘）
            _timer = new Timer(600000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true; // 确保定时器重复执行
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                _taskUPDHelper.PowerSetting("LG01");
                ASI.Lib.Log.DebugLog.Log("PowerSettingScheduler", "PowerSetting executed at: " + DateTime.Now);
                
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("PowerSettingScheduler", "Error during PowerSetting: " + ex.ToString());
            }
        }


      
        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
