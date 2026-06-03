using Display;

using System;

using static Display.DisplaySettingsEnums;

namespace UITest.PacketVerifiers
{
    public class TrainDynamicVerify : PacketVerifyBase
    {
        protected override bool CheckSequenceLength(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (data[i] != 0x02) { errorMessage = $"Expected 0x02 at byte {i}"; return false; }
            i++;
            if (i + 2 > data.Length) { errorMessage = $"Insufficient data for SequenceLength at byte {i}"; return false; }
            int seqLen = data[i] | (data[i + 1] << 8); i += 2;
            int seqEnd = i + seqLen - 2;
            if (seqEnd + 1 >= data.Length) { errorMessage = $"Sequence length beyond data at byte {seqEnd}"; return false; }
            return true;
        }

        protected override bool CheckMessageType(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            WindowDisplayMode msgType = (WindowDisplayMode)data[i];
            if (!Enum.IsDefined(typeof(WindowDisplayMode), msgType))
            { errorMessage = $"Invalid messageType at byte {i}, received {data[i]:X2}"; return false; }
            return MessageTypeHandlerFactory.GetHandler(msgType).Handle(data, ref i, out errorMessage);
        }

        public override bool ValidatePacket(byte[] data, out string errorMessage)
        {
            int i = 0; errorMessage = "";
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
                if (!CheckStringMode(data, ref i, out errorMessage))     { errorMessage = $"[Step 15] {errorMessage}"; return false; }
                if (!CheckPhotoIndex(data, ref i, out errorMessage))     { errorMessage = $"[Step 16] {errorMessage}"; return false; }
                if (!CheckStringMode(data, ref i, out errorMessage))     { errorMessage = $"[Step 17] {errorMessage}"; return false; }
                if (!CheckStringText(data, ref i, out errorMessage))     { errorMessage = $"[Step 18] {errorMessage}"; return false; }
                if (!CheckStringMode(data, ref i, out errorMessage))     { errorMessage = $"[Step 19] {errorMessage}"; return false; }
                if (!CheckPhotoIndex(data, ref i, out errorMessage))     { errorMessage = $"[Step 20] {errorMessage}"; return false; }
                if (!CheckStringMode(data, ref i, out errorMessage))     { errorMessage = $"[Step 21] {errorMessage}"; return false; }
                if (!CheckStringText(data, ref i, out errorMessage))     { errorMessage = $"[Step 22] {errorMessage}"; return false; }
                if (!CheckEndBytes(data, ref i, out errorMessage))       { errorMessage = $"[Step 23] {errorMessage}"; return false; }
            }
            catch (Exception ex) { errorMessage = $"Exception: {ex.Message}"; return false; }
            return true;
        }

        private bool CheckPhotoIndex(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i + 2 > data.Length) { errorMessage = $"Insufficient data for GraphicStartIndex at byte {i}"; return false; }
            i += 2;
            if (i + 1 > data.Length) { errorMessage = $"Insufficient data for GraphicNumber at byte {i}"; return false; }
            i += 1;
            if (i + 3 > data.Length) { errorMessage = $"Insufficient data for GraphicColor at byte {i}"; return false; }
            i += 3;
            if (i >= data.Length || data[i] != 0x1F)
            { errorMessage = $"Expected 0x1F at byte {i}"; return false; }
            i++;
            return true;
        }
    }
}
