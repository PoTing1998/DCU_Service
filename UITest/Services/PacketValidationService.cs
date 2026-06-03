using Display;
using Display.DisplayMode;
using System;
using System.Collections.Generic;
using System.Text;
using static Display.DisplaySettingsEnums;

namespace UITest.Services
{
    /// <summary>
    /// 驗證結果，包含是否通過及相關訊息
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Messages { get; set; } = new List<string>();

        public static ValidationResult Success(params string[] messages)
        {
            var r = new ValidationResult { IsValid = true };
            r.Messages.AddRange(messages);
            return r;
        }

        public static ValidationResult Fail(params string[] messages)
        {
            var r = new ValidationResult { IsValid = false };
            r.Messages.AddRange(messages);
            return r;
        }
    }

    /// <summary>
    /// 純業務邏輯，不依賴任何 UI 元件
    /// </summary>
    public class PacketValidationService
    {
        // ── 字串 / 編碼驗證 ──────────────────────────────────────────────

        public ValidationResult ValidateStringText(string text)
        {
            foreach (char c in text)
            {
                if (c >= 0x20 && c <= 0x7F) continue;

                byte[] bytes = Encoding.GetEncoding(950).GetBytes(c.ToString());
                if (bytes.Length == 2) continue;

                return ValidationResult.Fail("StringText 不符合 BIG-5 編碼規則。");
            }
            return ValidationResult.Success("StringText 符合 BIG-5 編碼規則。");
        }

        public ValidationResult ValidateStringBody(TextStringBody stringBody)
        {
            bool colorOk = stringBody.RedColor >= 0x00 && stringBody.RedColor <= 0xFF
                        && stringBody.GreenColor >= 0x00 && stringBody.BlueColor <= 0xFF;
            if (!colorOk)
                return ValidationResult.Fail("StringBody 顏色值不合法。");

            return ValidateStringText(stringBody.StringText);
        }

        public ValidationResult ValidateStringMessage(StringMessage stringMessage)
        {
            if (stringMessage.StringMode != 0x2A && stringMessage.StringMode != 0x2B)
                return ValidationResult.Fail("StringMode 不在合法範圍內。");

            return ValidateStringText(stringMessage.StringBody.ToString());
        }

        // ── Scroll / Level / Font 驗證 ───────────────────────────────────

        public ValidationResult ValidateMessageScroll(ScrollInfo scroll)
        {
            var msgs = new List<string>();

            byte[] bytes = scroll.ToBytes();
            msgs.Add(bytes.Length == 3
                ? "MessageScroll 符合 3 bytes 的要求。"
                : $"MessageScroll 不符合要求，實際長度為 {bytes.Length} bytes。");

            if (!Enum.IsDefined(typeof(ScrollMode), (ScrollMode)scroll.ScrollMode))
                return ValidationResult.Fail(string.Join("\r\n", msgs) + "\r\nScrollMode 的值無效。");
            msgs.Add($"ScrollMode 的值有效：{scroll.ScrollMode}");

            if (scroll.ScrollSpeed > 0x09)
                return ValidationResult.Fail(string.Join("\r\n", msgs) + $"\r\nScrollSpeed 的值無效：{scroll.ScrollSpeed}。");
            msgs.Add("ScrollSpeed 的值有效。");

            if (scroll.PauseTime > 0x0F)
                return ValidationResult.Fail(string.Join("\r\n", msgs) + $"\r\nPauseTime 的值無效：{scroll.PauseTime}。");
            msgs.Add("PauseTime 的值有效。");

            return ValidationResult.Success(msgs.ToArray());
        }

        public ValidationResult ValidateMessageLevel(byte level)
        {
            return Enum.IsDefined(typeof(MessageLevel), level)
                ? ValidationResult.Success("MessageLevel 的值有效。")
                : ValidationResult.Fail("MessageLevel 的值無效。");
        }

        public ValidationResult ValidateMessageFont(Sequence sequence)
        {
            if (!Enum.IsDefined(typeof(FontSize), sequence.Font.Size))
                return ValidationResult.Fail("FontSize 的值無效。");

            if (!Enum.IsDefined(typeof(Display.FontStyle), sequence.Font.Style))
                return ValidationResult.Fail("FontStyle 的值無效。");

            return ValidationResult.Success("FontSize 的值有效。", "FontStyle 的值有效。");
        }

        // ── 完整封包驗證 ─────────────────────────────────────────────────

        public bool ValidatePacket(byte[] data, out string errorMessage)
        {
            int i = 0;
            errorMessage = "";
            try
            {
                if (data[i] != 0x55 || data[i + 1] != 0xAA)
                { errorMessage = $"StartCode mismatch at byte {i}"; return false; }
                i += 2;

                byte idLen = data[i];
                if (idLen == 0x00) { errorMessage = $"Invalid ID_LENGTH at byte {i}"; return false; }
                i++;

                if (i + idLen > data.Length) { errorMessage = $"ID length exceeds data at byte {i}"; return false; }
                i += idLen;

                if (data[i] != 0x34) { errorMessage = $"FunctionCode mismatch at byte {i}"; return false; }
                i++;

                if (i + 2 > data.Length) { errorMessage = $"Insufficient data for Data_Length at byte {i}"; return false; }
                int dataLen = data[i] | (data[i + 1] << 8);
                i += 2;

                if (dataLen != data.Length - i - 1)
                { errorMessage = $"Data_Length mismatch at byte {i}"; return false; }

                if (data[i] != 0x01) { errorMessage = $"Expected 0x01 at byte {i}"; return false; }
                i++;

                if (i + 2 > data.Length) { errorMessage = $"Insufficient data for SequenceLength at byte {i}"; return false; }
                int seqLen = data[i] | (data[i + 1] << 8);
                i += 2;

                int seqEnd = i + seqLen - 2;
                if (seqEnd + 1 >= data.Length) { errorMessage = $"Sequence length beyond data at byte {seqEnd}"; return false; }

                if (i + 2 > data.Length || data[i] != 0x77 || data[i + 1] != 0x7F)
                { errorMessage = $"Expected Clear Command [0x77, 0x7F] at byte {i}"; return false; }
                i += 2;

                if (!Enum.IsDefined(typeof(FontSize), (FontSize)data[i]))
                { errorMessage = $"Invalid FontSize at byte {i}"; return false; }
                i++;

                if (!Enum.IsDefined(typeof(Display.FontStyle), (Display.FontStyle)data[i]))
                { errorMessage = $"Invalid FontStyle at byte {i}"; return false; }
                i++;

                if (!Enum.IsDefined(typeof(WindowDisplayMode), (WindowDisplayMode)data[i]))
                { errorMessage = $"Invalid messageType at byte {i}"; return false; }
                i++;

                if (i + 2 > data.Length) { errorMessage = $"Insufficient data for MessageLength at byte {i}"; return false; }
                int msgLen = data[i] | (data[i + 1] << 8);
                i += 2;

                int msgEnd = i + msgLen - 1;
                if (msgEnd >= data.Length || data[msgEnd] != 0x1E)
                { errorMessage = $"Message does not end with 0x1E at byte {msgEnd}"; return false; }

                byte level = data[i];
                if (level < 0x01 || level > 0x04)
                { errorMessage = $"Invalid MessageLevel at byte {i}"; return false; }
                i++;

                if (i + 3 > data.Length) { errorMessage = $"Insufficient data for MessageScroll at byte {i}"; return false; }
                byte scrollMode = data[i], scrollSpeed = data[i + 1], pauseTime = data[i + 2];
                i += 3;

                if (!Enum.IsDefined(typeof(ScrollMode), (ScrollMode)scrollMode))
                { errorMessage = $"Invalid ScrollMode at byte {i - 3}"; return false; }
                if (scrollSpeed > 0x09) { errorMessage = $"Invalid ScrollSpeed at byte {i - 2}"; return false; }
                if (pauseTime > 0xFF) { errorMessage = $"Invalid PauseTime at byte {i - 1}"; return false; }

                if (i >= data.Length || !Enum.IsDefined(typeof(StringMode), (StringMode)data[i]))
                { errorMessage = $"Invalid StringMode at byte {i}"; return false; }
                i++;

                int endIdx = Array.IndexOf(data, (byte)0x1F, i);
                if (endIdx == -1) { errorMessage = $"No 0x1F found from byte {i}"; return false; }

                // decode text (BIG-5)
                Encoding.GetEncoding(950).GetString(data, i, endIdx - i);
                i = endIdx + 1;

                if (i >= data.Length || data[i] != 0x1E) { errorMessage = $"Expected 0x1E at byte {i}"; return false; }
                i++;
                if (i >= data.Length || data[i] != 0x1D) { errorMessage = $"Expected 0x1D at byte {i}"; return false; }
            }
            catch (Exception ex)
            {
                errorMessage = $"Exception: {ex.Message}";
                return false;
            }
            return true;
        }
    }
}
