using Display;

using System;
using System.Text;

using static Display.DisplaySettingsEnums;

namespace UITest.PacketVerifiers
{
    /// <summary>
    /// 封包驗證基底類別。
    /// - 共用的 14 個 Check 方法定義於此（protected）
    /// - CheckMessageType 為抽象方法，各子類別自行實作
    /// - CheckFunctionCode / CheckSequenceLength / CheckClearCommand 為 virtual，
    ///   有特殊規則的子類別（如 UrgentHandlerVerify）可覆寫
    /// - ValidatePacket 為 virtual，步驟不同的子類別（如 TrainDynamicVerify）可覆寫
    /// </summary>
    public abstract class PacketVerifyBase
    {
        // ── 公開進入點 ────────────────────────────────────────────────────

        public virtual bool ValidatePacket(byte[] data, out string errorMessage)
        {
            int i = 0;
            errorMessage = "";
            try
            {
                if (!CheckStartCode(data, ref i, out errorMessage))      { errorMessage = $"[Step 1]  {errorMessage}"; return false; }
                if (!CheckIDLength(data, ref i, out errorMessage))       { errorMessage = $"[Step 2]  {errorMessage}"; return false; }
                if (!CheckFunctionCode(data, ref i, out errorMessage))   { errorMessage = $"[Step 3]  {errorMessage}"; return false; }
                if (!CheckDataLength(data, ref i, out errorMessage))     { errorMessage = $"[Step 4]  {errorMessage}"; return false; }
                if (!CheckSequenceLength(data, ref i, out errorMessage)) { errorMessage = $"[Step 5]  {errorMessage}"; return false; }
                if (!CheckClearCommand(data, ref i, out errorMessage))   { errorMessage = $"[Step 6]  {errorMessage}"; return false; }
                if (!CheckFontSize(data, ref i, out errorMessage))       { errorMessage = $"[Step 7]  {errorMessage}"; return false; }
                if (!CheckFontStyle(data, ref i, out errorMessage))      { errorMessage = $"[Step 8]  {errorMessage}"; return false; }
                if (!CheckMessageType(data, ref i, out errorMessage))    { errorMessage = $"[Step 9]  {errorMessage}"; return false; }
                if (!CheckMessageLength(data, ref i, out errorMessage))  { errorMessage = $"[Step 10] {errorMessage}"; return false; }
                if (!CheckMessageLevel(data, ref i, out errorMessage))   { errorMessage = $"[Step 11] {errorMessage}"; return false; }
                if (!CheckMessageScroll(data, ref i, out errorMessage))  { errorMessage = $"[Step 12] {errorMessage}"; return false; }
                if (!CheckStringMode(data, ref i, out errorMessage))     { errorMessage = $"[Step 13] {errorMessage}"; return false; }
                if (!CheckStringText(data, ref i, out errorMessage))     { errorMessage = $"[Step 14] {errorMessage}"; return false; }
                if (!CheckEndBytes(data, ref i, out errorMessage))       { errorMessage = $"[Step 15] {errorMessage}"; return false; }
            }
            catch (Exception ex)
            {
                errorMessage = $"Exception occurred: {ex.Message}";
                return false;
            }
            return true;
        }

        // ── 抽象方法（子類別必須實作）────────────────────────────────────

        protected abstract bool CheckMessageType(byte[] data, ref int i, out string errorMessage);

        // ── Virtual 方法（有特殊規則時可覆寫）───────────────────────────

        protected virtual bool CheckFunctionCode(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (data[i] != 0x34)
            {
                errorMessage = $"FunctionCode mismatch at byte {i}, expected 0x34";
                return false;
            }
            i++;
            return true;
        }

        protected virtual bool CheckSequenceLength(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (data[i] != 0x01 && data[i] != 0x02)
            {
                errorMessage = $"Expected 0x01 or 0x02 at byte {i}";
                return false;
            }
            i++;

            if (i + 2 > data.Length)
            {
                errorMessage = $"Insufficient data for SequenceLength at byte {i}";
                return false;
            }

            int seqLen = data[i] | (data[i + 1] << 8);
            i += 2;

            int seqEnd = i + seqLen - 2;
            if (seqEnd + 1 >= data.Length)
            {
                errorMessage = $"Sequence length extends beyond data length at byte {seqEnd}";
                return false;
            }
            return true;
        }

        protected virtual bool CheckClearCommand(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i + 1 > data.Length)
            {
                errorMessage = $"Insufficient data for Clear Command check at byte {i}";
                return false;
            }

            if (data[i] == 0x77) i++;   // 0x77 是可選的

            if (i >= data.Length || data[i] != 0x7F)
            {
                errorMessage = $"Expected Clear Command [optional 0x77, 0x7F] at byte {i}";
                return false;
            }
            i++;
            return true;
        }

        // ── 共用 Protected 方法（所有子類別共用，不需覆寫）──────────────

        protected bool CheckStartCode(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (data[i] != 0x55 || data[i + 1] != 0xAA)
            {
                errorMessage = $"StartCode mismatch at byte {i}, expected [0x55, 0xAA]";
                return false;
            }
            i += 2;
            return true;
        }

        protected bool CheckIDLength(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            byte idLen = data[i];
            if (idLen == 0x00)
            {
                errorMessage = $"Invalid ID_LENGTH at byte {i}, cannot be 0x00";
                return false;
            }
            i++;

            if (i + idLen > data.Length || idLen < 1)
            {
                errorMessage = $"ID length is invalid or exceeds data length at byte {i}";
                return false;
            }
            i += idLen;
            return true;
        }

        protected bool CheckDataLength(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i + 2 > data.Length)
            {
                errorMessage = $"Insufficient data for Data_Length at byte {i}";
                return false;
            }

            int dataLen = data[i] | (data[i + 1] << 8);
            i += 2;

            int remaining = data.Length - i - 1;
            if (dataLen != remaining)
            {
                errorMessage = $"Data_Length mismatch at byte {i}, expected {remaining}, got {dataLen}";
                return false;
            }
            return true;
        }

        protected bool CheckFontSize(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            FontSize fontSize = (FontSize)data[i];
            if (!Enum.IsDefined(typeof(FontSize), fontSize))
            {
                errorMessage = $"Invalid FontSize at byte {i}, received {data[i]:X2}";
                return false;
            }
            i++;
            return true;
        }

        protected bool CheckFontStyle(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            Display.FontStyle fontStyle = (Display.FontStyle)data[i];
            if (!Enum.IsDefined(typeof(Display.FontStyle), fontStyle))
            {
                errorMessage = $"Invalid FontStyle at byte {i}, received {data[i]:X2}";
                return false;
            }
            i++;
            return true;
        }

        protected bool CheckMessageLength(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i + 2 > data.Length)
            {
                errorMessage = $"Insufficient data for MessageLength at byte {i}";
                return false;
            }

            int msgLen = data[i] | (data[i + 1] << 8);
            i += 2;

            int msgEnd = i + msgLen - 1;
            if (msgEnd >= data.Length || data[msgEnd] != 0x1E)
            {
                errorMessage = $"Message does not end with 0x1E or length is incorrect at byte {msgEnd}";
                return false;
            }
            return true;
        }

        protected bool CheckMessageLevel(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            byte level = data[i];
            if (level < 0x01 || level > 0x04)
            {
                errorMessage = $"Invalid MessageLevel at byte {i}, expected value between 0x01 and 0x04";
                return false;
            }
            i++;
            return true;
        }

        protected bool CheckMessageScroll(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i + 3 > data.Length)
            {
                errorMessage = $"Insufficient data for MessageScroll at byte {i}";
                return false;
            }

            byte scrollMode  = data[i];
            byte scrollSpeed = data[i + 1];
            byte pauseTime   = data[i + 2];
            i += 3;

            if (!Enum.IsDefined(typeof(ScrollMode), (ScrollMode)scrollMode))
            {
                errorMessage = $"Invalid ScrollMode at byte {i - 3}, received {scrollMode:X2}";
                return false;
            }
            if (scrollSpeed > 0x09)
            {
                errorMessage = $"Invalid ScrollSpeed at byte {i - 2}, received {scrollSpeed:X2}";
                return false;
            }
            if (pauseTime > 0xFF)
            {
                errorMessage = $"Invalid PauseTime at byte {i - 1}, received {pauseTime:X2}";
                return false;
            }
            return true;
        }

        protected bool CheckStringMode(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i >= data.Length)
            {
                errorMessage = $"Insufficient data for StringMode at byte {i}";
                return false;
            }

            StringMode mode = (StringMode)data[i];
            if (!Enum.IsDefined(typeof(StringMode), mode))
            {
                errorMessage = $"Invalid StringMode at byte {i}, received {data[i]:X2}";
                return false;
            }
            i++;
            return true;
        }

        protected bool CheckStringText(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            int endIdx = Array.IndexOf(data, (byte)0x1F, i);
            if (endIdx == -1)
            {
                errorMessage = $"Message content does not end with 0x1F, starting at byte {i}";
                return false;
            }

            // decode BIG-5（驗證可解碼即可）
            Encoding.GetEncoding(950).GetString(data, i, endIdx - i);

            i = endIdx + 1;
            return true;
        }

        protected bool CheckEndBytes(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i >= data.Length || data[i] != 0x1E)
            {
                errorMessage = $"Expected 0x1E at byte {i}, but found {(i < data.Length ? data[i].ToString("X2") : "EOF")}";
                return false;
            }
            i++;

            if (i >= data.Length || data[i] != 0x1D)
            {
                errorMessage = $"Expected 0x1D at byte {i}, but found {(i < data.Length ? data[i].ToString("X2") : "EOF")}";
                return false;
            }
            i++;
            return true;
        }
    }
}
