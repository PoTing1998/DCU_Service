namespace UITest
{
    partial class CommLogForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlToolbar  = new System.Windows.Forms.Panel();
            this.btnSave     = new System.Windows.Forms.Button();
            this.btnClear    = new System.Windows.Forms.Button();
            this.chkHexOnly  = new System.Windows.Forms.CheckBox();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.rtbLog      = new System.Windows.Forms.RichTextBox();

            this.pnlToolbar.SuspendLayout();
            this.SuspendLayout();

            // ── 工具列 ────────────────────────────────────────────────────
            this.pnlToolbar.Dock      = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Height    = 36;
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.pnlToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.btnSave.Location = new System.Drawing.Point(6, 6);
            this.btnSave.Size     = new System.Drawing.Size(60, 24);
            this.btnSave.Text     = "存檔";
            this.btnSave.Click   += new System.EventHandler(this.btnSave_Click);

            this.btnClear.Location = new System.Drawing.Point(72, 6);
            this.btnClear.Size     = new System.Drawing.Size(60, 24);
            this.btnClear.Text     = "清除";
            this.btnClear.Click   += new System.EventHandler(this.btnClear_Click);

            this.chkHexOnly.AutoSize = true;
            this.chkHexOnly.Location = new System.Drawing.Point(150, 10);
            this.chkHexOnly.Text     = "只顯示 HEX";
            this.chkHexOnly.CheckedChanged += new System.EventHandler(this.chkHexOnly_CheckedChanged);

            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Location = new System.Drawing.Point(240, 10);
            this.chkAutoScroll.Text     = "自動捲動";
            this.chkAutoScroll.Checked  = true;

            this.pnlToolbar.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.btnSave, this.btnClear, this.chkHexOnly, this.chkAutoScroll
            });

            // ── Log 顯示區（深色終端機風格）─────────────────────────────
            this.rtbLog.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.BackColor   = System.Drawing.Color.FromArgb(15, 15, 15);
            this.rtbLog.ForeColor   = System.Drawing.Color.White;
            this.rtbLog.Font        = new System.Drawing.Font("Consolas", 9.5f);
            this.rtbLog.ReadOnly    = true;
            this.rtbLog.ScrollBars  = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this.rtbLog.WordWrap    = false;

            // ── Form ──────────────────────────────────────────────────────
            this.AutoScaleMode   = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize      = new System.Drawing.Size(760, 480);
            this.MinimumSize     = new System.Drawing.Size(400, 200);
            this.Text            = "通訊狀態顯示";
            this.ShowInTaskbar   = false;
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.rtbLog, this.pnlToolbar   // 注意順序：rtbLog 先加（Fill），toolbar 後加（Top）
            });
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommLogForm_FormClosing);

            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel       pnlToolbar;
        private System.Windows.Forms.Button      btnSave;
        private System.Windows.Forms.Button      btnClear;
        private System.Windows.Forms.CheckBox    chkHexOnly;
        private System.Windows.Forms.CheckBox    chkAutoScroll;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}
