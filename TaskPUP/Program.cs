using ASI.Lib.Config;
using ASI.Lib.Process;
using ASI.Wanda.DCU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskPUP
{
    class Program
    {
        static void Main(string[] args)
        {
            string qname = "TaskPUP";
            if (args.Length == 1) qname = args[0];

            IProcess TheProc = new ProcTaskPUP();
            if (TheProc.StartTask(ConfigApp.Instance.HostName, qname) >= 0)
            {
                TheProc.Run();
            }
            TheProc.StopTask();
        }
    }
}

