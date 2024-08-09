using DuFrame.MSG;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static DuFrame.DUEnum;

namespace DuFrame.MSGHelper
{
    public class PacketHelper
    {
        public Packet packet;
        SequenceHelper sequenceHelper;
        public PacketHelper(object packet)
        {
            this.packet =(Packet)packet;
            sequenceHelper = new SequenceHelper(this.packet.sequence);
        }

        protected readonly byte[] StartCode = new byte[] { 0x55, 0xAA }; 


        #region public Method
        /// <summary>
        /// 取得視窗封包內容 
        /// </summary>
        /// <returns></returns> 
        public byte[] GetPacket(WindowDisplayMode _currentWindowType)
        {
            List<byte> oByteList = new List<byte>();
            AddPacketHeader(oByteList);
            AddPacketData(_currentWindowType, oByteList);
            return oByteList.ToArray();
        }
        /// <summary>
        /// 多種視窗
        /// </summary>
        /// <returns></returns>
        public byte[] GetPacket<T>(List<MSG.Message> messageList)
        {
            List<byte> oByteList = new List<byte>();  
            AddPacketHeader(oByteList);
            AddPacketDataMode(messageList, oByteList); 
            byte[] result = oByteList.ToArray();
            return result;
        }
        /// <summary>
        /// 將byte打包成封包的格式
        /// </summary>
        /// <returns></returns>
        public byte[] GetPacket()
        {
            List<byte> oByteList = new List<byte>();

            AddPacketHeader(oByteList); 
            packet.DataLength = GetdataLength();
            oByteList.AddRange(packet.DataLength);
            oByteList.AddRange(packet.DataContent);

            return oByteList.ToArray();
        }
        #endregion
        #region Private Method
        /// <summary>
        /// 計算ID長度並且轉成只有一個byte 
        /// </summary>
        private static byte[] GetIdLength(int id)
        {
            try
            {
                return new byte[] { (byte)(id & 0xFF) };
            }
            catch (Exception)
            {
                return new byte[1];
            }
        }

        /// <summary>
        /// 得到seq的內容轉化成長度 
        /// </summary>
        /// <returns></returns>
        private byte[] GetBytesFromShort(short value)
        {
            return BitConverter.GetBytes(value);
        }
        //計算長度 取得第一個byte   
        private byte[] GetdataLength()
        {
            byte[] lengthBytes = GetBytesFromShort((short)packet.DataContent.Length);  
            byte[] firstTwoBytes = lengthBytes.Take(2).ToArray();
            return firstTwoBytes;
        }
        /// <summary>
        /// 取得長度的byte
        /// </summary>
        private byte[] GetData_Msg_Length(WindowDisplayMode _currentWindowType)
        {
            packet.DataContent = sequenceHelper.GetSequence(_currentWindowType);
            //新增的方式  
            byte[] byteArray = BitConverter.GetBytes(packet.DataContent.Length);
            byte[] firstTwoBytes = byteArray.Take(2).ToArray();
            return firstTwoBytes;
        }
       
        /// <summary>
        /// 添加封包頭部
        /// </summary>
        /// <param name="byteList"></param>
        private void AddPacketHeader(List<byte> byteList)
        {
            byteList.AddRange(StartCode);
            packet.IDlength = GetIdLength(packet.ID.Length);
            byteList.AddRange(packet.IDlength);
            byteList.AddRange(packet.ID);
            byteList.Add((byte)packet.Function);
        }
        /// <summary>
        /// 添加封包數據，根據當前窗口類型   
        /// </summary>
        private void AddPacketData(WindowDisplayMode _currentWindowType, List<byte> byteList)
        {
            //判斷時鐘
            if (_currentWindowType == WindowDisplayMode.RightSide && packet.sequence.SequenceNo == 4)
            {
                
                packet.sequence.SequenceNo = 0;
                for (int i = 0; i < 2; i++)
                {
                    packet. sequence.SequenceNo++;
                    if (i == 0)
                    {
                        byte[] dataContent3 = sequenceHelper.GetSequence(_currentWindowType);
                        packet.sequence.SequenceNo++;
                        byte[] dataContent4 = sequenceHelper.GetSequence(_currentWindowType);
                        
                        packet.DataContent = dataContent3.Concat(dataContent4).ToArray();
                        var length = packet.DataContent.Length;
                        byte[] byteArray = BitConverter.GetBytes(length);
                        byte[] firstTwoBytes = byteArray.Take(2).ToArray();
                        packet.DataLength = firstTwoBytes;
                        byteList.AddRange(packet.DataLength);
                        byteList.AddRange(packet.DataContent);

                    }
                } 
            }
            else
            {
                packet.DataLength = GetData_Msg_Length(_currentWindowType);
                byteList.AddRange(packet.DataLength);
                byteList.AddRange(packet.DataContent);
            }
        }
        //多視窗組合
        private void AddPacketDataMode(List<MSG.Message> messageList, List<byte> byteList)
        {

            packet.DataContent = sequenceHelper.GetSequenceMore(messageList);
            int result = packet.DataContent.Length;
            packet.DataLength = GetBytesFromShort((short)result);
            byteList.AddRange(packet.DataLength);

            packet.DataContent = sequenceHelper.GetSequenceMore(messageList);

            byteList.AddRange(packet.DataContent);
        }

        #endregion
    }
}
