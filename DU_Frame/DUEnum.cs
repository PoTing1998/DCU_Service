using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuFrame
{
   public class DisplaySettingsEnums
    {
        /// <summary>
        /// 取的設備值
        /// </summary>
        public enum Device
        {
            state=0x31,
            white_balance_value=0x32,
            Read_time_value_MCU= 0x33
        }
        /// <summary>
        ///  通訊測試 
        ///  參數設定&顯示模式切換 
        ///  開關機 
        ///  旅客訊息的設定 
        ///  預錄資料庫 
        ///  字型資料庫更新 
        ///  設備通訊更新 
        ///  緊急訊息播放
        /// </summary>
        public enum Function
        {
            CommunTest = 0x31,
            DisplaySettings = 0x32,
            switchOn = 0x33,
            PassengerInformation = 0x34,
            preRecordedDdatabase = 0x35,
            Text = 0x36,
            equipment = 0x37,
            emergencyMessage = 0x38
        }
        /// <summary>
        ///參數設定&顯示模式切換 32h
        ///設定 Command Code清單
        ///顯示器顯示亮度 
        ///顯示模式  (重新開電 MCU reset後，首先播放畫面設定) 
        ///通訊逾時的時間設定
        ///顯示器最大亮度限制 (白色顏色調整)
        ///萬年曆 IC的設定
        /// </summary>
        public enum SetCommand
        {
            light = 0x41,
            showMethod =0x42,
            CommonTime=0x43,
            lightLimit =0x44,
            calender =0x45,
            countdown = 0x46
        }

        /// <summary>
        /// 01H：正常畫面播放。 
        /// 02H：測試畫面。
        /// 03H：顯示 ID碼與 FW版本。 
        /// 04H：顯示通訊逾時的畫面
        /// </summary>
        public enum showMethod
        {
            normal =0x01,
            test=0x02,
            showIDandFW =0x03,
            showOverTime = 0x04,
        }
         public enum timeValue
        {
            seccend_one = 0x01,
            seccend_five = 0x05,
            seccend_ten = 0x0A,
            minute = 0x3C
        }
       
        /// <summary>
        /// 文字靜態顯示模式
        /// 文字閃爍顯示模式
        /// 顯示預錄訊息的內容
        /// 顯示預錄圖片靜動態的內容
        /// </summary>
        public enum StringMode
        {
            text= 0x2A,
            textFlash = 0x2B,
            recordedMassage=0x2C,
            recordedPicture =0x2D
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

        /// <summary>
        /// 顯示 ScrollMode
        /// 61H Jump 立即顯示 
        /// 62H  ScrollLeft 向左捲動(靠右對齊)
        /// 63H  ScrollLeft 向左捲動(靠左對齊)
        /// 64H ScrollLeft 向左捲動(左移消失)
        /// 65H ScrollDown 向下捲動
        /// 66H ScrollUp 向上捲動
        /// 67H 閃爍
        /// </summary>
        public enum ScrollMode
        {
            jump = 0x61,
            Scroll_right = 0x62,
            Scroll_left =0x63,
            Scroll_Disappearance =0x64,
            scroll_down = 0x65,
            scroll_up = 0x66,
            flash = 0x67,
        }
        /// <summary>
        /// 開啟選擇時鐘
        /// 類比時鐘
        /// 數位時鐘
        /// </summary>
        public enum clock
        {
            none = 0x00,
            analogClock = 0x31,
            digitalClock = 0x32
        }
        /// <summary>
        /// 字體大小
        ///16
        ///24
        ///英文
        /// </summary>
        public enum fontSize
        {
            Font16x16 = 0x21,
            Font24x24 = 0x22,
            Font5x7 = 0x23,
        }
        /// <summary>
        /// 字體
        /// </summary>
        public enum fontStyle 
        {
            明體 = 0x31,
            黑體 = 0x32,
            楷體 = 0x33,
        }
        /// <summary>
        /// 緊急訊息插播功能 38H 
        /// 緊急訊息插播功能 command
        /// </summary>
        public enum EmergencyCommand
        {
            On =0x01,
            Off =0x02,
        }


    }
}





