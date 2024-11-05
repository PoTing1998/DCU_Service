using ASI.Lib.Config;
using ASI.Lib.Process;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskPDN
{
    class Program
    {
        static void Main(string[] args)
        {
            string qname = "TaskPDN";
            if (args.Length == 1) qname = args[0];

            IProcess TheProc = new ProcTaskPDN();
            if (TheProc.StartTask(ConfigApp.Instance.HostName, qname) >= 0)
            {
                TheProc.Run();
            }
            TheProc.StopTask();
        }
    }
}


