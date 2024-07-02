using ASI.Lib.DB;

using DuFrame.MSG;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace DuFrame
{
    public class PacketContainer
    { 
        public PacketContainer(DuFrame.MSG.StringMode stringMode, object messages, Sequence sequence, Packet packet)
        {
            Packet = packet;
            Sequence = sequence;
            switch (messages) //判斷message的類別 
            {
                case Message msg: 
                    Message = msg; 
                    break;
                case List<Message> msgList:
                    Messages = msgList;
                    break;
                default:
                    // Use specific type in exception message  
                    throw new ArgumentException($"Invalid type for 'messages'. Expected Message or List<Message>, but received {messages.GetType()}.", nameof(messages));
            }
            StringMode = stringMode;
        }
        public Packet Packet { get; set; }
        public Sequence Sequence { get; set; }
        public DuFrame.MSG.Message Message { get; set; } 
        public DuFrame.MSG.StringMode StringMode { get; set; }
        public List<DuFrame.MSG.Message> Messages { get; set; }
  
    }

}
