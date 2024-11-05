using Display.DisplayMode;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display
{
    public class Sequence
    {
        public byte SequenceNo { get; set; } //判斷上下行
        public List<IMessage> Messages { get; set; } = new List<IMessage>();
        public byte SequenceEnd { get; set; } = 0x1D; // 1Dh
        public FontSetting Font { get; set; } 
        public byte ClearCommand { get; set; } = 0x77;
        public byte UrgentCommand { get; set; }
        public bool IsUrgent { get; set; } // 新增一個屬性來表示是否緊急
        public byte[] ToBytes()
        {
            var messageBytes = Messages.SelectMany(m => m.ToBytes()).ToArray();
            var length = (ushort)(messageBytes.Length+2 ); // 訊息長度 + SequenceEnd 的長度 

            // 如果是緊急訊息，包含緊急指令
            if (IsUrgent)
            {
                length += 1; // 多加一個緊急指令的位元組長度  
            }
            else
            {
                var fontBytes = Font.ToBytes();
                length += (ushort)fontBytes.Length; // 加上字體設定的長度
            }

            var lengthBytes = BitConverter.GetBytes(length); // 低位元組在前

            var result = new List<byte>();

            // 如果是緊急訊息，包含緊急指令 
            if (IsUrgent)
            {
                result.Add(UrgentCommand); 
            }
            result.Add(SequenceNo);
            result.AddRange(lengthBytes);
            result.Add(ClearCommand); 

            if (!IsUrgent)
            {
                var fontBytes = Font.ToBytes();
                result.AddRange(fontBytes);
            } 

            result.AddRange(messageBytes);

            result.Add(SequenceEnd);

            return result.ToArray();
        }
    }
}
