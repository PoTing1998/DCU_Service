using Display;

using System;

using static Display.DisplaySettingsEnums;

namespace UITest.PacketVerifiers
{
    public class IdentifierStatusImageTopLeft24x48HandlerVerify : PacketVerifyBase
    {
        protected override bool CheckMessageType(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            var cmdType = (DisplaySettingsEnums.CommandType)data[i];
            if (!Enum.IsDefined(typeof(DisplaySettingsEnums.CommandType), cmdType))
            { errorMessage = $"Invalid CommandType at byte {i}, received {data[i]:X2}"; return false; }
            i++;
            if (data[i] != 0x30 && data[i] != 0x31)
            { errorMessage = $"Invalid SwitchMode at byte {i}, received {data[i]:X2}"; return false; }
            i++;
            if (i + 3 > data.Length) { errorMessage = $"Insufficient data for RGB at byte {i}"; return false; }
            i += 3;
            if (data[i] > 13) { errorMessage = $"Invalid PhotoIndex at byte {i}"; return false; }
            i++;
            if (data[i] != 0x30 && data[i] != 0x31)
            { errorMessage = $"Invalid SwitchMode at byte {i}, received {data[i]:X2}"; return false; }
            i++;
            var cmdType2 = (DisplaySettingsEnums.CommandType)data[i];
            if (!Enum.IsDefined(typeof(DisplaySettingsEnums.CommandType), cmdType2))
            { errorMessage = $"Invalid CommandType at byte {i}, received {data[i]:X2}"; return false; }
            i += 6;
            WindowDisplayMode msgType = (WindowDisplayMode)data[i];
            if (!Enum.IsDefined(typeof(WindowDisplayMode), msgType))
            { errorMessage = $"Invalid messageType at byte {i}, received {data[i]:X2}"; return false; }
            return MessageTypeHandlerFactory.GetHandler(msgType).Handle(data, ref i, out errorMessage);
        }
    }
}
