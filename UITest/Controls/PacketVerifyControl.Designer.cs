namespace UITest.Controls
{
    partial class PacketVerifyControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Version1BT = new System.Windows.Forms.Button();
            this.txtV1      = new System.Windows.Forms.TextBox();
            this.Version2BT = new System.Windows.Forms.Button();
            this.txtV2      = new System.Windows.Forms.TextBox();
            this.Version3BT = new System.Windows.Forms.Button();
            this.txtV3      = new System.Windows.Forms.TextBox();
            this.Version4BT = new System.Windows.Forms.Button();
            this.txtV4      = new System.Windows.Forms.TextBox();
            this.Version5BT = new System.Windows.Forms.Button();
            this.txtV5      = new System.Windows.Forms.TextBox();
            this.Version6BT = new System.Windows.Forms.Button();
            this.txtV6      = new System.Windows.Forms.TextBox();
            this.Version7BT = new System.Windows.Forms.Button();
            this.txtV7      = new System.Windows.Forms.TextBox();
            this.Version8BT = new System.Windows.Forms.Button();
            this.txtV8      = new System.Windows.Forms.TextBox();
            this.ClearBT    = new System.Windows.Forms.Button();
            this.txtResult  = new System.Windows.Forms.TextBox();
            this.lblResult  = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // ── 左欄 Version 1 ────────────────────────────────────────────
            this.Version1BT.Location = new System.Drawing.Point(10, 10);
            this.Version1BT.Size     = new System.Drawing.Size(140, 23);
            this.Version1BT.Text     = "一般訊息";
            this.Version1BT.Click   += new System.EventHandler(this.Version1BT_Click);

            this.txtV1.Location   = new System.Drawing.Point(10, 38);
            this.txtV1.Multiline  = true;
            this.txtV1.Size       = new System.Drawing.Size(330, 55);
            this.txtV1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtV1.Text       = "55 AA 02 11 12 34 19 00 01 15 00 77 7F 22 31 71 0E 00 03 64 07 0A 2A C6 59 11 A6 55 A6 EC 1F 1E 1D 97";

            // ── 左欄 Version 2 ────────────────────────────────────────────
            this.Version2BT.Location = new System.Drawing.Point(10, 105);
            this.Version2BT.Size     = new System.Drawing.Size(140, 23);
            this.Version2BT.Text     = "左側版型";
            this.Version2BT.Click   += new System.EventHandler(this.Version2BT_Click);

            this.txtV2.Location   = new System.Drawing.Point(10, 133);
            this.txtV2.Multiline  = true;
            this.txtV2.Size       = new System.Drawing.Size(330, 55);
            this.txtV2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtV2.Text       = "55 AA 01 01 34 1F 00 01 1C 00 7F 21 31 7A FF FF 00 01 72 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 30";

            // ── 左欄 Version 3 ────────────────────────────────────────────
            this.Version3BT.Location = new System.Drawing.Point(10, 200);
            this.Version3BT.Size     = new System.Drawing.Size(140, 23);
            this.Version3BT.Text     = "左側+右時間";
            this.Version3BT.Click   += new System.EventHandler(this.Version3BT_Click);

            this.txtV3.Location   = new System.Drawing.Point(10, 228);
            this.txtV3.Multiline  = true;
            this.txtV3.Size       = new System.Drawing.Size(330, 55);
            this.txtV3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtV3.Text       = "55 AA 01 01 34 25 00 01 22 00 7F 21 31 7A 00 00 FF 01 7B FF 00 00 00 00 73 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D B2";

            // ── 左欄 Version 4 ────────────────────────────────────────────
            this.Version4BT.Location = new System.Drawing.Point(10, 295);
            this.Version4BT.Size     = new System.Drawing.Size(140, 23);
            this.Version4BT.Text     = "右側時間";
            this.Version4BT.Click   += new System.EventHandler(this.Version4BT_Click);

            this.txtV4.Location   = new System.Drawing.Point(10, 323);
            this.txtV4.Multiline  = true;
            this.txtV4.Size       = new System.Drawing.Size(330, 55);
            this.txtV4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtV4.Text       = "55 AA 01 01 34 20 00 01 1D 00 7F 21 31 7B FF FF FF 0C 00 74 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 3E";

            // ── 右欄 Version 5 ────────────────────────────────────────────
            this.Version5BT.Location = new System.Drawing.Point(380, 10);
            this.Version5BT.Size     = new System.Drawing.Size(140, 23);
            this.Version5BT.Text     = "列車訊息";
            this.Version5BT.Click   += new System.EventHandler(this.Version5BT_Click);

            this.txtV5.Location   = new System.Drawing.Point(380, 38);
            this.txtV5.Multiline  = true;
            this.txtV5.Size       = new System.Drawing.Size(370, 55);
            this.txtV5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtV5.Text       = "55 AA 01 01 34 3B 00 02 38 00 77 7F 21 31 83 30 00 04 61 07 08 2A FF FF 00 B7 48 A6 77 1F 2D 01 00 01 FF FF 00 1F 2A FF FF 00 A5 5B D3 C2 1F 2D 02 00 01 FF FF 00 1F 2A FF FF 00 A5 BB AF B8 1F 1E 1D CA";

            // ── 右欄 Version 6 ────────────────────────────────────────────
            this.Version6BT.Location = new System.Drawing.Point(380, 105);
            this.Version6BT.Size     = new System.Drawing.Size(140, 23);
            this.Version6BT.Text     = "緊急訊息";
            this.Version6BT.Click   += new System.EventHandler(this.Version6BT_Click);

            this.txtV6.Location   = new System.Drawing.Point(380, 133);
            this.txtV6.Multiline  = true;
            this.txtV6.Size       = new System.Drawing.Size(370, 55);
            this.txtV6.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtV6.Text       = "55 AA 01 01 38 20 00 01 01 1C 00 77 79 02 80 FF 7F 21 32 71 10 00 01 64 07 0A 2A FF 00 00 B8 55 A4 6A BD 75 1F 1E 1D 28";

            // ── 右欄 Version 7 ────────────────────────────────────────────
            this.Version7BT.Location = new System.Drawing.Point(380, 200);
            this.Version7BT.Size     = new System.Drawing.Size(180, 23);
            this.Version7BT.Text     = "上排左側圖片";
            this.Version7BT.Click   += new System.EventHandler(this.Version7BT_Click);

            this.txtV7.Location   = new System.Drawing.Point(380, 228);
            this.txtV7.Multiline  = true;
            this.txtV7.Size       = new System.Drawing.Size(370, 55);
            this.txtV7.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtV7.Text       = "55 AA 01 01 34 27 00 01 24 00 7F 21 31 7D 31 00 00 FF 01 31 7B FF 00 00 0C 00 74 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 26";

            // ── 右欄 Version 8 ────────────────────────────────────────────
            this.Version8BT.Location = new System.Drawing.Point(380, 295);
            this.Version8BT.Size     = new System.Drawing.Size(180, 23);
            this.Version8BT.Text     = "下排左側標準時間";
            this.Version8BT.Click   += new System.EventHandler(this.Version8BT_Click);

            this.txtV8.Location   = new System.Drawing.Point(380, 323);
            this.txtV8.Multiline  = true;
            this.txtV8.Size       = new System.Drawing.Size(370, 55);
            this.txtV8.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtV8.Text       = "55 AA 01 01 34 26 00 02 23 00 7F 21 31 7E 31 00 FF 00 31 7B 00 00 FF 0C 00 74 10 00 04 64 07 0A 2A FF 00 00 B8 55 A4 6A BD 75 1F 1E 1D 28";

            // ── 結果區 ────────────────────────────────────────────────────
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(10, 395);
            this.lblResult.Text     = "驗證結果";

            this.ClearBT.Location = new System.Drawing.Point(80, 392);
            this.ClearBT.Size     = new System.Drawing.Size(60, 23);
            this.ClearBT.Text     = "清除";
            this.ClearBT.Click   += new System.EventHandler(this.ClearBT_Click);

            this.txtResult.Location   = new System.Drawing.Point(10, 420);
            this.txtResult.Multiline  = true;
            this.txtResult.Size       = new System.Drawing.Size(740, 80);
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.ReadOnly   = true;

            // ── UserControl ───────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(770, 520);
            this.Controls.Add(this.Version1BT); this.Controls.Add(this.txtV1);
            this.Controls.Add(this.Version2BT); this.Controls.Add(this.txtV2);
            this.Controls.Add(this.Version3BT); this.Controls.Add(this.txtV3);
            this.Controls.Add(this.Version4BT); this.Controls.Add(this.txtV4);
            this.Controls.Add(this.Version5BT); this.Controls.Add(this.txtV5);
            this.Controls.Add(this.Version6BT); this.Controls.Add(this.txtV6);
            this.Controls.Add(this.Version7BT); this.Controls.Add(this.txtV7);
            this.Controls.Add(this.Version8BT); this.Controls.Add(this.txtV8);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.ClearBT);
            this.Controls.Add(this.txtResult);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button  Version1BT, Version2BT, Version3BT, Version4BT;
        private System.Windows.Forms.Button  Version5BT, Version6BT, Version7BT, Version8BT;
        private System.Windows.Forms.Button  ClearBT;
        private System.Windows.Forms.TextBox txtV1, txtV2, txtV3, txtV4;
        private System.Windows.Forms.TextBox txtV5, txtV6, txtV7, txtV8;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label   lblResult;
    }
}
