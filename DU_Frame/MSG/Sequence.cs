using ASI.Lib.Process;
using ASI.Wanda.DMD;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

using static DuFrame.DUEnum;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
