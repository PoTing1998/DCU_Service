using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.DisplayMode
{
    public  class Urgent : IMessage
    {

       
        public byte WarningLightSwitchSetting { get; set; } = 0x02;
        public byte PlayCountCommand { get; set; } = 0x80;
        public byte PlayCount { get; set; } = 0xFF;
    
        public byte MessageSwitch { get; set; }  
        public byte MessageType { get; set; }
        public byte UrgntMessageType { get; set; }
        public byte MessageLevel { get; set; }
        public ScrollInfo MessageScroll { get; set; }

        public FontSetting Font { get; set; } 
        public List<StringMessage> MessageContent { get; set; }
        public byte MessageEnd { get; set; } = 0x1E;

        public byte[] ToBytes()
        {
            var contentBytes = MessageContent.SelectMany(c => c.ToBytes()).ToArray();
            var fontBytes = Font.ToBytes();
            var scrollBytes = MessageScroll.ToBytes(); 

            var length = (ushort)(1 + 1  + scrollBytes.Length + contentBytes.Length  );

            // MessageType + MessageLevel + ScrollInfo + MessageContent + MessageEnd   

            var lengthBytes = BitConverter.GetBytes(length); 

            var result = new List<byte> { UrgntMessageType };  
            result.Add(WarningLightSwitchSetting);
            result.Add(PlayCountCommand); 
            result.Add(PlayCount);

            result.AddRange(fontBytes);
            result.Add(MessageType);
      
            result.AddRange(lengthBytes);
            result.Add(MessageLevel);
            result.AddRange(scrollBytes);
            result.AddRange(contentBytes);
            result.Add(MessageEnd);
            
            return result.ToArray();

        }

    }
}
