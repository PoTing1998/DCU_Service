using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPUP.Strategies
{
    public interface IMessageStrategy
    {
        void Execute(string jsonData);
    }
}
