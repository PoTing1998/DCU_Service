using System.Collections.Generic;

namespace DuFrame.MSG
{
    public class Sequence 
    {
        #region  property
        public MSG.Message message;
        public List<MSG.Message> messages;
     
        public byte[] SequenceContent { get; set; }
        public byte[] SequenceLength { get; set; }

        // Sequence結束代碼
        protected readonly byte[] mSequenceEnd = new byte[] { 0x1D };

        #endregion
        #region constractor
        public int SequenceNo { get; set; }
        public Sequence (int sequenceNo , object MSG)
        {
            this.SequenceNo = sequenceNo;
            if (MSG is Message message) this.message = message;
            else if (MSG is List<MSG.Message> messages) this.messages = messages;
        }
        #endregion
    }
}
