using System;
using System.Collections.Generic;
using System.Text;
using ASI.Lib.Process;
using ASI.Lib.Config;
using ASI.Wanda.DCU.TaskMain;

namespace ASI.Wanda.DCU.TaskMain
{
	class Program
	{
        static void Main(string[] args)
        {
            string qname = "TaskMain";
            if (args.Length == 1) qname = args[0];

            IProcess TheProc = new ProcMain();
            if (TheProc.StartTask(ConfigApp.Instance.HostName, qname) >= 0)
            {
                TheProc.Run();
            }
            TheProc.StopTask();
        }
    }
}
