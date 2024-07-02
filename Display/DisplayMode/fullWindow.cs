using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.DisplayMode
{
    /// <summary>
    /// 全視窗，一般訊息設定
    /// </summary>
    public class FullWindow : IMessage
    {
        public byte MessageType { get; set; }
        public byte MessageLevel { get; set; }
        public ScrollInfo MessageScroll { get; set; } 
        public List<StringMessage> MessageContent { get; set; }
        public byte MessageEnd { get; set; } = 0x1E;

        public byte[] ToBytes() 
        {
            var contentBytes = MessageContent.SelectMany(c => c.ToBytes()).ToArray();
           
            var scrollBytes = MessageScroll.ToBytes();

            var length = (ushort)(1 + 1 + scrollBytes.Length + contentBytes.Length );

            // MessageType + MessageLevel + ScrollInfo + MessageContent 
            
            var lengthBytes = BitConverter.GetBytes(length);

            var result = new List<byte> { MessageType };
            
            result.AddRange(lengthBytes);
            result.Add(MessageLevel);
            result.AddRange(scrollBytes);
            result.AddRange(contentBytes);
            result.Add(MessageEnd);

            return result.ToArray();  

        }
    }
    public class ScrollInfo 
    {
        public byte ScrollMode { get; set; }
        public byte ScrollSpeed { get; set; }
        public byte PauseTime { get; set; }

        public byte[] ToBytes()
        {
            return new byte[] { ScrollMode, ScrollSpeed, PauseTime };
        }
    }
    public class FontSetting : IMessage
    {
        public byte MessageType { get; set; } = 0x7F; // Font setting command
        public FontSize Size { get; set; }
        public FontStyle Style { get; set; }

        public byte[] ToBytes()
        {
            return new byte[] { MessageType, (byte)Size, (byte)Style };
        }
    }







}
