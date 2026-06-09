using ASI.Lib.Msg.Parsing;
using Display;
using Display.DisplayMode;
using Display.Function;
using System;
using System.Collections.Generic;
using static Display.DisplaySettingsEnums;

namespace UITest.Services
{
    /// <summary>封包組成步驟的結果</summary>
    public class BuildResult<T>
    {
        public bool   IsValid      { get; set; }
        public T      Value        { get; set; }
        public string ErrorMessage { get; set; }
        public string HexDump      { get; set; }

        public static BuildResult<T> Success(T value, string hexDump = null)
            => new BuildResult<T> { IsValid = true, Value = value, HexDump = hexDump };

        public static BuildResult<T> Fail(string error)
            => new BuildResult<T> { IsValid = false, ErrorMessage = error };
    }

    /// <summary>負責封包各層的組建，不依賴任何 UI 元件</summary>
    public class PacketBuilderService
    {
        private readonly PacketValidationService _validator = new PacketValidationService();

        // ── Step 1：建立 TextStringBody ──────────────────────────────────

        /// <param name="hexColor">RGB Hex 字串，例如 "FF0000" 或 "#FF0000"</param>
        /// <param name="messageContent">顯示文字（BIG-5 相容）</param>
        public BuildResult<TextStringBody> BuildTextStringBody(string hexColor, string messageContent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hexColor))
                    return BuildResult<TextStringBody>.Fail("顏色不能為空。");

                byte[] colorBytes = DataConversion.FromHex(hexColor);
                if (colorBytes == null || colorBytes.Length != 3)
                    return BuildResult<TextStringBody>.Fail("顏色格式錯誤，請輸入 6 碼 Hex（例如 FF0000）。");

                var body = new TextStringBody
                {
                    RedColor   = colorBytes[0],
                    GreenColor = colorBytes[1],
                    BlueColor  = colorBytes[2],
                    StringText = messageContent ?? string.Empty
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

        // ── Step 2：建立 StringMessage ───────────────────────────────────

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

        // ── Step 3：建立 FullWindow ──────────────────────────────────────

        /// <param name="level">訊息等級 1–4</param>
        /// <param name="scroll">捲動設定</param>
        public BuildResult<FullWindow> BuildFullWindow(StringMessage stringMessage, byte level, ScrollInfo scroll)
        {
            try
            {
                var fw = new FullWindow
                {
                    MessageType   = 0x71,
                    MessageLevel  = level,
                    MessageScroll = scroll,
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

        // ── Step 4：建立 Sequence ────────────────────────────────────────

        /// <param name="font">字型設定</param>
        public BuildResult<Sequence> BuildSequence(FullWindow fullWindow, FontSetting font)
        {
            var seq = new Sequence
            {
                SequenceNo = 1,
                Font       = font,
                Messages   = new List<IMessage> { fullWindow }
            };

            var fontVr = _validator.ValidateMessageFont(seq);
            if (!fontVr.IsValid)
                return BuildResult<Sequence>.Fail(fontVr.Messages[0]);

            return BuildResult<Sequence>.Success(seq, ToHex(seq.ToBytes()));
        }

        // ── Step 5：建立完整封包 ─────────────────────────────────────────

        /// <param name="ids">目標顯示板 ID 清單</param>
        /// <param name="functionCode">功能碼（例如 0x34）</param>
        public BuildResult<Packet> BuildPacket(Sequence sequence, List<byte> ids, byte functionCode)
        {
            try
            {
                var processor = new PacketProcessor();
                var startCode = new byte[] { 0x55, 0xAA };
                var packet    = processor.CreatePacket(startCode, ids, functionCode, new List<Sequence> { sequence });
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
    }
}
