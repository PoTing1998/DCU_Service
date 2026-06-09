using Display;
using Display.DisplayMode;
using Display.Function;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using UITest.Services;

using static Display.DisplaySettingsEnums;

namespace UITest.Controls
{
    public partial class PacketBuilderControl : UserControl
    {
        private PacketValidationService _validationService;
        private PacketBuilderService    _builderService;

        // 各步驟的中間結果
        private TextStringBody _textStringBody;
        private StringMessage  _stringMessage;
        private FullWindow     _fullWindow;
        private Sequence       _sequence;

        public PacketBuilderControl()
        {
            InitializeComponent();

            // 設計階段不初始化服務，避免 Designer 載入時 NullReferenceException
            if (!DesignMode)
            {
                _validationService = new PacketValidationService();
                _builderService    = new PacketBuilderService();
            }
        }

        // ── 讀取 UI 設定 ─────────────────────────────────────────────────

        private ScrollInfo GetScrollInfo() => new ScrollInfo
        {
            ScrollMode  = (byte)(ScrollMode)Enum.Parse(typeof(ScrollMode), cmbScrollMode.SelectedItem.ToString()),
            ScrollSpeed = (byte)nudScrollSpeed.Value,
            PauseTime   = (byte)nudPauseTime.Value
        };

        private FontSetting GetFontSetting() => new FontSetting
        {
            Size  = (FontSize) Enum.Parse(typeof(FontSize),         cmbFontSize.SelectedItem.ToString()),
            Style = (Display.FontStyle)Enum.Parse(typeof(Display.FontStyle), cmbFontStyle.SelectedItem.ToString())
        };

        private List<byte> GetIDs()
        {
            return txtIDs.Text.Trim()
                .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Convert.ToByte(s.Trim(), 16))
                .ToList();
        }

        private byte GetFunctionCode()
        {
            // "0x34 旅客訊息" → 0x34
            var prefix = cmbFuncCode.SelectedItem.ToString().Substring(0, 4);
            return Convert.ToByte(prefix, 16);
        }

        // ── Step 1 ────────────────────────────────────────────────────────

        private void btnStep1_Click(object sender, EventArgs e)
        {
            var result = _builderService.BuildTextStringBody(
                txtColor.Text.Trim(),
                txtContent.Text.Trim());

            if (result.IsValid)
            {
                _textStringBody = result.Value;
                AppendLine("✔ TextStringBody 建立成功");
                AppendLine(result.HexDump);
                AppendLine("──────────────────────────");
            }
            else
            {
                AppendLine($"✘ TextStringBody 失敗：{result.ErrorMessage}");
            }
        }

        // ── Step 2 ────────────────────────────────────────────────────────

        private void btnStep2_Click(object sender, EventArgs e)
        {
            if (_textStringBody == null) { AppendLine("✘ 尚未建立 TextStringBody（請先執行 Step 1）"); return; }

            var result = _builderService.BuildStringMessage(_textStringBody);
            if (result.IsValid)
            {
                _stringMessage = result.Value;
                AppendLine("✔ StringMessage 建立成功");
                AppendLine(result.HexDump);
                AppendLine("──────────────────────────");
            }
            else
            {
                AppendLine($"✘ StringMessage 失敗：{result.ErrorMessage}");
            }
        }

        // ── Step 3 ────────────────────────────────────────────────────────

        private void btnStep3_Click(object sender, EventArgs e)
        {
            if (_stringMessage == null) { AppendLine("✘ 尚未建立 StringMessage（請先執行 Step 2）"); return; }

            var result = _builderService.BuildFullWindow(_stringMessage, (byte)nudLevel.Value, GetScrollInfo());
            if (result.IsValid)
            {
                _fullWindow = result.Value;
                AppendLine($"✔ FullWindow 建立成功  MessageType=0x{_fullWindow.MessageType:X2}  Level={_fullWindow.MessageLevel}");
                AppendLine(result.HexDump);
                AppendLine("──────────────────────────");
            }
            else
            {
                AppendLine($"✘ FullWindow 失敗：{result.ErrorMessage}");
            }
        }

        // ── Step 4 ────────────────────────────────────────────────────────

        private void btnStep4_Click(object sender, EventArgs e)
        {
            if (_fullWindow == null) { AppendLine("✘ 尚未建立 FullWindow（請先執行 Step 3）"); return; }

            var result = _builderService.BuildSequence(_fullWindow, GetFontSetting());
            if (result.IsValid)
            {
                _sequence = result.Value;
                AppendLine("✔ Sequence 建立成功");
                AppendLine(result.HexDump);
                AppendLine("──────────────────────────");
            }
            else
            {
                AppendLine($"✘ Sequence 失敗：{result.ErrorMessage}");
            }
        }

        // ── Step 5 ────────────────────────────────────────────────────────

        private void btnStep5_Click(object sender, EventArgs e)
        {
            if (_sequence == null) { AppendLine("✘ 尚未建立 Sequence（請先執行 Step 4）"); return; }

            try
            {
                var result = _builderService.BuildPacket(_sequence, GetIDs(), GetFunctionCode());
                if (result.IsValid)
                {
                    AppendLine("✔ 封包建立成功");
                    AppendLine(result.HexDump);
                    AppendLine("──────────────────────────");
                }
                else
                {
                    AppendLine($"✘ 封包失敗：{result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                AppendLine($"✘ IDs 格式錯誤：{ex.Message}");
            }
        }

        // ── 一鍵組成 ─────────────────────────────────────────────────────

        private void btnBuildAll_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            _textStringBody = null;
            _stringMessage  = null;
            _fullWindow     = null;
            _sequence       = null;

            btnStep1_Click(sender, e);
            if (_textStringBody == null) return;

            btnStep2_Click(sender, e);
            if (_stringMessage == null) return;

            btnStep3_Click(sender, e);
            if (_fullWindow == null) return;

            btnStep4_Click(sender, e);
            if (_sequence == null) return;

            btnStep5_Click(sender, e);
        }

        // ── 清除 ─────────────────────────────────────────────────────────

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            _textStringBody = null;
            _stringMessage  = null;
            _fullWindow     = null;
            _sequence       = null;
        }

        // ── 複製輸出 ─────────────────────────────────────────────────────

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOutput.Text))
            {
                Clipboard.SetText(txtOutput.Text);
                AppendLine("（已複製到剪貼板）");
            }
        }

        // ── 工具 ─────────────────────────────────────────────────────────

        private void AppendLine(string text) => txtOutput.AppendText(text + "\r\n");
    }
}
