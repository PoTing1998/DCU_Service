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
            this.lblVerifier   = new System.Windows.Forms.Label();
            this.cmbVerifier   = new System.Windows.Forms.ComboBox();
            this.btnSample     = new System.Windows.Forms.Button();
            this.lblHex        = new System.Windows.Forms.Label();
            this.txtHex        = new System.Windows.Forms.TextBox();
            this.btnVerify     = new System.Windows.Forms.Button();
            this.btnClear      = new System.Windows.Forms.Button();
            this.pnlResult     = new System.Windows.Forms.Panel();
            this.lblResultIcon = new System.Windows.Forms.Label();
            this.lblResultMsg  = new System.Windows.Forms.Label();
            this.pnlResult.SuspendLayout();
            this.SuspendLayout();

            // ── 版型選擇 ─────────────────────────────────────
            this.lblVerifier.AutoSize = true;
            this.lblVerifier.Location = new System.Drawing.Point(10, 14);
            this.lblVerifier.Text     = "版型";

            this.cmbVerifier.Location      = new System.Drawing.Point(50, 10);
            this.cmbVerifier.Size          = new System.Drawing.Size(200, 21);
            this.cmbVerifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVerifier.Items.AddRange(new object[]
            {
                "一般訊息（FullWindow）",
                "左側版型（LeftPlatform）",
                "左側＋右時間（LeftPlatformRightTime）",
                "右側時間（RightTime）",
                "列車訊息（TrainDynamic）",
                "緊急訊息（Urgent）",
                "上排左側圖片（IdentifierStatusImage）",
                "下排左側標準時間（StandardTimeBottomLeft）"
            });
            this.cmbVerifier.SelectedIndex        = 0;
            this.cmbVerifier.SelectedIndexChanged += new System.EventHandler(this.cmbVerifier_SelectedIndexChanged);

            this.btnSample.Location = new System.Drawing.Point(260, 8);
            this.btnSample.Size     = new System.Drawing.Size(80, 25);
            this.btnSample.Text     = "載入範例";
            this.btnSample.Click   += new System.EventHandler(this.btnSample_Click);

            // ── Hex 輸入 ─────────────────────────────────────
            this.lblHex.AutoSize = true;
            this.lblHex.Location = new System.Drawing.Point(10, 46);
            this.lblHex.Text     = "Hex 封包（空白分隔）";

            this.txtHex.Location   = new System.Drawing.Point(10, 64);
            this.txtHex.Multiline  = true;
            this.txtHex.Size       = new System.Drawing.Size(740, 80);
            this.txtHex.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHex.Font       = new System.Drawing.Font("Courier New", 9F);

            // ── 操作按鈕 ─────────────────────────────────────
            this.btnVerify.Location  = new System.Drawing.Point(10, 154);
            this.btnVerify.Size      = new System.Drawing.Size(90, 28);
            this.btnVerify.Text      = "▶ 驗證";
            this.btnVerify.BackColor = System.Drawing.Color.FromArgb(220, 235, 255);
            this.btnVerify.Click    += new System.EventHandler(this.btnVerify_Click);

            this.btnClear.Location = new System.Drawing.Point(108, 154);
            this.btnClear.Size     = new System.Drawing.Size(70, 28);
            this.btnClear.Text     = "清除";
            this.btnClear.Click   += new System.EventHandler(this.btnClear_Click);

            // ── 結果面板 ─────────────────────────────────────
            this.pnlResult.Location    = new System.Drawing.Point(10, 192);
            this.pnlResult.Size        = new System.Drawing.Size(740, 120);
            this.pnlResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlResult.BackColor   = System.Drawing.SystemColors.Control;

            this.lblResultIcon.AutoSize  = true;
            this.lblResultIcon.Location  = new System.Drawing.Point(12, 12);
            this.lblResultIcon.Font      = new System.Drawing.Font("Microsoft JhengHei", 24F, System.Drawing.FontStyle.Bold);
            this.lblResultIcon.Text      = "";

            this.lblResultMsg.Location  = new System.Drawing.Point(12, 55);
            this.lblResultMsg.Size      = new System.Drawing.Size(710, 55);
            this.lblResultMsg.Font      = new System.Drawing.Font("Microsoft JhengHei", 9F);
            this.lblResultMsg.Text      = "";

            this.pnlResult.Controls.Add(this.lblResultIcon);
            this.pnlResult.Controls.Add(this.lblResultMsg);

            // ── UserControl ───────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(770, 330);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblVerifier, this.cmbVerifier, this.btnSample,
                this.lblHex, this.txtHex,
                this.btnVerify, this.btnClear,
                this.pnlResult
            });

            this.pnlResult.ResumeLayout(false);
            this.pnlResult.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label     lblVerifier;
        private System.Windows.Forms.ComboBox  cmbVerifier;
        private System.Windows.Forms.Button    btnSample;
        private System.Windows.Forms.Label     lblHex;
        private System.Windows.Forms.TextBox   txtHex;
        private System.Windows.Forms.Button    btnVerify;
        private System.Windows.Forms.Button    btnClear;
        private System.Windows.Forms.Panel     pnlResult;
        private System.Windows.Forms.Label     lblResultIcon;
        private System.Windows.Forms.Label     lblResultMsg;
    }
}
