using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    public partial class CountdownUnitControl : UserControl
    {
        public enum CountdownUnit : byte
        {
            Sec1  = 0x01,
            Sec5  = 0x05,
            Sec10 = 0x0A,
            Min1  = 0x3C   // 60 秒
        }

        public Func<byte[], bool> SendAction { get; set; }

        public CountdownUnitControl()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var  unit = GetSelectedUnit();
            var  mon  = ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "Countdown", "傳送失敗：未設定 SendAction");
                MessageBox.Show("尚未連接串列埠。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] packet = BuildPacket(unit);

            mon.Log(ConnectionMonitor.LogLevel.Send, "Countdown",
                $"倒數單位 → {GetLabel(unit)}  HEX: {PacketBuilderService.ToHex(packet)}");

            bool ok = SendAction(packet);

            if (ok)
                mon.Log(ConnectionMonitor.LogLevel.Recv, "Countdown", $"倒數單位設定成功：{GetLabel(unit)}");
            else
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "Countdown", "傳送失敗");
                MessageBox.Show("傳送失敗：串列埠未開啟。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private CountdownUnit GetSelectedUnit()
        {
            if (rdo1s.Checked)  return CountdownUnit.Sec1;
            if (rdo10s.Checked) return CountdownUnit.Sec10;
            if (rdo1m.Checked)  return CountdownUnit.Min1;
            return CountdownUnit.Sec5;
        }

        private static string GetLabel(CountdownUnit u)
        {
            switch (u)
            {
                case CountdownUnit.Sec1:  return "1 秒";
                case CountdownUnit.Sec10: return "10 秒";
                case CountdownUnit.Min1:  return "1 分鐘";
                default:                  return "5 秒";
            }
        }

        /// <summary>功能碼 0x7B（SplitWindowTimeMessageSetting）</summary>
        private static byte[] BuildPacket(CountdownUnit unit)
        {
            byte funcCode = 0x7B;
            byte val      = (byte)unit;
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
