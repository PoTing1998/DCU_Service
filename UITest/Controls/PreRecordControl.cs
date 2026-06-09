using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UITest.Controls
{
    public partial class PreRecordControl : UserControl
    {
        /// <summary>
        /// 由外部（Form1）注入的傳送 delegate，接受 byte[]，回傳是否成功。
        /// </summary>
        public Func<byte[], bool> SendAction { get; set; }

        public PreRecordControl()
        {
            InitializeComponent();
        }

        // ── 傳送 ────────────────────────────────────────────
        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] data;
            if (!TryParseHex(txtHex.Text, out data))
            {
                SetStatus("格式錯誤：請輸入以空白分隔的 16 進位數值（如 55 AA 01）", error: true);
                return;
            }

            if (SendAction == null)
            {
                SetStatus("尚未連接串列埠", error: true);
                return;
            }

            bool ok = SendAction(data);
            if (ok)
                SetStatus($"已傳送 {data.Length} bytes", error: false);
            else
                SetStatus("傳送失敗：串列埠未開啟", error: true);
        }

        // ── ASCII Code ──────────────────────────────────────
        private void btnAscii_Click(object sender, EventArgs e)
        {
            byte[] data;
            if (!TryParseHex(txtHex.Text, out data))
            {
                SetStatus("格式錯誤：請輸入以空白分隔的 16 進位數值", error: true);
                return;
            }

            // 顯示 ASCII 對照：可列印字元直接顯示，不可列印以 . 代替
            var sb = new StringBuilder();
            foreach (byte b in data)
                sb.Append(b >= 0x20 && b < 0x7F ? (char)b : '.');

            MessageBox.Show(
                $"HEX  : {txtHex.Text.Trim()}\nASCII: {sb}",
                "ASCII Code 對照",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ── 工具函式 ────────────────────────────────────────
        private bool TryParseHex(string input, out byte[] result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(input)) return false;
            try
            {
                result = input.Trim()
                              .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(s => Convert.ToByte(s, 16))
                              .ToArray();
                return result.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        private void SetStatus(string msg, bool error)
        {
            lblStatus.Text      = msg;
            lblStatus.ForeColor = error
                ? System.Drawing.Color.Red
                : System.Drawing.Color.DarkGreen;
        }
    }
}
