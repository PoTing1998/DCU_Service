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

        public abstract byte[] PrintString();
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
            var textBytes = Encoding.GetEncoding(950).GetBytes(StringText);
            var result = new List<byte> { RedColor, GreenColor, BlueColor };
            result.AddRange(textBytes);
            return result.ToArray();
        }
        public override byte[] PrintString()
        {
            // 將 StringText 轉換為 BIG-5 編碼的字節數組
            var textBytes = Encoding.GetEncoding(950).GetBytes(StringText);

            // 初始化結果列表並添加 RedColor, GreenColor, BlueColor
            var result = new List<byte> { RedColor, GreenColor, BlueColor };

            // 確保每個字符符合 ASCII 或 BIG-5 編碼
            foreach (var b in textBytes)
            {
                // 這裡你可以添加任何額外的驗證或處理邏輯
                result.Add(b);
            }

            // 返回最終的字節數組
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
        public override byte[] PrintString()
        {
            
            return null;
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
        public override byte[] PrintString()
        {
           
            return null;
        }
    }

   


}
