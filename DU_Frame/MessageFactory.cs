using DuFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DuFrame.DisplaySettingsEnums;

namespace DU_Frame
{
 
    public static class MessageFactory
    {
       
        public static DuFrame.MSG.Message CreateMessage(
            DuFrame.MSG.StringMode stringMode,
            DuFrame.DisplaySettingsEnums.WindowDisplayMode mode,
            int size, string type,
            int? startValue = null, int? endValue = null,
            WindowActionCode? windowActionCode = null)
        {
            var photoColor = GetColor("Blue");
            var timeColor = GetColor("Yellow");
            int PaltFormIndex = 1;

            switch (mode)
            {
                case DuFrame.DisplaySettingsEnums.WindowDisplayMode.FullWindow:
                    return HandleFullWindowMode(stringMode, size, type, windowActionCode.Value);
                case DuFrame.DisplaySettingsEnums.WindowDisplayMode.LeftPlatform:
                    return new DuFrame.MSG.Message(stringMode, mode, size, type, photoColor, DataConversion.ByteConverter.IntConvertOneByte(PaltFormIndex));
                case DuFrame.DisplaySettingsEnums.WindowDisplayMode.LeftPlatformRightTime:
                    return new DuFrame.MSG.Message(stringMode, mode, size, type, photoColor, DataConversion.ByteConverter.IntConvertOneByte(PaltFormIndex), timeColor, ConvertTime(startValue.Value), ConvertTime(endValue.Value));
                case DuFrame.DisplaySettingsEnums.WindowDisplayMode.RightTime:
                    return HandleRightSideMode(stringMode, windowActionCode.Value, size, type, startValue.Value, endValue.Value);
                case DuFrame.DisplaySettingsEnums.WindowDisplayMode.TrainDynamic:
                    return new DuFrame.MSG.Message(stringMode, mode, size, type);
                default:
                    return null;
            }
        }
      
        /// <summary>
        /// 將一個整數轉換為一個包含單一字節的字節數組。
        /// 此方法只保留整數的最低8位。
        /// </summary>
        /// <param name="value">要轉換的整數。</param>
        /// <returns>包含從整數最低8位得到的單個字節的字節數組。</returns>
        public static byte[] IntConvertToOneByte(int value)
        {
            // 創建一個新的字節數組，其大小為1。
            byte[] byteArray = new byte[1];
            // 將整數的最低8位（一個字節）存儲到數組的第0個位置。
            byteArray[0] = (byte)(value & 0xFF);
            // 返回包含單個字節的數組。
            return byteArray;
        }
        /// <summary>
        /// 取得色為碼
        /// </summary>
        /// <param name="colorName"></param>
        /// <returns></returns>
        private static byte[] GetColor(string colorName)
        {
            return DataConversion.ByteConverter.FromHex(ASI.Wanda.DCU.DB.Tables.System.sysConfig.PickColor(colorName));
        }
        /// <summary>
        /// 緊急裝置
        /// </summary>
        /// <param name="stringMode"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        private static DuFrame.MSG.Message HandleFullWindowMode(
            DuFrame.MSG.StringMode stringMode, int size, string type, DisplaySettingsEnums.WindowActionCode actionCode)
        {
            if (actionCode == WindowActionCode.EmergencyMessagePlaybackCount)
            {
                var status = GetStatusForEmergencyMessage();
                var emergencyCommand = EmergencyCommand.On;
                return new DuFrame.MSG.Message(stringMode, DuFrame.DisplaySettingsEnums.WindowDisplayMode.FullWindow, emergencyCommand, actionCode, status, size, type);
            }
            else 
            {
                return new DuFrame.MSG.Message(stringMode, DuFrame.DisplaySettingsEnums.WindowDisplayMode.FullWindow, size, type);
            }
        }

        private static DuFrame.MSG.Message HandleRightSideMode(
         DuFrame.MSG.StringMode stringMode, WindowActionCode windowMode, int size, string type, int start, int end)
        {
            var photoColor = GetColor("Blue");
            var timeColor = GetColor("Yellow");
            int PaltFormIndex = 1;
            return new DuFrame.MSG.Message(stringMode, DuFrame.DisplaySettingsEnums.WindowDisplayMode.RightTime, windowMode, true, photoColor, 
                DataConversion.ByteConverter.IntConvertOneByte(PaltFormIndex), timeColor, ConvertTime(start), ConvertTime(end), true, size, type);
        }
        #region private
        private static byte[] GetStatusForEmergencyMessage()
        {
            byte[] status = new byte[] { 0x02 };

            return status;
        }

        /// <summary>
        /// 轉換倒數秒數
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private static byte[] ConvertTime(int seconds)
        {
            return DataConversion.ByteConverter.ConvertToFiveSecondUnitsByteArray(seconds);
        }
        #endregion
    }
}
