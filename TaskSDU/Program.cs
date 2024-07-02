using ASI.Lib.Config;
using ASI.Lib.Process;
using ASI.Wanda.DCU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskSDU
{
    class Program
    {
        static void Main(string[] args)
        {
            string qname = "TaskSDU";
            if (args.Length == 1) qname = args[0];

            IProcess TheProc = new ProcTaskSDU();
            if (TheProc.StartTask(ConfigApp.Instance.HostName, qname) >= 0)
            {
                TheProc.Run();
            }
            TheProc.StopTask();
        }
    }
}

