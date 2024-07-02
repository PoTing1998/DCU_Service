using ASI.Lib.Config;
using ASI.Lib.Log;
using ASI.Lib.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskPA
{
    class Program
    {
        static void Main(string[] args)
        {
            string qname = "TaskPA";
            if (args.Length == 1) qname = args[0];
            
            IProcess TheProc = new ProcTaskPA();
            if (TheProc.StartTask(ConfigApp.Instance.HostName, qname) >= 0)
            {
                TheProc.Run();
            }
            TheProc.StopTask();
        }
    }
}
