using ASI.Wanda.DMD.TaskDMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDMD.Handlers
{
    public interface IDMDMessageHandler
    {
        void Handle(ASI.Wanda.DMD.Message.Message message, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> helper);
    }

}
