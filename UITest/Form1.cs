using System.Windows.Forms;

namespace UITest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // 啟動時預設顯示「串列埠設定」Tab
            tabControl.SelectedTab = tabSerial;
            // 連接預錄封包的傳送 delegate 到串列設定的 SendData
            sysCtrlPanel.PreRecordSendAction = bytes => serialSettingCtrl.SendData(bytes);
        }
    }
}
