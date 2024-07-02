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
        public byte CheckSum { get; set; }

        public byte[] ToBytes()
        {
            var idLength = (byte)IDs.Count;
            var dataBytes = Sequences.SelectMany(s => s.ToBytes()).ToArray();
            var lengthBytes = BitConverter.GetBytes((ushort)dataBytes.Length); // Low Byte first

            var result = new List<byte>();
            result.AddRange(StartCode); // Add StartCode
            result.Add(idLength); // Add ID length
            result.AddRange(IDs); // Add IDs
            result.Add(FunctionCode); // Add FunctionCode
            result.AddRange(lengthBytes); // Add data length
            result.AddRange(dataBytes); // Add data bytes


            // Calculate CheckSum
            CheckSum = (byte)(dataBytes.Sum(b => b) & 0xFF);
            result.Add(CheckSum); // Add CheckSum

            return result.ToArray();
        }

        public class CustomPacket
        {
            public byte[] StartCode { get; set; } = new byte[] { 0xAA, 0x55 }; // Start code: AAH 55H
            public List<byte> IDs { get; set; } = new List<byte>();
            public byte FunctionCode { get; set; }
            public byte[] Sequences { get; set; } = new byte[] { };
            public byte CheckSum { get; set; }

            public byte[] ToBytes()
            {
                var idLength = (byte)IDs.Count;
                var dataBytes = Sequences;
                var lengthBytes = BitConverter.GetBytes((ushort)dataBytes.Length); // Low Byte first
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(lengthBytes);
                }

                var result = new List<byte>();
                result.AddRange(StartCode); // Add StartCode
                result.Add(idLength); // Add ID length
                result.AddRange(IDs); // Add IDs
                result.Add(FunctionCode); // Add FunctionCode
                result.AddRange(lengthBytes); // Add data length
                result.AddRange(dataBytes); // Add data bytes

                // Calculate CheckSum
                CheckSum = (byte)(dataBytes.Sum(b => b) & 0xFF);
                result.Add(CheckSum); // Add CheckSum

                return result.ToArray();
            }
        }

    }
}



