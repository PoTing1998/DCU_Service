using ASI.Lib.Msg.Parsing;
using ASI.Wanda.DCU.TaskSDU;

using Display;
using Display.DisplayMode;
using Display.Function;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Display.DisplaySettingsEnums;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UITest
{
    public partial class Form1 : Form
    {
        private string _mProcName;
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial;
        private string StationID;
        private string AreaID;
        private string DeviceID;
        // 將 textStringBody 定義為類的成員變量
        private TextStringBody textStringBody;
        private StringMessage stringMessage;
        private FullWindow fullWindow;
        private Sequence sequence;
        public Form1()
        {
            InitializeComponent();
            _mProcName = "SDUHelper";
            StationID = "LG01";
            AreaID = "UPF";
            DeviceID = "PDU-1";
            //  取得預錄訊息
            var message_id = ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.GetPlayingItemId(StationID, AreaID, DeviceID);
            //  取得單一一筆預錄訊息的資料
            var message_layout = ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.SelectMessage(message_id);
            data(message_layout);
            var fontColorString = ASI.Wanda.DCU.DB.Tables.System.sysConfig.PickColor(message_layout.font_color);
            var fontColor = DataConversion.FromHex(fontColorString);
            
            var textStringBody = new TextStringBody
            {
                RedColor = fontColor[0],
                GreenColor = fontColor[1],
                BlueColor = fontColor[2],
                StringText = message_layout.message_content
            };
            var stringMessage = new StringMessage
            {
                StringMode = 0x2A, // TextMode (Static) 
                StringBody = textStringBody
            };
            var fullWindowMessage = new FullWindow //Display version
            {
                MessageType = 0x71, // FullWindow message 
                MessageLevel = (byte)message_layout.message_priority, // level
                //滾動模式
                MessageScroll = new ScrollInfo
                {
                    ScrollMode = 0x64,
                    ScrollSpeed = (byte)message_layout.move_speed,
                    PauseTime = 10
                },
                MessageContent = new List<StringMessage> { stringMessage }
            };
            var sequence1 = new Display.Sequence
            {
                SequenceNo = 1,
                Font = new FontSetting { Size = FontSize.Font24x24, Style = Display.FontStyle.Ming },
                Messages = new List<IMessage> { fullWindowMessage }
            };
            var processor = new PacketProcessor();
            var startCode = new byte[] { 0x55, 0xAA };
            var function = new PassengerInfoHandler(); // Use PassengerInfoHandler 
            var packet = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Sequence> { sequence1 });
            var serializedData = processor.SerializePacket(packet);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 獲取當前 textBox1 中的內容
            string textBoxContent = textBox1.Text;

            // 提取字體顏色和字體內容
            string fontColor = ExtractValue(textBoxContent, "字體顏色 : ");
            string messageContent = ExtractValue(textBoxContent, "字體內容 : ");

            // 將 messageContent 轉換為字節數組
            var messageContentByte = Encoding.GetEncoding(950).GetBytes(messageContent);

            // 將字體顏色轉換為字節數組
            var fontColorString = ASI.Wanda.DCU.DB.Tables.System.sysConfig.PickColor(fontColor);
            var fontColorByte = DataConversion.FromHex(fontColorString);

            // 在 textBox2 中顯示提取的字體顏色和內容，並顯示字節數組
            textBox2.Text = $"提取的字體顏色: {fontColorString} \r\n";

            textBox2.Text += $"提取的字體內容: {messageContent} \r\n";
            textStringBody = new TextStringBody
            {
                RedColor = fontColorByte[0],
                GreenColor = fontColorByte[1],
                BlueColor = fontColorByte[2],
                StringText = messageContent
            };
            bool isBodyValid = ValidateStringBody(textStringBody);
            // 顯示規則判斷結果
            if (isBodyValid)
            {
                textBox2.Text += "textStringBody 符合規則。\r\n";
                byte[] byteArray = textStringBody.ToBytes();
                string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2"))); // "X2" 將每個字節格式化為兩位16進制數
                textBox2.Text += $"{messageContentHexString}\r\n";
                textBox2.Text += "========================\r\n";
            }
            else
            {
                textBox2.Text += "textStringBody 不符合規則。\r\n";
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        { // 確保 textStringBody 已被初始化
            if (textStringBody != null)
            {
                // 將 messageContent 轉換為字節數組
                var messageContentByte = Encoding.GetEncoding(950).GetBytes(textStringBody.StringText);

                // 將字節數組轉換為字符串以顯示
                string messageContentByteString = string.Join(" ", messageContentByte);
              
                // 在 textBox2 中顯示字體顏色和內容的字節數組
                textBox2.Text += $"RedColor: {textStringBody.RedColor}\r\n";
                textBox2.Text += $"GreenColor: {textStringBody.GreenColor}\r\n";
                textBox2.Text += $"BlueColor: {textStringBody.BlueColor}\r\n";
                textBox2.Text += $"Message Content (Byte Array): {messageContentByteString}\r\n";

                // 驗證 StringText 是否符合 ASCII 或中文 BIG-5 編碼的條件
                bool isTextValid = ValidateStringText(textStringBody.StringText);

                // 顯示編碼規則的驗證結果
                if (isTextValid)
                {
                    textBox2.Text += "StringText 符合 BIG-5 編碼規則。\r\n";
                }
                else
                {
                    textBox2.Text += "StringText 不符合 BIG-5 編碼規則。\r\n";
                }
                // 如果需要創建 StringMessage 對象
                stringMessage = new StringMessage
                {
                    StringMode = 0x2A, // TextMode (Static) 
                    StringBody = textStringBody
                };
                // 進行規則判斷
                bool isMessageValid = ValidateStringMessage(stringMessage);

                // 顯示規則判斷結果
                if (isMessageValid)
                {
                    textBox2.Text += "StringMessage 符合規則。\r\n";
                    byte[] byteArray = stringMessage.ToBytes();
                    string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2"))); // "X2" 將每個字節格式化為兩位16進制數
                    textBox2.Text += $"{messageContentHexString}\r\n";
                    textBox2.Text += "========================\r\n";
                }
                else
                {
                    textBox2.Text += "StringMessage 不符合規則。\r\n";
                }
               
            }
            else
            {
                textBox2.Text = "尚未初始化 textStringBody。\r\n";
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                fullWindow = new FullWindow //Display version
                {
                    MessageType = 0x71, // FullWindow message 
                    MessageLevel = (byte)3, // level
                                            //滾動模式
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
                    textBox2.Text += "MessageType 的值有效。\r\n";
                    textBox2.Text += $"MessageType: {fullWindow.MessageType}\r\n";
          
                }
                else
                {
                    textBox2.Text += "MessageType 的值無效。\r\n";
                }
                ValidateMessageLevel(fullWindow.MessageLevel);
                ValidateMessageScroll(fullWindow.MessageScroll);

                byte[] byteArray = fullWindow.ToBytes();
                string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2"))); // "X2" 將每個字節格式化為兩位16進制數
                textBox2.Text += $"{messageContentHexString}\r\n";
                textBox2.Text += "========================\r\n";

            }
            catch (Exception)
            {

                MessageBox.Show("組成封包有誤");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
             sequence = new Display.Sequence
            {
                SequenceNo = 1,
                Font = new FontSetting { Size = FontSize.Font24x24, Style = Display.FontStyle.Ming },
                Messages = new List<IMessage> { fullWindow }
            };
           

            // 將字節數組轉換為16進制字符串並顯示
            byte[] byteArray = sequence.ToBytes();
            string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2"))); // "X2" 將每個字節格式化為兩位16進制數

            ValidateMessageFont(sequence);
            textBox2.Text += messageContentHexString;
            textBox2.Text += "========================\r\n";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            var processor = new PacketProcessor();
            var startCode = new byte[] { 0x55, 0xAA };
            var function = new PassengerInfoHandler(); // Use PassengerInfoHandler 
            var packet = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Sequence> { sequence });
            var serializedData = processor.SerializePacket(packet);
            byte[] byteArray = packet.ToBytes();
            string messageContentHexString = string.Join(" ", byteArray.Select(b => b.ToString("X2"))); // "X2" 將每個字節格式化為兩位16進制數
            textBox2.Text += messageContentHexString;
            textBox2.Text += "========================\r\n";
        }

        #region private
        private void data(ASI.Wanda.DCU.DB.Models.DMD.dmd_pre_record_message data)
        {
            textBox1.Text += "字體顏色 : " + data.font_color + "\r\n"; ;
            textBox1.Text += "字體內容 : " + data.message_content + "\r\n"; ;
            textBox1.Text += "訊息等級 : " + data.message_priority + "\r\n"; ;
            textBox1.Text += "訊息速度 :" + data.move_speed + "\r\n"; ;
            textBox1.Text += "訊息間格 : " + data.Interval + "\r\n"; ;
            textBox1.Text += "訊息字體大小 : " + data.font_size + "\r\n"; ;
            textBox1.Text += "訊息字體風格 : " + data.font_type + "\r\n"; ;

        }
        // 用於提取文本中指定標籤後的值
        private string ExtractValue(string source, string label)
        {
            int startIndex = source.IndexOf(label);
            if (startIndex != -1)
            {
                startIndex += label.Length;
                int endIndex = source.IndexOf("\r\n", startIndex);
                if (endIndex == -1) endIndex = source.Length; // 如果找不到換行符則取到末尾
                return source.Substring(startIndex, endIndex - startIndex).Trim();
            }
            return string.Empty;
        }

        // 驗證 StringText 是否符合 ASCII 或中文 BIG-5 編碼
        private bool ValidateStringText(string text)
        {
            foreach (char c in text)
            {
                // 判斷是否是 ASCII 字元 (20h-7Fh)
                if (c >= 0x20 && c <= 0x7F)
                {
                    continue;
                }

                // 判斷是否是中文 BIG-5 編碼範圍內的字元
                byte[] bytes = Encoding.GetEncoding(950).GetBytes(c.ToString());
                if (bytes.Length == 2)
                {
                    continue;
                }

                // 如果不符合上述條件，返回 false
                return false;
            }

            // 如果所有字元都符合條件，返回 true
            return true;
        }


        // 驗證 StringMessage 是否符合規則
        private bool ValidateStringMessage(StringMessage stringMessage)
        {
            // 檢查 StringMode 是否在合法範圍內
            if (stringMessage.StringMode != 0x2A && stringMessage.StringMode != 0x2B)
            {
                return false;
            }
            // 確保 StringBody 內容已驗證
            return ValidateStringText(stringMessage.StringBody.ToString());
        }
        //驗證stringBody是否正確
        public bool ValidateStringBody(TextStringBody stringBody)
        {
            // 確認 StringColor 的值在合法範圍內 (00h-FFh)
            bool isColorValid = stringBody.RedColor >= 0x00 && stringBody.RedColor <= 0xFF
                                && stringBody.GreenColor >= 0x00 && stringBody.BlueColor <= 0xFF;

            if (!isColorValid)
            {
                return false; // 如果顏色不合法，返回 false
            }

            // 驗證 StringText 是否符合 ASCII 或中文 BIG-5 編碼
            bool isTextValid = ValidateStringText(stringBody.StringText);

            return isTextValid;

        }

        public bool ValidateMessageScroll( ScrollInfo fullWindow)
        {
            // 驗證 MessageScroll 的字節數是否正確
            var messageScrollBytes = fullWindow.ToBytes();
            if (messageScrollBytes.Length == 3)
            {
                textBox2.Text += "MessageScroll 符合 3 bytes 的要求。\r\n";
            }
            else
            {
                textBox2.Text += $"MessageScroll 不符合要求，實際長度為 {messageScrollBytes.Length} bytes。\r\n";
            }
            // 驗證 ScrollMode 是否是有效的enum
            if (Enum.IsDefined(typeof(ScrollMode), (ScrollMode)fullWindow.ScrollMode))
            {
                textBox2.Text += "ScrollMode 的值有效。\r\n";
                textBox2.Text += $"ScrollMode: {fullWindow.ScrollMode}\r\n";
            }
            else
            {
                textBox2.Text += "ScrollMode 的值無效。\r\n";
            }

            // 驗證 ScrollSpeed 是否在合法範圍內 (00H - 09H)
            if (fullWindow.ScrollSpeed >= 0x00 && fullWindow.ScrollSpeed <= 0x09)
            {
                textBox2.Text += "ScrollSpeed 的值有效。\r\n";
            }
            else
            {
                textBox2.Text += $"ScrollSpeed 的值無效：{fullWindow.ScrollSpeed}。\r\n";
                return false; // ScrollSpeed 不符，返回 false
            }

            // 驗證 PauseTime 是否在合法範圍內 (00H - 0FH)
            if (fullWindow.PauseTime >= 0x00 && fullWindow.PauseTime <= 0x0F)
            {
                textBox2.Text += "PauseTime 的值有效。\r\n";
            }
            else
            {
                textBox2.Text += $"PauseTime 的值無效：{fullWindow.PauseTime}。\r\n";
                return false; // PauseTime 不符，返回 false
            }

            // 如果所有驗證都通過，返回 true
            return true;
        }

        public bool ValidateMessageLevel(byte level)
        {
            // 將 byte 类型的 MessageLevel 转换为 MessageLevel 枚举类型，并验证是否有效
            if (Enum.IsDefined(typeof(MessageLevel), level))
            {
                textBox2.Text += "MessageLevel 的值有效。\r\n";
                return true;
            }
            else
            {
                textBox2.Text += "MessageLevel 的值無效。\r\n";
                return false;
            }
        }
        public bool ValidateMessageFont(Sequence sequence)
        {
            // 驗證 FontSize 是否有效
            if (Enum.IsDefined(typeof(FontSize), sequence.Font.Size))
            {
                textBox2.Text += "FontSize 的值有效。\r\n";
            }
            else
            {
                textBox2.Text += "FontSize 的值無效。\r\n";
                return false; // 只要有一個無效，就返回 false
            }

            // 驗證 FontStyle 是否有效
            if (Enum.IsDefined(typeof(Display.FontStyle), sequence.Font.Style))
            {
                textBox2.Text += "FontStyle 的值有效。\r\n";
            }
            else
            {
                textBox2.Text += "FontStyle 的值無效。\r\n";
                return false; // 只要有一個無效，就返回 false
            }

            // 如果兩者都有效，返回 true
            return true;
        }


        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
    }
}
