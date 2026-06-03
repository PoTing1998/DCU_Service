using ASI.Wanda.DCU.TaskCDU;

using Display;
using Display.DisplayMode;

using System;
using System.Windows.Forms;

using UITest.Services;

using static Display.DisplaySettingsEnums;

namespace UITest.Controls
{
    public partial class PacketBuilderControl : UserControl
    {
        private readonly PacketValidationService _validationService = new PacketValidationService();
        private readonly PacketBuilderService    _builderService    = new PacketBuilderService();

        private TextStringBody _textStringBody;
        private StringMessage  _stringMessage;
        private FullWindow     _fullWindow;
        private Sequence       _sequence;

        public PacketBuilderControl()
        {
            InitializeComponent();
        }

        // Step 1 - 建立 TextStringBody
        private void button1_Click(object sender, EventArgs e)
        {
            var fontColor      = PacketBuilderService.ExtractValue(txtInput.Text, "字體顏色 : ");
            var messageContent = PacketBuilderService.ExtractValue(txtInput.Text, "字體內容 : ");

            var result = _builderService.BuildTextStringBody(
                fontColor, messageContent,
                color => ASI.Wanda.DCU.DB.Tables.System.sysConfig.PickColor(color),
                hex   => DataConversion.FromHex(hex));

            if (result.IsValid)
            {
                _textStringBody = result.Value;
                AppendLine("textStringBody 符合規則。");
                AppendLine(result.HexDump);
                AppendLine("========================");
            }
            else
            {
                AppendLine($"textStringBody 不符合規則：{result.ErrorMessage}");
            }
        }

        // Step 2 - 建立 StringMessage
        private void button2_Click(object sender, EventArgs e)
        {
            if (_textStringBody == null) { AppendLine("尚未初始化 textStringBody。"); return; }

            AppendLine($"RedColor: {_textStringBody.RedColor}");
            AppendLine($"GreenColor: {_textStringBody.GreenColor}");
            AppendLine($"BlueColor: {_textStringBody.BlueColor}");

            var textVr = _validationService.ValidateStringText(_textStringBody.StringText);
            AppendLine(textVr.IsValid ? "StringText 符合 BIG-5 編碼規則。" : "StringText 不符合 BIG-5 編碼規則。");

            var result = _builderService.BuildStringMessage(_textStringBody);
            if (result.IsValid)
            {
                _stringMessage = result.Value;
                AppendLine("StringMessage 符合規則。");
                AppendLine(result.HexDump);
                AppendLine("========================");
            }
            else
            {
                AppendLine($"StringMessage 不符合規則：{result.ErrorMessage}");
            }
        }

        // Step 3 - 建立 FullWindow
        private void button3_Click(object sender, EventArgs e)
        {
            if (_stringMessage == null) { MessageBox.Show("尚未初始化 StringMessage"); return; }

            var result = _builderService.BuildFullWindow(_stringMessage);
            if (result.IsValid)
            {
                _fullWindow = result.Value;
                AppendLine($"MessageType: {_fullWindow.MessageType}");
                AppendLine(result.HexDump);
                AppendLine("========================");
            }
            else
            {
                MessageBox.Show("組成封包有誤：" + result.ErrorMessage);
            }
        }

        // Step 4 - 建立 Sequence
        private void button4_Click(object sender, EventArgs e)
        {
            if (_fullWindow == null) { MessageBox.Show("尚未初始化 FullWindow"); return; }

            var result = _builderService.BuildSequence(_fullWindow);
            if (result.IsValid)
            {
                _sequence = result.Value;
                AppendLine(result.HexDump);
                AppendLine("========================");
            }
            else
            {
                AppendLine($"Sequence 錯誤：{result.ErrorMessage}");
            }
        }

        // Step 5 - 建立完整封包
        private void button5_Click(object sender, EventArgs e)
        {
            if (_sequence == null) { MessageBox.Show("尚未初始化 Sequence"); return; }

            var result = _builderService.BuildPacket(_sequence);
            if (result.IsValid)
            {
                AppendLine(result.HexDump);
                AppendLine("========================");
            }
            else
            {
                AppendLine($"封包錯誤：{result.ErrorMessage}");
            }
        }

        private void AppendLine(string text) => txtOutput.Text += text + "\r\n";
    }
}
