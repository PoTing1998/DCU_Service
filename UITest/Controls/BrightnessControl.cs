using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    public partial class BrightnessControl : UserControl
    {
        public Func<byte[], bool> SendAction { get; set; }

        public BrightnessControl()
        {
            InitializeComponent();
            UpdateManualVisibility();
        }

        // ── RadioButton 切換時顯示/隱藏手動控制 ──────────────────────────

        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            UpdateManualVisibility();
        }

        private void UpdateManualVisibility()
        {
            bool manual = rdoManual.Checked;
            lblManual.Visible    = manual;
            nudBrightness.Visible = manual;
            lblHint.Visible      = manual;
        }

        // ── 傳送 ──────────────────────────────────────────────────────────

        private void btnSend_Click(object sender, EventArgs e)
        {
            var mon = ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "Brightness", "傳送失敗：未設定 SendAction");
                MessageBox.Show("尚未連接串列埠。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] packet = BuildPacket();
            string desc   = rdoManual.Checked
                ? $"手動亮度 {nudBrightness.Value}"
                : "Sensor 自動亮度";

            mon.Log(ConnectionMonitor.LogLevel.Send, "Brightness",
                $"{desc}  HEX: {PacketBuilderService.ToHex(packet)}");

            bool ok = SendAction(packet);

            if (ok)
                mon.Log(ConnectionMonitor.LogLevel.Recv, "Brightness", $"亮度設定成功：{desc}");
            else
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "Brightness", "傳送失敗");
                MessageBox.Show("傳送失敗：串列埠未開啟。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 亮度設定封包：功能碼 0x32（參數設定）
        /// data[0] = 0x00（Sensor 自動）/ 0x01–0x10（手動 1–16）
        /// </summary>
        private byte[] BuildPacket()
        {
            byte val = rdoSensor.Checked
                ? (byte)0x00
                : (byte)nudBrightness.Value;

            byte funcCode = 0x32;
            byte[] data   = new byte[] { val };
            byte[] len    = System.BitConverter.GetBytes((ushort)data.Length);

            var result = new List<byte>();
            result.AddRange(new byte[] { 0x55, 0xAA });
            result.Add(0x00);
            result.Add(funcCode);
            result.AddRange(len);
            result.AddRange(data);
            result.Add(val); // checksum
            return result.ToArray();
        }
    }
}
