using ASI.Wanda.DMD.Message;

using DuFrame.MSGHelper;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DuFrame.DisplaySettingsEnums;

namespace DuFrame.MSG
{
    public class Packet 
    {
        #region property
        public Sequence sequence { get; set; }
        public Device Device { get; set; }
        public byte[] IDlength { get; set; }
        public byte[] ID { get; set; }
        public Function Function { get; set; }

        public byte[] DataLength { get; set; }

        public byte[] DataContent { get; set; }
        private SequenceHelper sequenceHelper;
        #endregion
        #region constractor

        //一般模式
        public Packet(byte[] ID , Function function)
        {
            this.ID = ID;
            Function = function;
        }
        //訊息模式
        public Packet(Sequence sequence, byte[] ID, Function function) 
        {
            this.sequence = sequence;
            this.ID = ID;
            Function = function;
            sequenceHelper = new SequenceHelper(sequence);
        }
        #endregion

     
    }

}



