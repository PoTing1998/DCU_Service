namespace UITest
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Version1BT = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.Version2BT = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.ClearBT = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.Version3BT = new System.Windows.Forms.Button();
            this.Version4BT = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.Version5BT = new System.Windows.Forms.Button();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.Version6BT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(32, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(293, 122);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(32, 217);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(293, 373);
            this.textBox2.TabIndex = 2;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(352, 154);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "Sequence";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(352, 109);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "FullWindow";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(352, 59);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "StringMessage";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(352, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "stringBody";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(352, 217);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 11;
            this.button5.Text = "packet";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(459, 62);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(276, 74);
            this.textBox5.TabIndex = 16;
            this.textBox5.Text = "55 AA 02 11 12 34 19 00 01 15 00 77 7F 22 31 71 0E 00 03 64 07 0A 2A C6 59 11 A6 " +
    "55 A6 EC 1F 1E 1D 97";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(457, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "拆解ByteArray";
            // 
            // Version1BT
            // 
            this.Version1BT.Location = new System.Drawing.Point(459, 33);
            this.Version1BT.Name = "Version1BT";
            this.Version1BT.Size = new System.Drawing.Size(119, 23);
            this.Version1BT.TabIndex = 18;
            this.Version1BT.Text = "一般訊息Unpacket";
            this.Version1BT.UseVisualStyleBackColor = true;
            this.Version1BT.Click += new System.EventHandler(this.Version1BT_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(820, 499);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(187, 46);
            this.textBoxResult.TabIndex = 19;
            // 
            // Version2BT
            // 
            this.Version2BT.Location = new System.Drawing.Point(459, 142);
            this.Version2BT.Name = "Version2BT";
            this.Version2BT.Size = new System.Drawing.Size(119, 23);
            this.Version2BT.TabIndex = 20;
            this.Version2BT.Text = "左側Unpacket";
            this.Version2BT.UseVisualStyleBackColor = true;
            this.Version2BT.Click += new System.EventHandler(this.Version2BT_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(459, 186);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(276, 74);
            this.textBox6.TabIndex = 21;
            this.textBox6.Text = " 55 AA 01 01 34 1F 00 01 1C 00 7F 21 31 7A FF FF 00 01 72 10 00 04 64 07 0A 2A FF" +
    " FF FF B8 55 A4 6A BD 75 1F 1E 1D 30";
            // 
            // ClearBT
            // 
            this.ClearBT.Location = new System.Drawing.Point(820, 441);
            this.ClearBT.Name = "ClearBT";
            this.ClearBT.Size = new System.Drawing.Size(119, 23);
            this.ClearBT.TabIndex = 22;
            this.ClearBT.Text = "清除";
            this.ClearBT.UseVisualStyleBackColor = true;
            this.ClearBT.Click += new System.EventHandler(this.ClearBT_Click);
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(459, 310);
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(276, 74);
            this.textBox7.TabIndex = 23;
            this.textBox7.Text = "55 AA 01 01 34 25 00 01 22 00 7F 21 31 7A 00 00 FF 01 7B FF 00 00 00 00 73 10 00 " +
    "04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D B2";
            // 
            // Version3BT
            // 
            this.Version3BT.Location = new System.Drawing.Point(459, 281);
            this.Version3BT.Name = "Version3BT";
            this.Version3BT.Size = new System.Drawing.Size(162, 23);
            this.Version3BT.TabIndex = 24;
            this.Version3BT.Text = "左側加上又時間Unpacket";
            this.Version3BT.UseVisualStyleBackColor = true;
            this.Version3BT.Click += new System.EventHandler(this.Version3BT_Click);
            // 
            // Version4BT
            // 
            this.Version4BT.Location = new System.Drawing.Point(459, 417);
            this.Version4BT.Name = "Version4BT";
            this.Version4BT.Size = new System.Drawing.Size(162, 23);
            this.Version4BT.TabIndex = 26;
            this.Version4BT.Text = "右側時間Unpacket";
            this.Version4BT.UseVisualStyleBackColor = true;
            this.Version4BT.Click += new System.EventHandler(this.Version4BT_Click);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(459, 467);
            this.textBox9.Multiline = true;
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(276, 74);
            this.textBox9.TabIndex = 27;
            this.textBox9.Text = "55 AA 01 01 34 20 00 01 1D 00 7F 21 31 7B FF FF FF 0C 00 74 10 00 04 64 07 0A 2A " +
    "FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 3E";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(760, 62);
            this.textBox10.Multiline = true;
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(276, 74);
            this.textBox10.TabIndex = 28;
            this.textBox10.Text = "55 AA 01 01 34 3B 00 02 38 00 77 7F 21 31 83 30 00 04 61 07 08 2A FF FF 00 B7 48 " +
    "A6 77 1F 2D 01 00 01 FF FF 00 1F 2A FF FF 00 A5 5B D3 C2 1F 2D 02 00 01 FF FF 00" +
    " 1F 2A FF FF 00 A5 BB AF B8 1F 1E 1D CA";
            // 
            // Version5BT
            // 
            this.Version5BT.Location = new System.Drawing.Point(760, 33);
            this.Version5BT.Name = "Version5BT";
            this.Version5BT.Size = new System.Drawing.Size(119, 23);
            this.Version5BT.TabIndex = 29;
            this.Version5BT.Text = "列車訊息Unpacket";
            this.Version5BT.UseVisualStyleBackColor = true;
            this.Version5BT.Click += new System.EventHandler(this.Version5BT_Click);
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(760, 245);
            this.textBox11.Multiline = true;
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(276, 74);
            this.textBox11.TabIndex = 30;
            this.textBox11.Text = "55 AA 01 01 38 20 00 01 01 1C 00 77 79 02 80 FF 7F 21 32 71 10 00 01 64 07 0A 2A " +
    "FF 00 00 B8 55 A4 6A BD 75 1F 1E 1D 28";
            // 
            // Version6BT
            // 
            this.Version6BT.Location = new System.Drawing.Point(760, 200);
            this.Version6BT.Name = "Version6BT";
            this.Version6BT.Size = new System.Drawing.Size(162, 23);
            this.Version6BT.TabIndex = 31;
            this.Version6BT.Text = "緊急訊息Unpacket";
            this.Version6BT.UseVisualStyleBackColor = true;
            this.Version6BT.Click += new System.EventHandler(this.Version6BT_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 642);
            this.Controls.Add(this.Version6BT);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.Version5BT);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.Version4BT);
            this.Controls.Add(this.Version3BT);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.ClearBT);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.Version2BT);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.Version1BT);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Version1BT;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button Version2BT;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button ClearBT;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Button Version3BT;
        private System.Windows.Forms.Button Version4BT;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Button Version5BT;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Button Version6BT;
    }
}

