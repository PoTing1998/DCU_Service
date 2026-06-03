using Display;
using Display.DisplayMode;
using Display.Function;
using System;
using System.Collections.Generic;
using System.Text;
using static Display.DisplaySettingsEnums;

namespace UITest.Services
{
    /// <summary>
    /// 封包組成步驟的結果
    /// </summary>
    public class BuildResult<T>
    {
        public bool IsValid { get; set; }
        public T Value { get; set; }
        public string ErrorMessage { get; set; }
        public string HexDump { get; set; }

        public static BuildResult<T> Success(T value, string hexDump = null)
            => new BuildResult<T> { IsValid = true, Value = value, HexDump = hexDump };

        public static BuildResult<T> Fail(string error)
            => new BuildResult<T> { IsValid = false, ErrorMessage = error };
    }

    /// <summary>
    /// 負責封包各層的組建，不依賴任何 UI 元件
    /// </summary>
    public class PacketBuilderService
    {
        private readonly PacketValidationService _validator = new PacketValidationService();

        // ── Step 1：從輸入文字解析 TextStringBody ────────────────────────

        public BuildResult<TextStringBody> BuildTextStringBody(
            string fontColor, string messageContent,
            Func<string, string> pickColor,
            Func<string, byte[]> fromHex)
        {
            try
            {
                var hexColor = pickColor(fontColor);
                var colorBytes = fromHex(hexColor);
                var body = new TextStringBody
                {
                    RedColor   = colorBytes[0],
                    GreenColor = colorBytes[1],
                    BlueColor  = colorBytes[2],
                    StringText = messageContent
                };

                var vr = _validator.ValidateStringBody(body);
                if (!vr.IsValid)
                    return BuildResult<TextStringBody>.Fail(vr.Messages[0]);

                return BuildResult<TextStringBody>.Success(body, ToHex(body.ToBytes()));
            }
            catch (Exception ex)
            {
                return BuildResult<TextStringBody>.Fail($"建立 TextStringBody 失敗：{ex.Message}");
            }
        }

        // ── Step 2：組成 StringMessage ───────────────────────────────────

        public BuildResult<StringMessage> BuildStringMessage(TextStringBody body)
        {
            var msg = new StringMessage
            {
                StringMode = 0x2A,   // TextMode (Static)
                StringBody = body
            };

            var vr = _validator.ValidateStringMessage(msg);
            if (!vr.IsValid)
                return BuildResult<StringMessage>.Fail(vr.Messages[0]);

            return BuildResult<StringMessage>.Success(msg, ToHex(msg.ToBytes()));
        }

        // ── Step 3：組成 FullWindow ──────────────────────────────────────

        public BuildResult<FullWindow> BuildFullWindow(StringMessage stringMessage)
        {
            try
            {
                var fw = new FullWindow
                {
                    MessageType  = 0x71,
                    MessageLevel = 3,
                    MessageScroll = new ScrollInfo
                    {
                        ScrollMode  = 0x64,
                        ScrollSpeed = 7,
                        PauseTime   = 10
                    },
                    MessageContent = new List<StringMessage> { stringMessage }
                };

                if (!Enum.IsDefined(typeof(WindowDisplayMode), (WindowDisplayMode)fw.MessageType))
                    return BuildResult<FullWindow>.Fail("MessageType 無效。");

                var levelVr  = _validator.ValidateMessageLevel(fw.MessageLevel);
                var scrollVr = _validator.ValidateMessageScroll(fw.MessageScroll);

                if (!levelVr.IsValid)  return BuildResult<FullWindow>.Fail(levelVr.Messages[0]);
                if (!scrollVr.IsValid) return BuildResult<FullWindow>.Fail(scrollVr.Messages[0]);

                return BuildResult<FullWindow>.Success(fw, ToHex(fw.ToBytes()));
            }
            catch (Exception ex)
            {
                return BuildResult<FullWindow>.Fail($"建立 FullWindow 失敗：{ex.Message}");
            }
        }

        // ── Step 4：組成 Sequence ────────────────────────────────────────

        public BuildResult<Sequence> BuildSequence(FullWindow fullWindow)
        {
            var seq = new Sequence
            {
                SequenceNo = 1,
                Font       = new FontSetting { Size = FontSize.Font24x24, Style = Display.FontStyle.Ming },
                Messages   = new List<IMessage> { fullWindow }
            };

            var fontVr = _validator.ValidateMessageFont(seq);
            if (!fontVr.IsValid)
                return BuildResult<Sequence>.Fail(fontVr.Messages[0]);

            return BuildResult<Sequence>.Success(seq, ToHex(seq.ToBytes()));
        }

        // ── Step 5：組成完整封包 ─────────────────────────────────────────

        public BuildResult<Packet> BuildPacket(Sequence sequence)
        {
            try
            {
                var processor  = new PacketProcessor();
                var startCode  = new byte[] { 0x55, 0xAA };
                var handler    = new PassengerInfoHandler();
                var ids        = new List<byte> { 0x11, 0x12 };

                var packet = processor.CreatePacket(startCode, ids, handler.FunctionCode, new List<Sequence> { sequence });
                return BuildResult<Packet>.Success(packet, ToHex(packet.ToBytes()));
            }
            catch (Exception ex)
            {
                return BuildResult<Packet>.Fail($"建立封包失敗：{ex.Message}");
            }
        }

        // ── 工具 ─────────────────────────────────────────────────────────

        public static string ToHex(byte[] bytes)
            => string.Join(" ", System.Linq.Enumerable.Select(bytes, b => b.ToString("X2")));

        public static string ExtractValue(string source, string label)
        {
            int start = source.IndexOf(label);
            if (start == -1) return string.Empty;
            start += label.Length;
            int end = source.IndexOf("\r\n", start);
            if (end == -1) end = source.Length;
            return source.Substring(start, end - start).Trim();
        }

        public static byte[] ConvertHexStringToByteArray(string hex)
        {
            hex = hex.Replace(" ", "");
            byte[] arr = new byte[hex.Length / 2];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return arr;
        }
    }
}
