using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    public partial class DisplayPowerControl : UserControl
    {
        /// <summary>由 Form1 注入：實際傳送 byte[]</summary>
        public Func<byte[], bool> SendAction { get; set; }

        public DisplayPowerControl()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            bool powerOn = rdoOn.Checked;
            var  mon     = ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "DisplayPower", "傳送失敗：未設定 SendAction");
                MessageBox.Show("尚未連接串列埠。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] packet = BuildPacket(powerOn);

            mon.Log(ConnectionMonitor.LogLevel.Send, "DisplayPower",
                $"{(powerOn ? "打開" : "關閉")}顯示器  HEX: {PacketBuilderService.ToHex(packet)}");

            bool ok = SendAction(packet);

            if (ok)
                mon.Log(ConnectionMonitor.LogLevel.Recv, "DisplayPower",
                    $"顯示器{(powerOn ? "打開" : "關閉")}成功");
            else
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "DisplayPower", "傳送失敗");
                MessageBox.Show("傳送失敗：串列埠未開啟。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 開關機封包：功能碼 0x33
        /// data = 0x01（開機）/ 0x00（關機）
        /// </summary>
        private static byte[] BuildPacket(bool powerOn)
        {
            byte funcCode = 0x33;
            byte val      = powerOn ? (byte)0x01 : (byte)0x00;
            byte[] data   = new byte[] { val };
            byte[] len    = System.BitConverter.GetBytes((ushort)data.Length);
            byte   cs     = val;

            var result = new List<byte>();
            result.AddRange(new byte[] { 0x55, 0xAA });
            result.Add(0x00);       // ID_Len=0（廣播）
            result.Add(funcCode);
            result.AddRange(len);
            result.AddRange(data);
            result.Add(cs);
            return result.ToArray();
        }
    }
}
