using Display;
using Display.DisplayMode;
using Display.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Display.DisplaySettingsEnums;

namespace UITest
{
    /// <summary>
    /// 封包組成測試相關功能 (優化版)
    /// </summary>
    public partial class Form1
    {
        #region Packet Builder - Constants

        /// <summary>
        /// 預設範例訊息
        /// </summary>
        private const string DEFAULT_SAMPLE_MESSAGE = @"字體顏色 : 紅色
字體內容 : 測試訊息Test Message";

        #endregion

        #region Packet Builder - Initialization

        /// <summary>
        /// 初始化封包組成器
        /// </summary>
        private void InitializePacketBuilder()
        {
            // 載入預設範例
            LoadDefaultSampleMessage();
        }

        /// <summary>
        /// 載入預設範例訊息
        /// </summary>
        private void LoadDefaultSampleMessage()
        {
            if (string.IsNullOrWhiteSpace(txtMessageInput.Text))
            {
                txtMessageInput.Text = DEFAULT_SAMPLE_MESSAGE;
            }
        }

        #endregion

        #region Packet Builder - One-Click Generation

        /// <summary>
        /// 一鍵生成完整封包（新增功能）
        /// </summary>
        private void GenerateFullPacketOneClick()
        {
            try
            {
                txtPacketOutput.Clear();
                txtPacketOutput.Text = "==================== 開始一鍵生成封包 ====================\r\n\r\n";

                // Step 1: 建立 StringBody
                if (!CreateStringBodyInternal(out var textStringBody))
                {
                    txtPacketOutput.Text += "\r\n✗ 封包生成失敗: StringBody 建立錯誤\r\n";
                    return;
                }
                txtPacketOutput.Text += "✓ Step 1: StringBody 建立成功\r\n\r\n";

                // Step 2: 建立 StringMessage
                if (!CreateStringMessageInternal(textStringBody, out var stringMessage))
                {
                    txtPacketOutput.Text += "\r\n✗ 封包生成失敗: StringMessage 建立錯誤\r\n";
                    return;
                }
                txtPacketOutput.Text += "✓ Step 2: StringMessage 建立成功\r\n\r\n";

                // Step 3: 建立 FullWindow
                if (!CreateFullWindowInternal(stringMessage, out var fullWindow))
                {
                    txtPacketOutput.Text += "\r\n✗ 封包生成失敗: FullWindow 建立錯誤\r\n";
                    return;
                }
                txtPacketOutput.Text += "✓ Step 3: FullWindow 建立成功\r\n\r\n";

                // Step 4: 建立 Sequence
                if (!CreateSequenceInternal(fullWindow, out var sequence))
                {
                    txtPacketOutput.Text += "\r\n✗ 封包生成失敗: Sequence 建立錯誤\r\n";
                    return;
                }
                txtPacketOutput.Text += "✓ Step 4: Sequence 建立成功\r\n\r\n";

                // Step 5: 建立 Packet
                if (!CreatePacketInternal(sequence, out var packetBytes))
                {
                    txtPacketOutput.Text += "\r\n✗ 封包生成失敗: Packet 建立錯誤\r\n";
                    return;
                }
                txtPacketOutput.Text += "✓ Step 5: Packet 建立成功\r\n\r\n";

                // 顯示最終結果
                txtPacketOutput.Text += "==================== 完整封包 ====================\r\n\r\n";
                txtPacketOutput.Text += FormatPacketOutput(packetBytes);
                txtPacketOutput.Text += "\r\n\r\n==================== 封包生成完成 ====================\r\n";
                txtPacketOutput.Text += $"總長度: {packetBytes.Length} bytes\r\n";
                txtPacketOutput.Text += $"生成時間: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n";

                // 保存到類成員變數供分步使用
                this.textStringBody = textStringBody;
                this.stringMessage = stringMessage;
                this.fullWindow = fullWindow;
                this.sequence = sequence;
            }
            catch (Exception ex)
            {
                txtPacketOutput.ForeColor = System.Drawing.Color.Red;
                txtPacketOutput.Text += $"\r\n\r\n錯誤: {ex.Message}\r\n";
                txtPacketOutput.ForeColor = System.Drawing.Color.Black;
            }
        }

        #endregion

        #region Packet Builder - Internal Creation Methods

        /// <summary>
        /// 內部方法：建立 StringBody
        /// </summary>
        private bool CreateStringBodyInternal(out TextStringBody result)
        {
            result = null;
            try
            {
                string textBoxContent = txtMessageInput.Text;
                string fontColor = ExtractValue(textBoxContent, "字體顏色");
                string messageContent = ExtractValue(textBoxContent, "字體內容");

                if (string.IsNullOrWhiteSpace(messageContent))
                {
                    txtPacketOutput.Text += "錯誤: 未找到字體內容\r\n";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(fontColor))
                {
                    fontColor = "白色"; // 預設白色
                    txtPacketOutput.Text += "提示: 未指定字體顏色，使用預設值「白色」\r\n";
                }

                var fontColorBytes = ColorHelper.GetColorBytes(fontColor);
                var fontColorHex = ColorHelper.GetColorHex(fontColor);

                result = new TextStringBody
                {
                    RedColor = fontColorBytes[0],
                    GreenColor = fontColorBytes[1],
                    BlueColor = fontColorBytes[2],
                    StringText = messageContent
                };

                txtPacketOutput.Text += $"  字體顏色: {fontColor} ({fontColorHex})\r\n";
                txtPacketOutput.Text += $"  字體內容: {messageContent}\r\n";
                txtPacketOutput.Text += $"  RGB: R={result.RedColor:X2} G={result.GreenColor:X2} B={result.BlueColor:X2}\r\n";

                return ValidateStringBody(result);
            }
            catch (Exception ex)
            {
                txtPacketOutput.Text += $"StringBody 建立錯誤: {ex.Message}\r\n";
                return false;
            }
        }

        /// <summary>
        /// 內部方法：建立 StringMessage
        /// </summary>
        private bool CreateStringMessageInternal(TextStringBody textStringBody, out StringMessage result)
        {
            result = null;
            try
            {
                result = new StringMessage
                {
                    StringMode = 0x2A, // Text Static
                    StringBody = textStringBody
                };

                txtPacketOutput.Text += $"  StringMode: 0x{result.StringMode:X2} (Text Static)\r\n";
                return ValidateStringMessage(result);
            }
            catch (Exception ex)
            {
                txtPacketOutput.Text += $"StringMessage 建立錯誤: {ex.Message}\r\n";
                return false;
            }
        }

        /// <summary>
        /// 內部方法：建立 FullWindow
        /// </summary>
        private bool CreateFullWindowInternal(StringMessage stringMessage, out FullWindow result)
        {
            result = null;
            try
            {
                result = new FullWindow
                {
                    MessageType = 0x71,
                    MessageLevel = 3,
                    MessageScroll = new ScrollInfo
                    {
                        ScrollMode = 0x64, // Left scroll
                        ScrollSpeed = 7,
                        PauseTime = 10
                    },
                    MessageContent = new List<StringMessage> { stringMessage }
                };

                txtPacketOutput.Text += $"  MessageType: 0x{result.MessageType:X2} (FullWindow)\r\n";
                txtPacketOutput.Text += $"  MessageLevel: {result.MessageLevel}\r\n";
                txtPacketOutput.Text += $"  ScrollMode: 0x{result.MessageScroll.ScrollMode:X2}\r\n";
                txtPacketOutput.Text += $"  ScrollSpeed: {result.MessageScroll.ScrollSpeed}\r\n";
                txtPacketOutput.Text += $"  PauseTime: {result.MessageScroll.PauseTime}\r\n";

                return true;
            }
            catch (Exception ex)
            {
                txtPacketOutput.Text += $"FullWindow 建立錯誤: {ex.Message}\r\n";
                return false;
            }
        }

        /// <summary>
        /// 內部方法：建立 Sequence
        /// </summary>
        private bool CreateSequenceInternal(FullWindow fullWindow, out Display.Sequence result)
        {
            result = null;
            try
            {
                result = new Display.Sequence
                {
                    SequenceNo = 1,
                    Font = new FontSetting
                    {
                        Size = FontSize.Font24x24,
                        Style = Display.FontStyle.Ming
                    },
                    Messages = new List<IMessage> { fullWindow }
                };

                txtPacketOutput.Text += $"  SequenceNo: {result.SequenceNo}\r\n";
                txtPacketOutput.Text += $"  FontSize: {result.Font.Size} (24x24)\r\n";
                txtPacketOutput.Text += $"  FontStyle: {result.Font.Style} (Ming)\r\n";

                return ValidateMessageFont(result);
            }
            catch (Exception ex)
            {
                txtPacketOutput.Text += $"Sequence 建立錯誤: {ex.Message}\r\n";
                return false;
            }
        }

        /// <summary>
        /// 內部方法：建立 Packet
        /// </summary>
        private bool CreatePacketInternal(Display.Sequence sequence, out byte[] result)
        {
            result = null;
            try
            {
                var processor = new PacketProcessor();
                var startCode = new byte[] { 0x55, 0xAA };
                var deviceIDs = new List<byte> { 0x11, 0x12 };
                var function = new PassengerInfoHandler();

                var packet = processor.CreatePacket(
                    startCode,
                    deviceIDs,
                    function.FunctionCode,
                    new List<Display.Sequence> { sequence }
                );

                result = packet.ToBytes();

                txtPacketOutput.Text += $"  StartCode: 0x{startCode[0]:X2} 0x{startCode[1]:X2}\r\n";
                txtPacketOutput.Text += $"  DeviceIDs: 0x{deviceIDs[0]:X2} 0x{deviceIDs[1]:X2}\r\n";
                txtPacketOutput.Text += $"  FunctionCode: 0x{function.FunctionCode:X2} (PassengerInfo)\r\n";

                return true;
            }
            catch (Exception ex)
            {
                txtPacketOutput.Text += $"Packet 建立錯誤: {ex.Message}\r\n";
                return false;
            }
        }

        #endregion

        #region Packet Builder - Button Click Events

        /// <summary>
        /// 一鍵生成完整封包按鈕
        /// </summary>
        private void btnGeneratePacket_Click(object sender, EventArgs e)
        {
            GenerateFullPacketOneClick();
        }

        /// <summary>
        /// 清除封包輸出和中間變數
        /// </summary>
        private void btnClearPacketOutput_Click(object sender, EventArgs e)
        {
            txtPacketOutput.Clear();
            txtMessageInput.Text = DEFAULT_SAMPLE_MESSAGE;
            textStringBody = null;
            stringMessage = null;
            fullWindow = null;
            sequence = null;
            MessageBox.Show("已清除所有資料並重置為預設範例", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Packet Builder - Helper Methods

        /// <summary>
        /// 從文本中提取指定標籤後的值（改進版，支持更靈活的格式）
        /// </summary>
        private string ExtractValue(string source, string label)
        {
            // 支持的格式：
            // 1. "標籤 : 值"
            // 2. "標籤: 值"
            // 3. "標籤：值"
            // 4. "標籤 值"

            string[] separators = new[] { " : ", ": ", "：", " " };

            foreach (var separator in separators)
            {
                string searchPattern = label + separator;
                int startIndex = source.IndexOf(searchPattern, StringComparison.OrdinalIgnoreCase);

                if (startIndex != -1)
                {
                    startIndex += searchPattern.Length;
                    int endIndex = source.IndexOf("\r\n", startIndex);
                    if (endIndex == -1) endIndex = source.IndexOf("\n", startIndex);
                    if (endIndex == -1) endIndex = source.Length;

                    string value = source.Substring(startIndex, endIndex - startIndex).Trim();
                    return value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 格式化十六進制輸出
        /// </summary>
        private string FormatHexOutput(byte[] data)
        {
            if (data == null || data.Length == 0) return "";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0 && i % 16 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append($"{data[i]:X2} ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化完整封包輸出（包含HEX和DEC）
        /// </summary>
        private string FormatPacketOutput(byte[] data)
        {
            StringBuilder sb = new StringBuilder();

            // HEX格式
            sb.AppendLine("HEX 格式:");
            sb.AppendLine(FormatHexOutput(data));

            // DEC格式
            sb.AppendLine("\r\nDEC 格式:");
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0 && i % 16 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append($"{data[i],3} ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 驗證字串是否符合 ASCII 或中文 BIG-5 編碼
        /// </summary>
        private bool ValidateStringText(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            foreach (char c in text)
            {
                // ASCII 範圍
                if (c >= 0x20 && c <= 0x7F)
                {
                    continue;
                }

                // BIG5 中文
                byte[] bytes = Encoding.GetEncoding(950).GetBytes(c.ToString());
                if (bytes.Length == 2)
                {
                    continue;
                }

                txtPacketOutput.Text += $"  警告: 字元 '{c}' 不符合 BIG5 編碼\r\n";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 驗證 StringMessage 是否符合規則
        /// </summary>
        private bool ValidateStringMessage(StringMessage stringMessage)
        {
            if (stringMessage.StringMode != 0x2A && stringMessage.StringMode != 0x2B)
            {
                txtPacketOutput.Text += $"  警告: StringMode 值無效: 0x{stringMessage.StringMode:X2}\r\n";
                return false;
            }

            // StringBody 可能是 TextStringBody 或其他類型
            if (stringMessage.StringBody is TextStringBody textBody)
            {
                return ValidateStringText(textBody.StringText);
            }

            return true;
        }

        /// <summary>
        /// 驗證 StringBody 是否正確
        /// </summary>
        public bool ValidateStringBody(TextStringBody stringBody)
        {
            bool isColorValid = stringBody.RedColor >= 0x00 && stringBody.RedColor <= 0xFF
                                && stringBody.GreenColor >= 0x00 && stringBody.GreenColor <= 0xFF
                                && stringBody.BlueColor >= 0x00 && stringBody.BlueColor <= 0xFF;

            if (!isColorValid)
            {
                txtPacketOutput.Text += "  警告: RGB 顏色值無效\r\n";
                return false;
            }

            return ValidateStringText(stringBody.StringText);
        }

        /// <summary>
        /// 驗證滾動模式是否正確
        /// </summary>
        public bool ValidateMessageScroll(ScrollInfo scrollInfo)
        {
            if (scrollInfo.ScrollSpeed > 0x09)
            {
                txtPacketOutput.Text += $"  警告: ScrollSpeed 值無效: {scrollInfo.ScrollSpeed}\r\n";
                return false;
            }

            if (scrollInfo.PauseTime > 0x0F)
            {
                txtPacketOutput.Text += $"  警告: PauseTime 值無效: {scrollInfo.PauseTime}\r\n";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 驗證訊息層級
        /// </summary>
        public bool ValidateMessageLevel(byte level)
        {
            if (Enum.IsDefined(typeof(MessageLevel), level))
            {
                return true;
            }
            else
            {
                txtPacketOutput.Text += $"  警告: MessageLevel 值無效: {level}\r\n";
                return false;
            }
        }

        /// <summary>
        /// 驗證字體設定
        /// </summary>
        public bool ValidateMessageFont(Display.Sequence sequence)
        {
            if (!Enum.IsDefined(typeof(FontSize), sequence.Font.Size))
            {
                txtPacketOutput.Text += $"  警告: FontSize 值無效\r\n";
                return false;
            }

            if (!Enum.IsDefined(typeof(Display.FontStyle), sequence.Font.Style))
            {
                txtPacketOutput.Text += $"  警告: FontStyle 值無效\r\n";
                return false;
            }

            return true;
        }

        #endregion
    }
}
