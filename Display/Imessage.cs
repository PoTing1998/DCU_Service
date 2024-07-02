using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display
{
    public interface IMessage
    {
        byte MessageType { get; set; }
        byte[] ToBytes();
    }

    public enum FontSize
    {
        Font16x16 = 0x21,
        Font24x24 = 0x22,
        Font5x7 = 0x23
    }

    public enum FontStyle
    {
        Ming = 0x31,
        Hei = 0x32,
        Kai = 0x33
    }

}
