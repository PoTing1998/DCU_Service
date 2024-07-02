using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.DisplayMode
{
    public class trainDynamic
    {

        public byte MessageType { get; set; }
        public byte MessageLevel { get; set; }
        public ScrollInfo MessageScroll { get; set; }
        public byte PhotoRedColor { get; set; }
        public byte PhotoGreenColor { get; set; }
        public byte PhotoBlueColor { get; set; }
        public byte PhotoIndex { get; set; }
        public byte TimeRedColor { get; set; }
        public byte TimeGreenColor { get; set; }
        public byte TimeBlueColor { get; set; }
        public byte StartValue { get; set; }
        public byte EndValue { get; set; }

        public List<StringMessage> MessageContent { get; set; }
        public byte MessageEnd { get; set; } = 0x1E;

        public byte[] ToBytes()
        {
            var contentBytes = MessageContent.SelectMany(c => c.ToBytes()).ToArray();
            var scrollBytes = MessageScroll.ToBytes();
            var length = (ushort)
                (1 + 1 + scrollBytes.Length +
                contentBytes.Length + 1); // MessageType + MessageLevel + ScrollInfo + MessageContent + MessageEnd

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
}
