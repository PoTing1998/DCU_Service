using Display;

using System;

using static Display.DisplaySettingsEnums;

namespace UITest.Verify
{
    /// <summary>
    /// 緊急訊息版型：
    /// - FunctionCode = 0x38（而非 0x34）
    /// - CheckSequenceLength 有額外的啟動/顯示代碼前置
    /// - CheckClearCommand 包含緊急代碼 0x79 + 燈號/次數/播放模式
    /// - CheckMessageType 直接為 WindowDisplayMode
    /// </summary>
    public class UrgentHandlerVerify : PacketVerifyBase
    {
        protected override bool CheckFunctionCode(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (data[i] != 0x38)
            {
                errorMessage = $"FunctionCode mismatch at byte {i}, expected 0x38";
                return false;
            }
            i++;
            return true;
        }

        protected override bool CheckSequenceLength(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";

            // 信號啟動代碼
            if (data[i] != 0x01 && data[i] != 0x02)
            {
                errorMessage = $"Expected 0x01 or 0x02 at byte {i} for signal activation";
                return false;
            }
            i++;

            // 顯示模式代碼
            if (data[i] != 0x01 && data[i] != 0x02)
            {
                errorMessage = $"Expected 0x01 or 0x02 at byte {i} for display mode";
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

        protected override bool CheckClearCommand(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            if (i + 1 > data.Length)
            {
                errorMessage = $"Insufficient data for Clear Command check at byte {i}";
                return false;
            }

            if (data[i] == 0x77) i++;   // 可選

            // 緊急代碼
            if (data[i] != 0x79)
            {
                errorMessage = $"Expected urgent Command [optional 0x77, 0x79] at byte {i}";
                return false;
            }
            i++;

            // 燈號模式 (0x01~0x03)
            if (data[i] != 0x01 && data[i] != 0x02 && data[i] != 0x03)
            {
                errorMessage = $"Expected urgentLightMode at byte {i}";
                return false;
            }
            i++;

            // 啟動次數
            if (data[i] != 0x80)
            {
                errorMessage = $"Expected urgentTimeMode at byte {i}";
                return false;
            }
            i++;

            // 播放模式（無限為 0xFF）
            if (data[i] != 0xFF)
            {
                errorMessage = $"Expected urgentTime at byte {i}";
                return false;
            }
            i++;

            // 結尾 0x7F
            if (i >= data.Length || data[i] != 0x7F)
            {
                errorMessage = $"Expected 0x7F at byte {i}";
                return false;
            }
            i++;
            return true;
        }

        protected override bool CheckMessageType(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
            WindowDisplayMode msgType = (WindowDisplayMode)data[i];
            if (!Enum.IsDefined(typeof(WindowDisplayMode), msgType))
            {
                errorMessage = $"Invalid messageType at byte {i}, received {data[i]:X2}";
                return false;
            }
            return MessageTypeHandlerFactory.GetHandler(msgType).Handle(data, ref i, out errorMessage);
        }
    }
}
