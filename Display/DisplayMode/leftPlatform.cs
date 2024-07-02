using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.DisplayMode
{
        public class LeftPlatform : IMessage
        {
            public byte MessageType { get; set; }
            public byte MessageLevel { get; set; }
            public ScrollInfo MessageScroll { get; set; }
            public byte CommandType { get; set; } = 0x7A; 
            public byte RedColor { get; set; }
            public byte GreenColor { get; set; }
            public byte BlueColor { get; set; }
            public byte PhotoIndex { get; set; }
          //  public FontSetting Font { get; set; }
            public List<StringMessage> MessageContent { get; set; }

            // Assuming CommandData is a combination of RedColor, GreenColor, BlueColor, and Value (PhotoIndex in this case)
            public byte[] ToBytes()
            {
                // Construct CommandData as (RedColor, GreenColor, BlueColor, PhotoIndex)
                var commandData = new byte[] { CommandType, RedColor, GreenColor, BlueColor, PhotoIndex };

                // Convert MessageContent to bytes
                var contentBytes = MessageContent.SelectMany(c => c.ToBytes()).ToArray();
             //   var fontBytes = Font.ToBytes();
                var scrollBytes = MessageScroll.ToBytes();

                // Construct the final byte array
                var result = new List<byte> { MessageType };
                result.AddRange(commandData);   // Add CommandData directly after MessageType
                result.Add(MessageLevel);
             //   result.AddRange(fontBytes);
                result.AddRange(scrollBytes);
                result.AddRange(contentBytes);

                return result.ToArray();
            }
        }
    

}
