using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPUP.Strategies
{

    public class EcoModeStrategy : IMessageStrategy
    {
        private readonly Action _action;

        public EcoModeStrategy(Action action)
        {
            _action = action;
        }

        public void Execute(string jsonData)
        {
            _action?.Invoke();
        }
    }

}
