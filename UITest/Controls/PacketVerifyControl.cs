using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using UITest.Verify;

namespace UITest.Controls
{
    public partial class PacketVerifyControl : UserControl
    {
        // ── 版型對應表：(驗證器, 範例封包) ──────────────────────────────
        private readonly List<(PacketVerifyBase Verifier, string Sample)> _verifiers
            = new List<(PacketVerifyBase, string)>
        {
            (new FullWindowHandlerVerify(),
             "55 AA 02 11 12 34 19 00 01 15 00 77 7F 22 31 71 0E 00 03 64 07 0A 2A C6 59 11 A6 55 A6 EC 1F 1E 1D 97"),

            (new LeftPlatformHandlerVerify(),
             "55 AA 01 01 34 1F 00 01 1C 00 7F 21 31 7A FF FF 00 01 72 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 30"),

            (new LeftPlatformRightTimeHandlerVerify(),
             "55 AA 01 01 34 25 00 01 22 00 7F 21 31 7A 00 00 FF 01 7B FF 00 00 00 00 73 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D B2"),

            (new RightTimeHandlerVerify(),
             "55 AA 01 01 34 20 00 01 1D 00 7F 21 31 7B FF FF FF 0C 00 74 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 3E"),

            (new TrainDynamicVerify(),
             "55 AA 01 01 34 3B 00 02 38 00 77 7F 21 31 83 30 00 04 61 07 08 2A FF FF 00 B7 48 A6 77 1F 2D 01 00 01 FF FF 00 1F 2A FF FF 00 A5 5B D3 C2 1F 2D 02 00 01 FF FF 00 1F 2A FF FF 00 A5 BB AF B8 1F 1E 1D CA"),

            (new UrgentHandlerVerify(),
             "55 AA 01 01 38 20 00 01 01 1C 00 77 79 02 80 FF 7F 21 32 71 10 00 01 64 07 0A 2A FF 00 00 B8 55 A4 6A BD 75 1F 1E 1D 28"),

            (new IdentifierStatusImageTopLeft24x48HandlerVerify(),
             "55 AA 01 01 34 27 00 01 24 00 7F 21 31 7D 31 00 00 FF 01 31 7B FF 00 00 0C 00 74 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 26"),

            (new StandardTimeBottomLeftHandlerVerify(),
             "55 AA 01 01 34 26 00 02 23 00 7F 21 31 7E 31 00 FF 00 31 7B 00 00 FF 0C 00 74 10 00 04 64 07 0A 2A FF 00 00 B8 55 A4 6A BD 75 1F 1E 1D 28"),
        };

        public PacketVerifyControl()
        {
            InitializeComponent();
            LoadSample(); // 預設載入第一個版型的範例
        }

        // ── 版型切換：自動載入範例 ───────────────────────────────────────

        private void cmbVerifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSample();
            ClearResult();
        }

        private void LoadSample()
        {
            int idx = cmbVerifier.SelectedIndex;
            if (idx >= 0 && idx < _verifiers.Count)
                txtHex.Text = _verifiers[idx].Sample;
        }

        // ── 按鈕事件 ─────────────────────────────────────────────────────

        private void btnSample_Click(object sender, EventArgs e) => LoadSample();

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtHex.Clear();
            ClearResult();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            int idx = cmbVerifier.SelectedIndex;
            if (idx < 0 || idx >= _verifiers.Count) return;

            byte[] data = ASI.Lib.Msg.Parsing.ByteArray.HexStringToBytes(txtHex.Text.Trim());
            if (data == null || data.Length == 0)
            {
                ShowResult(false, "Hex 格式錯誤，請輸入以空白分隔的十六進位數值。");
                return;
            }

            bool ok = _verifiers[idx].Verifier.ValidatePacket(data, out string errorMessage);
            ShowResult(ok, ok ? $"共 {data.Length} bytes，驗證通過。" : errorMessage);
        }

        // ── 結果顯示 ─────────────────────────────────────────────────────

        private void ShowResult(bool ok, string message)
        {
            if (ok)
            {
                pnlResult.BackColor   = Color.FromArgb(230, 255, 230); // 淡綠
                lblResultIcon.Text    = "✔";
                lblResultIcon.ForeColor = Color.DarkGreen;
            }
            else
            {
                pnlResult.BackColor   = Color.FromArgb(255, 225, 225); // 淡紅
                lblResultIcon.Text    = "✘";
                lblResultIcon.ForeColor = Color.DarkRed;
            }
            lblResultMsg.Text = message;
        }

        private void ClearResult()
        {
            pnlResult.BackColor   = System.Drawing.SystemColors.Control;
            lblResultIcon.Text    = "";
            lblResultMsg.Text     = "";
        }
    }
}
