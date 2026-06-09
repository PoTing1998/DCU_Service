using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display
{
    public class Packet
    {
        public byte[] StartCode { get; set; } = new byte[] { 0xAA, 0x55 }; // Start code： AAH 55H
        public List<byte> IDs { get; set; } = new List<byte>();
        public byte FunctionCode { get; set; }
        public List<Sequence> Sequences { get; set; } = new List<Sequence>();
        /// <summary>
        /// 反序列化（解析收到的封包）時，記錄原始 CheckSum 以供驗證。
        /// 序列化（ToBytes）時此屬性不會被更新，CheckSum 由 ToBytes() 計算後直接附在封包尾端。
        /// </summary>
        public byte CheckSum { get; set; }

        public byte[] ToBytes()
        {
            var idLength = (byte)IDs.Count;
            var dataBytes = Sequences.SelectMany(s => s.ToBytes()).ToArray();
            var lengthBytes = BitConverter.GetBytes((ushort)dataBytes.Length); // Low Byte first

            var result = new List<byte>();
            result.AddRange(StartCode);    // Add StartCode
            result.Add(idLength);          // Add ID length
            result.AddRange(IDs);          // Add IDs
            result.Add(FunctionCode);      // Add FunctionCode
            result.AddRange(lengthBytes);  // Add data length (Little-Endian)
            result.AddRange(dataBytes);    // Add data bytes

            // CheckSum = sum of Data bytes only（規格定義）
            // 使用區域變數，不寫入 this.CheckSum，避免副作用
            var checkSum = CalculateCheckSum(dataBytes);
            result.Add(checkSum);

            return result.ToArray();
        }

        public class CustomPacket
        {
            public byte[] StartCode { get; set; } = new byte[] { 0xAA, 0x55 }; // Start code: AAH 55H
            public List<byte> IDs { get; set; } = new List<byte>();
            public byte FunctionCode { get; set; }
            public byte[] Sequences { get; set; } = new byte[] { };
            /// <summary>
            /// 反序列化時記錄原始 CheckSum；序列化時由 ToBytes() 計算，此屬性不會被更新。
            /// </summary>
            public byte CheckSum { get; set; }

            public byte[] ToBytes()
            {
                var idLength  = (byte)IDs.Count;
                var dataBytes = Sequences;
                // Little-Endian，與 Packet.ToBytes() 一致，不做 Reverse
                var lengthBytes = BitConverter.GetBytes((ushort)dataBytes.Length);

                var result = new List<byte>();
                result.AddRange(StartCode);    // Add StartCode
                result.Add(idLength);          // Add ID length
                result.AddRange(IDs);          // Add IDs
                result.Add(FunctionCode);      // Add FunctionCode
                result.AddRange(lengthBytes);  // Add data length (Little-Endian)
                result.AddRange(dataBytes);    // Add data bytes

                // CheckSum = sum of Data bytes only（規格定義），不寫入 this.CheckSum
                var checkSum = (byte)(dataBytes.Sum(b => b) & 0xFF);
                result.Add(checkSum);

                return result.ToArray();
            }
        }

        private byte CalculateCheckSum(byte[] dataContent)
        {
            int sum = 0;

            // 逐個字節相加
            foreach (var b in dataContent)
            {
                sum += b;
            }

            // 返回低8位，忽略高位進位
            return (byte)(sum & 0xFF);
        }


    }
}



