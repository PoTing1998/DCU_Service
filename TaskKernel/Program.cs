using System;
using System.Collections.Generic;
using System.Text;
using ASI.Lib.Process;
using ASI.Lib.Config;
using ASI.Wanda.DCU.TaskKernel;
using System.Diagnostics;

namespace ASI.Wanda.DCU.TaskKernel
{
	class Program
	{
		static void Main(string[] args)
		{
            string qname = "TaskKernel";
			if (args.Length == 1) qname = args[0];
                    
			IProcess TheProc = new ProcKernel();
			
			if (TheProc.StartTask(ConfigApp.Instance.HostName, qname) >= 0)
			{
				TheProc.Run();
			}
			TheProc.StopTask();
		}
	}
}
