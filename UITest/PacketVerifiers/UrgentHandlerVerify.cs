using Display;

using System;

using static Display.DisplaySettingsEnums;

namespace UITest.PacketVerifiers
{
    public class UrgentHandlerVerify : PacketVerifyBase
    {
        protected override bool CheckFunctionCode(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (data[i] != 0x38) { errorMessage = $"FunctionCode mismatch at byte {i}, expected 0x38"; return false; }
            i++; return true;
        }

        protected override bool CheckSequenceLength(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (data[i] != 0x01 && data[i] != 0x02) { errorMessage = $"Expected 0x01 or 0x02 at byte {i} for signal activation"; return false; }
            i++;
            if (data[i] != 0x01 && data[i] != 0x02) { errorMessage = $"Expected 0x01 or 0x02 at byte {i} for display mode"; return false; }
            i++;
            if (i + 2 > data.Length) { errorMessage = $"Insufficient data for SequenceLength at byte {i}"; return false; }
            int seqLen = data[i] | (data[i + 1] << 8); i += 2;
            int seqEnd = i + seqLen - 2;
            if (seqEnd + 1 >= data.Length) { errorMessage = $"Sequence length beyond data at byte {seqEnd}"; return false; }
            return true;
        }

        protected override bool CheckClearCommand(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i + 1 > data.Length) { errorMessage = $"Insufficient data at byte {i}"; return false; }
            if (data[i] == 0x77) i++;
            if (data[i] != 0x79) { errorMessage = $"Expected 0x79 at byte {i}"; return false; }
            i++;
            if (data[i] != 0x01 && data[i] != 0x02 && data[i] != 0x03) { errorMessage = $"Expected urgentLightMode at byte {i}"; return false; }
            i++;
            if (data[i] != 0x80) { errorMessage = $"Expected urgentTimeMode at byte {i}"; return false; }
            i++;
            if (data[i] != 0xFF) { errorMessage = $"Expected urgentTime at byte {i}"; return false; }
            i++;
            if (i >= data.Length || data[i] != 0x7F) { errorMessage = $"Expected 0x7F at byte {i}"; return false; }
            i++; return true;
        }

        protected override bool CheckMessageType(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            WindowDisplayMode msgType = (WindowDisplayMode)data[i];
            if (!Enum.IsDefined(typeof(WindowDisplayMode), msgType))
            { errorMessage = $"Invalid messageType at byte {i}, received {data[i]:X2}"; return false; }
            return MessageTypeHandlerFactory.GetHandler(msgType).Handle(data, ref i, out errorMessage);
        }
    }
}
