using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    public partial class CommTimeoutControl : UserControl
    {
        public Func<byte[], bool> SendAction { get; set; }

        public CommTimeoutControl()
        {
            InitializeComponent();
            UpdateVisibility();
        }

        private void rdo_CheckedChanged(object sender, EventArgs e)
            => UpdateVisibility();

        private void UpdateVisibility()
        {
            bool timeout = rdoTimeout.Checked;
            lblTimeVal.Visible  = timeout;
            nudTimeout.Visible  = timeout;
            lblMinute.Visible   = timeout;
            lblHint.Visible     = timeout;
            rdoSleep.Visible    = timeout;
            rdoPreset.Visible   = timeout;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var mon = ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "CommTimeout", "傳送失敗：未設定 SendAction");
                MessageBox.Show("尚未連接串列埠。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] packet = BuildPacket();
            string desc   = rdoContinue.Checked
                ? "持續顯示（不休眠）"
                : $"逾時 {nudTimeout.Value} 分鐘 → {(rdoSleep.Checked ? "休眠" : "預設訊息")}";

            mon.Log(ConnectionMonitor.LogLevel.Send, "CommTimeout",
                $"{desc}  HEX: {PacketBuilderService.ToHex(packet)}");

            if (SendAction(packet))
                mon.Log(ConnectionMonitor.LogLevel.Recv, "CommTimeout", $"設定成功：{desc}");
            else
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "CommTimeout", "傳送失敗");
                MessageBox.Show("傳送失敗：串列埠未開啟。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 功能碼 0x37（設備通訊更新）
        /// data[0] = 0x00 持續顯示 / 逾時分鐘數（1–60）
        /// data[1] = 動作：0x01 休眠 / 0x02 預設訊息（rdoContinue 時忽略）
        /// </summary>
        private byte[] BuildPacket()
        {
            byte timeVal = rdoContinue.Checked ? (byte)0x00 : (byte)nudTimeout.Value;
            byte action  = rdoContinue.Checked ? (byte)0x00
                         : rdoSleep.Checked    ? (byte)0x01 : (byte)0x02;

            byte[] data = new byte[] { timeVal, action };
            byte[] len  = System.BitConverter.GetBytes((ushort)data.Length);
            byte   cs   = (byte)((timeVal + action) & 0xFF);

            var r = new List<byte>();
            r.AddRange(new byte[] { 0x55, 0xAA });
            r.Add(0x00);
            r.Add(0x37);
            r.AddRange(len);
            r.AddRange(data);
            r.Add(cs);
            return r.ToArray();
        }
    }
}
