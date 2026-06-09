using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    public partial class AlarmLightControl : UserControl
    {
        public enum AlarmMode : byte
        {
            Off   = 0x00,   // 關閉
            On    = 0x01,   // 打開
            Blink = 0x02    // 閃爍
        }

        /// <summary>由 Form1 注入</summary>
        public Func<byte[], bool> SendAction { get; set; }

        public AlarmLightControl()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var  mode = GetSelectedMode();
            var  mon  = ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "AlarmLight", "傳送失敗：未設定 SendAction");
                MessageBox.Show("尚未連接串列埠。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] packet = BuildPacket(mode);

            mon.Log(ConnectionMonitor.LogLevel.Send, "AlarmLight",
                $"警示燈 → {mode}  HEX: {PacketBuilderService.ToHex(packet)}");

            bool ok = SendAction(packet);

            if (ok)
                mon.Log(ConnectionMonitor.LogLevel.Recv, "AlarmLight", $"警示燈設定成功：{mode}");
            else
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "AlarmLight", "傳送失敗");
                MessageBox.Show("傳送失敗：串列埠未開啟。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private AlarmMode GetSelectedMode()
        {
            if (rdoOff.Checked)   return AlarmMode.Off;
            if (rdoBlink.Checked) return AlarmMode.Blink;
            return AlarmMode.On;
        }

        /// <summary>
        /// 警示燈封包：功能碼 0x79（WarningLightAction）
        /// data[0] = 模式（0x00/0x01/0x02）
        /// </summary>
        private static byte[] BuildPacket(AlarmMode mode)
        {
            byte funcCode = 0x79;
            byte val      = (byte)mode;
            byte[] data   = new byte[] { val };
            byte[] len    = System.BitConverter.GetBytes((ushort)data.Length);
            byte   cs     = val;

            var result = new List<byte>();
            result.AddRange(new byte[] { 0x55, 0xAA });
            result.Add(0x00);
            result.Add(funcCode);
            result.AddRange(len);
            result.AddRange(data);
            result.Add(cs);
            return result.ToArray();
        }
    }
}
