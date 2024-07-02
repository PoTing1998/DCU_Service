using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display
{
    public class StringMessage
    {
        public byte StringMode { get; set; }
        public StringBody StringBody { get; set; }
        public  byte StringEnd { get; set; } = 0x1F; // 1Fh

        public byte[] ToBytes()
        {
            var bodyBytes = StringBody.ToBytes();
            var result = new List<byte> { StringMode };
            result.AddRange(bodyBytes);
            result.Add(StringEnd);
            return result.ToArray();
        }
    }

    public abstract class StringBody  
    {
        public abstract byte[] ToBytes();
    }

    // TextMode 靜態顯示模式和閃爍顯示模式
    public class TextStringBody : StringBody
    {
        public byte RedColor { get; set; }
        public byte GreenColor { get; set; }
        public byte BlueColor { get; set; }
        public string StringText { get; set; }
        
        public override byte[] ToBytes()
        {
            var textBytes = Encoding.GetEncoding("BIG5").GetBytes(StringText);
            var result = new List<byte> { RedColor, GreenColor, BlueColor };
            result.AddRange(textBytes);

            return result.ToArray();
        } 
    }

    // 預錄訊息顯示模式 
    public class PreRecordedTextBody : StringBody
    {
        public ushort IndexNumber { get; set; } // Low Byte first               
        public byte RedColor { get; set; }
        public byte GreenColor { get; set; }
        public byte BlueColor { get; set; }
        
        public override byte[] ToBytes()
        {
            var indexBytes = BitConverter.GetBytes(IndexNumber); 
            var result = new List<byte>(indexBytes) { RedColor, GreenColor, BlueColor };

            return result.ToArray();
        }
    }

    // 預錄圖片靜動態顯示模式  
    public class PreRecordedGraphicBody : StringBody
    {
        public ushort GraphicStartIndex { get; set; } // Low Byte first
        public byte GraphicNumber { get; set; }
        public byte RedColor { get; set; }
        public byte GreenColor { get; set; }
        public byte BlueColor { get; set; }
        
        public override byte[] ToBytes()
        {
            var indexBytes = BitConverter.GetBytes(GraphicStartIndex);
            var result = new List<byte>(indexBytes) { GraphicNumber, RedColor, GreenColor, BlueColor };

            return result.ToArray();
        }
    }

   


}
