using Display;

using System;

using static Display.DisplaySettingsEnums;

namespace UITest.PacketVerifiers
{
    public class LeftPlatformHandlerVerify : PacketVerifyBase
    {
        protected override bool CheckMessageType(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            var cmdType = (DisplaySettingsEnums.CommandType)data[i];
            if (!Enum.IsDefined(typeof(DisplaySettingsEnums.CommandType), cmdType))
            { errorMessage = $"Invalid CommandType at byte {i}, received {data[i]:X2}"; return false; }
            i += 5;
            WindowDisplayMode msgType = (WindowDisplayMode)data[i];
            if (!Enum.IsDefined(typeof(WindowDisplayMode), msgType))
            { errorMessage = $"Invalid messageType at byte {i}, received {data[i]:X2}"; return false; }
            return MessageTypeHandlerFactory.GetHandler(msgType).Handle(data, ref i, out errorMessage);
        }
    }
}
