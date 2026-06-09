using System;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    public partial class DisplayModeControl : UserControl
    {
        /// <summary>
        /// 顯示模式代碼：
        /// 0x01 = 正常畫面
        /// 0x02 = 測試畫面
        /// 0x03 = 顯示ID碼與FW版本
        /// 0x04 = 顯示通訊逾時的畫面
        /// </summary>
        public enum DisplayMode : byte
        {
            Normal  = 0x01,
            Test    = 0x02,
            IDFW    = 0x03,
            Timeout = 0x04
        }

        /// <summary>由 Form1 注入：實際傳送 byte[]，回傳 true=成功</summary>
        public Func<byte[], bool> SendAction { get; set; }

        public DisplayModeControl()
        {
            InitializeComponent();
        }

        // ── 傳送 ──────────────────────────────────────────────────────────

        private void btnSend_Click(object sender, EventArgs e)
        {
            var mode = GetSelectedMode();
            var mon  = ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "DisplayMode",
                    "傳送失敗：尚未設定 SendAction");
                MessageBox.Show("尚未連接串列埠。", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 組成 Parameter Setting 封包（功能碼 0x32）
            byte[] packet = BuildPacket(mode);

            mon.Log(ConnectionMonitor.LogLevel.Send, "DisplayMode",
                $"設定顯示模式 → {mode}（0x{(byte)mode:X2}）  " +
                $"HEX: {Services.PacketBuilderService.ToHex(packet)}");

            bool ok = SendAction(packet);

            if (ok)
                mon.Log(ConnectionMonitor.LogLevel.Recv, "DisplayMode", $"顯示模式設定成功：{mode}");
            else
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "DisplayMode", "傳送失敗（串列埠未開啟）");
                MessageBox.Show("傳送失敗：串列埠未開啟。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── 讀取目前選項 ──────────────────────────────────────────────────

        private DisplayMode GetSelectedMode()
        {
            if (rdoTest.Checked)    return DisplayMode.Test;
            if (rdoIDFW.Checked)    return DisplayMode.IDFW;
            if (rdoTimeout.Checked) return DisplayMode.Timeout;
            return DisplayMode.Normal;
        }

        // ── 組成封包 ──────────────────────────────────────────────────────

        /// <summary>
        /// 顯示模式設定封包（功能碼 0x32 參數設定）
        /// 格式：StartCode(2) + ID_Len(1) + IDs + FuncCode(1) + DataLen(2) + Mode(1) + CheckSum(1)
        /// </summary>
        private static byte[] BuildPacket(DisplayMode mode)
        {
            byte funcCode = 0x32; // 參數設定 & 顯示模式切換
            byte modeVal  = (byte)mode;

            // data = 單一 byte（顯示模式代碼）
            byte[] data      = new byte[] { modeVal };
            byte[] lenBytes  = System.BitConverter.GetBytes((ushort)data.Length);
            byte   checkSum  = (byte)(modeVal & 0xFF);

            var result = new System.Collections.Generic.List<byte>();
            result.AddRange(new byte[] { 0x55, 0xAA }); // StartCode
            result.Add(0x00);                            // ID_Len = 0（廣播）
            result.Add(funcCode);
            result.AddRange(lenBytes);
            result.AddRange(data);
            result.Add(checkSum);

            return result.ToArray();
        }
    }
}
