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
    /// 封包組成測試相關功能
    /// </summary>
    public partial class Form1
    {
        #region Packet Builder - Button Click Events

        /// <summary>
        /// 1. 建立 StringBody
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            txtPacketOutput.Clear();

            string textBoxContent = txtMessageInput.Text;
            string fontColor = ExtractValue(textBoxContent, "字體顏色 : ");
            string messageContent = ExtractValue(textBoxContent, "字體內容 : ");

            var messageContentByte = Encoding.GetEncoding(950).GetBytes(messageContent);
            var fontColorString = ColorHelper.GetColorHex(fontColor);
            var fontColorByte = ColorHelper.GetColorBytes(fontColor);

            txtPacketOutput.Text = $"提取的字體顏色: {fontColorString} \r\n";
            txtPacketOutput.Text += $"提取的字體內容: {messageContent} \r\n";

            textStringBody = new TextStringBody
            {
                RedColor = fontColorByte[0],
                GreenColor = fontColorByte[1],
                BlueColor = fontColorByte[2],
                StringText = messageContent
            };

            bool isBodyValid = ValidateStringBody(textStringBody);

            if (isBodyValid)
            {
                txtPacketOutput.Text += "textStringBody 符合規則。\r\n";
                byte[] byteArray = textStringBody.ToBytes();
                string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2")));
                txtPacketOutput.Text += $"{messageContentHexString}\r\n";
                txtPacketOutput.Text += "========================\r\n";
            }
            else
            {
                txtPacketOutput.Text += "textStringBody 不符合規則。\r\n";
            }
        }

        /// <summary>
        /// 2. 建立 StringMessage
        /// </summary>
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (textStringBody != null)
            {
                var messageContentByte = Encoding.GetEncoding(950).GetBytes(textStringBody.StringText);
                string messageContentByteString = string.Join(" ", messageContentByte);

                txtPacketOutput.Text += $"RedColor: {textStringBody.RedColor}\r\n";
                txtPacketOutput.Text += $"GreenColor: {textStringBody.GreenColor}\r\n";
                txtPacketOutput.Text += $"BlueColor: {textStringBody.BlueColor}\r\n";
                txtPacketOutput.Text += $"Message Content (Byte Array): {messageContentByteString}\r\n";

                bool isTextValid = ValidateStringText(textStringBody.StringText);

                if (isTextValid)
                {
                    txtPacketOutput.Text += "StringText 符合 BIG-5 編碼規則。\r\n";
                }
                else
                {
                    txtPacketOutput.Text += "StringText 不符合 BIG-5 編碼規則。\r\n";
                }

                stringMessage = new StringMessage
                {
                    StringMode = 0x2A,
                    StringBody = textStringBody
                };

                bool isMessageValid = ValidateStringMessage(stringMessage);

                if (isMessageValid)
                {
                    txtPacketOutput.Text += "StringMessage 符合規則。\r\n";
                    byte[] byteArray = stringMessage.ToBytes();
                    string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2")));
                    txtPacketOutput.Text += $"{messageContentHexString}\r\n";
                    txtPacketOutput.Text += "========================\r\n";
                }
                else
                {
                    txtPacketOutput.Text += "StringMessage 不符合規則。\r\n";
                }
            }
            else
            {
                txtPacketOutput.Text = "尚未初始化 textStringBody。\r\n";
            }
        }

        /// <summary>
        /// 3. 建立 FullWindow
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            if (stringMessage == null)
            {
                txtPacketOutput.Text += "錯誤: 尚未建立 StringMessage。請先點擊「2. 建立 StringMessage」\r\n";
                return;
            }

            try
            {
                fullWindow = new FullWindow
                {
                    MessageType = 0x71,
                    MessageLevel = (byte)3,
                    MessageScroll = new ScrollInfo
                    {
                        ScrollMode = 0x64,
                        ScrollSpeed = (byte)7,
                        PauseTime = 10
                    },
                    MessageContent = new List<StringMessage> { stringMessage }
                };

                if (Enum.IsDefined(typeof(WindowDisplayMode), (WindowDisplayMode)fullWindow.MessageType))
                {
                    txtPacketOutput.Text += "MessageType 的值有效。\r\n";
                    txtPacketOutput.Text += $"MessageType: {fullWindow.MessageType}\r\n";
                }
                else
                {
                    txtPacketOutput.Text += "MessageType 的值無效。\r\n";
                }

                ValidateMessageLevel(fullWindow.MessageLevel);
                ValidateMessageScroll(fullWindow.MessageScroll);

                byte[] byteArray = fullWindow.ToBytes();
                string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2")));
                txtPacketOutput.Text += $"{messageContentHexString}\r\n";
                txtPacketOutput.Text += "========================\r\n";
            }
            catch (Exception)
            {
                MessageBox.Show("組成封包有誤");
            }
        }

        /// <summary>
        /// 4. 建立 Sequence
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            if (fullWindow == null)
            {
                txtPacketOutput.Text += "錯誤: 尚未建立 FullWindow。請先點擊「3. 建立 FullWindow」\r\n";
                return;
            }

            sequence = new Display.Sequence
            {
                SequenceNo = 1,
                Font = new FontSetting { Size = FontSize.Font24x24, Style = Display.FontStyle.Ming },
                Messages = new List<IMessage> { fullWindow }
            };

            byte[] byteArray = sequence.ToBytes();
            ValidateMessageFont(sequence);
            string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2")));
            txtPacketOutput.Text += messageContentHexString;
            txtPacketOutput.Text += "\r\n========================\r\n";
        }

        /// <summary>
        /// 5. 建立 Packet
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            if (sequence == null)
            {
                txtPacketOutput.Text += "錯誤: 尚未建立 Sequence。請先點擊「4. 建立 Sequence」\r\n";
                return;
            }

            var processor = new PacketProcessor();
            var startCode = new byte[] { 0x55, 0xAA };
            var function = new PassengerInfoHandler();
            var packet = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Sequence> { sequence });

            byte[] byteArray = packet.ToBytes();
            string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2")));
            txtPacketOutput.Text += messageContentHexString;
            txtPacketOutput.Text += "\r\n========================\r\n";
        }

        /// <summary>
        /// 清除封包輸出和中間變數
        /// </summary>
        private void btnClearPacketOutput_Click(object sender, EventArgs e)
        {
            txtPacketOutput.Clear();
            textStringBody = null;
            stringMessage = null;
            fullWindow = null;
            sequence = null;
        }

        #endregion

        #region Packet Builder - Helper Methods

        /// <summary>
        /// 從文本中提取指定標籤後的值
        /// </summary>
        private string ExtractValue(string source, string label)
        {
            int startIndex = source.IndexOf(label);
            if (startIndex != -1)
            {
                startIndex += label.Length;
                int endIndex = source.IndexOf("\r\n", startIndex);
                if (endIndex == -1) endIndex = source.Length;
                return source.Substring(startIndex, endIndex - startIndex).Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// 驗證字串是否符合 ASCII 或中文 BIG-5 編碼
        /// </summary>
        private bool ValidateStringText(string text)
        {
            foreach (char c in text)
            {
                if (c >= 0x20 && c <= 0x7F)
                {
                    continue;
                }

                byte[] bytes = Encoding.GetEncoding(950).GetBytes(c.ToString());
                if (bytes.Length == 2)
                {
                    continue;
                }

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
                return false;
            }
            return ValidateStringText(stringMessage.StringBody.ToString());
        }

        /// <summary>
        /// 驗證 StringBody 是否正確
        /// </summary>
        public bool ValidateStringBody(TextStringBody stringBody)
        {
            bool isColorValid = stringBody.RedColor >= 0x00 && stringBody.RedColor <= 0xFF
                                && stringBody.GreenColor >= 0x00 && stringBody.BlueColor <= 0xFF;
            if (!isColorValid)
            {
                return false;
            }

            bool isTextValid = ValidateStringText(stringBody.StringText);
            return isTextValid;
        }

        /// <summary>
        /// 驗證滾動模式是否正確
        /// </summary>
        public bool ValidateMessageScroll(ScrollInfo fullWindow)
        {
            var messageScrollBytes = fullWindow.ToBytes();
            if (messageScrollBytes.Length == 3)
            {
                txtPacketOutput.Text += "MessageScroll 符合 3 bytes 的要求。\r\n";
            }
            else
            {
                txtPacketOutput.Text += $"MessageScroll 不符合要求，實際長度為 {messageScrollBytes.Length} bytes。\r\n";
            }

            if (Enum.IsDefined(typeof(ScrollMode), (ScrollMode)fullWindow.ScrollMode))
            {
                txtPacketOutput.Text += "ScrollMode 的值有效。\r\n";
                txtPacketOutput.Text += $"ScrollMode: {fullWindow.ScrollMode}\r\n";
            }
            else
            {
                txtPacketOutput.Text += "ScrollMode 的值無效。\r\n";
            }

            if (fullWindow.ScrollSpeed >= 0x00 && fullWindow.ScrollSpeed <= 0x09)
            {
                txtPacketOutput.Text += "ScrollSpeed 的值有效。\r\n";
            }
            else
            {
                txtPacketOutput.Text += $"ScrollSpeed 的值無效：{fullWindow.ScrollSpeed}。\r\n";
                return false;
            }

            if (fullWindow.PauseTime >= 0x00 && fullWindow.PauseTime <= 0x0F)
            {
                txtPacketOutput.Text += "PauseTime 的值有效。\r\n";
            }
            else
            {
                txtPacketOutput.Text += $"PauseTime 的值無效：{fullWindow.PauseTime}。\r\n";
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
                txtPacketOutput.Text += "MessageLevel 的值有效。\r\n";
                return true;
            }
            else
            {
                txtPacketOutput.Text += "MessageLevel 的值無效。\r\n";
                return false;
            }
        }

        /// <summary>
        /// 驗證字體設定
        /// </summary>
        public bool ValidateMessageFont(Sequence sequence)
        {
            if (Enum.IsDefined(typeof(FontSize), sequence.Font.Size))
            {
                txtPacketOutput.Text += "FontSize 的值有效。\r\n";
            }
            else
            {
                txtPacketOutput.Text += "FontSize 的值無效。\r\n";
                return false;
            }

            if (Enum.IsDefined(typeof(Display.FontStyle), sequence.Font.Style))
            {
                txtPacketOutput.Text += "FontStyle 的值有效。\r\n";
            }
            else
            {
                txtPacketOutput.Text += "FontStyle 的值無效。\r\n";
                return false;
            }

            return true;
        }

        #endregion
    }
}
