using ASI.Wanda.DMD.Message;

using DuFrame.MSGHelper;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DuFrame.DUEnum;

namespace DuFrame.MSG
{
    public class Packet 
    {
        #region property
        public Sequence sequence { get; set; }
        public DuFrame.DUEnum.Device Device { get; set; }
        public byte[] IDlength { get; set; }
        private byte[] id; // 私有字段，存儲原始的ID值
        public byte[] ID
        {
            get { return id; }
            set { id = value; IDDecimal = GetIDAsDecimal(value); } // 設定ID的同時轉換為十進位
        }
        public string IDDecimal { get; private set; } // 十進位表示的ID
        public DuFrame.DUEnum.Function Function { get; set; }

        public byte[] DataLength { get; set; }

        public byte[] DataContent { get; set; }
        private SequenceHelper sequenceHelper;
        #endregion
        #region constractor

        //一般模式
        public Packet(byte[] ID ,DuFrame.DUEnum.Function function)
        {
            this.ID = ID;
            Function = function;
        }
        //訊息模式
        public Packet(Sequence sequence, byte[] ID, DuFrame.DUEnum.Function function) 
        {
            this.sequence = sequence;

           
            this.ID =ID;
            Function = function;
            sequenceHelper = new SequenceHelper(sequence);
        }
        // 將byte[]轉換為十進位數字的方法
        private string GetIDAsDecimal(byte[] idBytes)
        {
            // 確保ID存在
            if (idBytes == null)
                return "ID is not set.";

            // 將byte[]轉換為十進位數字
            string idDecimal = BitConverter.ToString(idBytes);

            return idDecimal;
        }

        public string GetIDAsDecimal()
        {
            // 確保ID存在
            if (ID == null)
                return "ID is not set.";

            // 將byte[]轉換為十進位數字
            string idDecimal = BitConverter.ToString(ID);

            return idDecimal;
        }
        #endregion


    }

}



