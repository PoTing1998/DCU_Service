using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display
{
    public class DisplaySettingsEnums
    {
        /// <summary>
        /// 取的設備值
        /// </summary>
        public enum Device
        {
            state = 0x31,
            white_balance_value = 0x32,
            Read_time_value_MCU = 0x33
        }
        /// <summary>
        /// 71H, SPDU/SCDU 視窗格式 1顯示(全視窗)，一般訊息設定
        /// 72H, SPDU/SCDU 視窗格式 2顯示(左側月台碼)，一般訊息設定。 
        /// 73H, SPDU/SCDU 視窗格式 3顯示(左側月台碼，右側時間顯示)，一般訊息設定。 
        /// 74H, SPDU/SCDU 視窗格式 4顯示(右側時間顯示)，一般訊息效果顯示。 
        /// 75H, SPDU/SCDU 單次訊息顯示，視窗格式依照上一次設定顯示，但無週期。
        /// 76H, 保留，暫時無定義。 
        /// 83h,列車動態
        /// </summary>
        /// <summary>
        /// Window display modes for SPDU/SCDU messages.
        /// </summary>
        public enum WindowDisplayMode
        {
            FullWindow = 0x71,
            LeftPlatform = 0x72,
            LeftPlatformRightTime = 0x73,
            RightTime = 0x74,
            SingleMessage = 0x75,
            Reserved = 0x76,
            TrainDynamic = 0x83,
        }
        public enum MessageLevel : byte
        {
            Level1 = 0x01, // 最高级别
            Level2 = 0x02, // 高级别
            Level3 = 0x03, // 低级别
            Level4 = 0x04  // 最低级别
        }


        public enum StringMode
        {
            TextStatic  = 0X2A,
            TextFlash = 0x2B ,
            Pre_RecordedMessage =0X2C,
            Pre_RecordedPicturesStatic_Dynamic =0x2D
        }

        /// <summary>
        ///  77H, ClearScreen 清除畫面（blanking）， 用於立即更新訊息使用。 
        ///  78H, PauseTime 停留時間
        ///  79H, SPDU 警示燈動作設定，
        ///  7AH, SPDU/SCDU 月台碼圖片訊息顯示
        ///  7BH, SPDU/SCDU 分割視窗時間訊息設定，
        ///  7CH,視窗與中英文的切換
        ///  7DH, SCDU線別代碼圖片顯示在上排左側 24x48 區塊中
        ///  7EH, SCDU 標準時間顯示在下排左側 24x48 區塊中
        ///  7FH, 字型大小和字體種類 
        ///  80H,緊急訊息播放次數
        ///  82H,視窗格式 6 顯示(右側顯示 48x48 時間區域，有類比時鐘和數位時間顯示方式)
        /// </summary>
        public enum WindowActionCode
        {
            ClearScreen = 0x77,
            PauseTime = 0x78,
            WarningLightAction = 0x79,
            PlatformPictureMessageDisplay = 0x7A,
            SplitWindowTimeMessageSetting = 0x7B,
            LanguageSwitch = 0x7C,
            PictureOnLeft = 0x7D,
            PictureDownLeft = 0x7E,
            TextSettings = 0x7F,
            EmergencyMessagePlaybackCount = 0x80,
            ClockDisplay = 0x82,
            Default = 0x00
        }

        public enum ScrollMode
        {
            Jump = 0x61,                   // 即時顯示
            ScrollLeft = 0x62,             // 向左捲動（右至左，常规滚动）
            ScrollLeftWithDisappear = 0x63, // 向左捲動（消失，向左滚动消失）
            ScrollLeftWithFadeOut = 0x64,  // 向左捲動（逐渐消失，右至左逐渐淡出）
            ScrollDown = 0x65,             // 向下捲動
            ScrollUp = 0x66,               // 向上捲動
            Flash = 0x67                   // 閃爍
        }
    }
   
}
