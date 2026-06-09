using System;
using System.Windows.Forms;

namespace UITest.Controls
{
    public partial class SystemControlPanel : UserControl
    {
        public SystemControlPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 讓 Form1 注入傳送 delegate 給「預錄封包直接傳送」頁面。
        /// </summary>
        public Func<byte[], bool> PreRecordSendAction
        {
            get => preRecordCtrl.SendAction;
            set => preRecordCtrl.SendAction = value;
        }
    }
}
