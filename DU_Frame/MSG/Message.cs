
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using ASI.Wanda.DMD;


using DuFrame.MSG;

using static DuFrame.DUEnum;

namespace DuFrame.MSG
{

    public class Message 
    {

        public StringMode mstringMode;

        #region constractor
        /// <summary>
        /// 文字模式 or 列車動態訊息
        /// </summary>
        public Message(
            StringMode container
           , WindowDisplayMode version
           , DuFrame.DUEnum.fontSize fontSize
           , DuFrame.DUEnum.fontStyle fontStyle)
        {
            mstringMode = container;
           
            Version = version;
            FontSize = fontSize;
            FontStyle = fontStyle;
        }
        /// <summary>
        /// 左側月台顯示
        /// </summary>
 
        public Message(
           StringMode container
          , WindowDisplayMode version
          , DuFrame.DUEnum.fontSize fontSize
          , DuFrame.DUEnum.fontStyle fontStyle
          , byte[] PhotoColor
          , byte[] index)
        {
            mstringMode = container;
            Version = version;
            FontSize = fontSize;
            FontStyle = fontStyle;
            Platformphoto = PhotoColor;
            Platformphotoindex = index;
        }
        /// <summary>
        /// 左側月台+右側時間
        /// </summary>
        public Message(
           StringMode container
          , WindowDisplayMode version
          , DuFrame.DUEnum.fontSize fontSize
          , DuFrame.DUEnum.fontStyle fontStyle
          , byte[] PhotoColor
          , byte[] index
          , byte[] time_color
          , byte[] startValue
          , byte[] endValue)
        {
            mstringMode = container;
            Version = version;
            FontSize = fontSize;
            FontStyle = fontStyle;
            Platformphoto = PhotoColor;
            Platformphotoindex = index;
            TimeColor = time_color;
            TimeStart = startValue;
            TimeEnd = endValue;
        }
        /// <summary>
        /// 右側時間模式  
        /// </summary>
        public Message(
          StringMode container
         , WindowDisplayMode version
         , DuFrame.DUEnum.fontSize fontSize
         , DuFrame.DUEnum.fontStyle fontStyle
         , byte[] time_color
         , byte[] startValue
         , byte[] endValue)
        {
            mstringMode = container;
            Version = version;
            FontSize = fontSize;
            FontStyle = fontStyle;
            TimeColor = time_color;
            TimeStart = startValue;
            TimeEnd = endValue;
        }
        /// <summary>
        /// 數位時鐘
        /// </summary>
        public Message(
         StringMode container
         , WindowDisplayMode version
         , DuFrame.DUEnum.fontSize fontSize
         , DuFrame.DUEnum.fontStyle fontStyle
         , byte[] clockColor
         , DuFrame.DUEnum.clock clock)
        {
            mstringMode = container;
            Version = version;
          
            FontSize = fontSize;
            FontStyle = fontStyle;
            ClockColor = clockColor;
            Clock = clock;
        }
        public Message(
        StringMode container
        , WindowDisplayMode version
        , DuFrame.DUEnum.fontSize fontSize
        , DuFrame.DUEnum.fontStyle fontStyle
        , byte[] clockColor
        , byte[] photographDataContent
        , DuFrame.DUEnum.clock clock)
        {
            mstringMode = container;
            Version = version;
            PhotographDataContent = photographDataContent;
            FontSize = fontSize;
            FontStyle = fontStyle;
            ClockColor = clockColor;
            Clock = clock;
        }

        /// <summary>
        /// 特殊模式兩排一起送
        /// </summary>
        public Message(
            StringMode container
            , WindowDisplayMode version
            , WindowActionCode topCode
            , bool top
            , byte[] PhotoColor
            , byte []  index
            , byte[] time_color
            , byte[] startValue
            , byte[] endValue
            , WindowActionCode bottomCode
            , bool bottom
            , byte [] LeftclockColor
            , byte [] RightclockColor
            , byte [] RightStartValue
            , byte [] RightEndValue
            , DuFrame.DUEnum.fontSize fontSize
            , DuFrame.DUEnum.fontStyle fontStyle
            )
        {
            mstringMode = container;
            Version = version;
            TopCommandType = topCode;
            TopPlatformSwitch = top;
            TopPlatformColor = PhotoColor;
            TopPlatformIndex = index;
            TopClockColor = time_color;
            TopClockStartTime = startValue;
            TopClockEndTime = endValue;
            BottomCommandType = bottomCode;
            BottomClockSwitch = bottom;
            BottomLeftClockColor = LeftclockColor;
            BottomRightClockColor= RightclockColor;
            BottomRightClockStart = RightStartValue;
            BottomRightClockEnd = RightEndValue;
            FontSize = fontSize;
            FontStyle = fontStyle;
        }
        //連通道上排
        public Message(StringMode container, WindowDisplayMode version , WindowActionCode topCode , bool top ,byte[] PhotoColor , byte[] index , byte[] time_color, byte[]startValue, byte[] endValue , bool ModeSwitch,DUEnum.fontSize fontSize , DUEnum.fontStyle fontStyle)
        {
            mstringMode = container;
            Version = version;
            TopCommandType = topCode;
            TopPlatformSwitch = top;
            TopPlatformColor = PhotoColor;
            TopPlatformIndex = index;
            TopClockColor = time_color;
            TopClockStartTime = startValue;
            TopClockEndTime = endValue;
            TopClockSwitch = ModeSwitch;
            FontSize = fontSize;
            FontStyle = fontStyle;
  
        }
        //連通道下排
        public Message(StringMode container, WindowDisplayMode version, WindowActionCode bottomCode, bool bottom , byte[] LeftclockColor, byte[] RightclockColor, byte[] RightStartValue, byte[]RightEndValue
            ,bool ModeSwitch , DUEnum.fontSize fontSize , DUEnum.fontStyle fontStyle)
        {
            mstringMode = container;
            Version = version;
            BottomCommandType = bottomCode;
            BottomClockSwitch= bottom;
            BottomLeftClockColor = LeftclockColor;
            BottomRightClockColor = RightclockColor;
            BottomRightClockStart = RightStartValue;
            BottomRightClockEnd = RightEndValue;
            BottomRightTimeSwitch = ModeSwitch;
            FontSize = fontSize;
            FontStyle = fontStyle;

        }
       
        /// <summary>
        /// 緊急播放模式
        /// </summary>
        public Message(
          StringMode container
         , WindowDisplayMode version
         , EmergencyCommand emergencyCommand 
         , WindowActionCode emergency
         , byte[] emergencyLampStatus
         , DuFrame.DUEnum.fontSize fontSize
         , DuFrame.DUEnum.fontStyle fontStyle)
        {
            mstringMode = container;
            emergencyMode = emergencyCommand;
            EmergencyLampStatus = emergencyLampStatus;
            Emergency=emergency;
            Version = version;
            FontSize = fontSize;
            FontStyle = fontStyle;
        }
        #endregion

        #region  property

        // 消息長度
        public fontSize FontSize { get; set; }
        public fontStyle FontStyle { get; set; }

        // 視窗版本和時間
        public DuFrame.DUEnum.WindowDisplayMode Version { get; set; }
        public byte[] TimeColor { get; set; }
        public byte[] TimeStart { get; set; }
        public byte[] TimeEnd { get; set; } = new byte[] { 0x00 };


        //月台的圖像顏色跟索引值
        public byte[] Platformphoto { get; set; }
        public byte[] Platformphotoindex { get; set; }
        // 鬧鐘命令和設置
        public DuFrame.DUEnum.clock Clock { get; set; }
        public byte[] ClockColor { get; set; }

        // 圖片長度和內容
        public byte[] PhotographLength { get; set; }
        public byte[] PhotographDataContent { get; set; }
        public byte[] PhotographDataContentUp { get; set; }
        public byte[] PhotographDataContentDown { get; set; }


        //連通道用的版型
        public WindowActionCode TopCommandType { get; set; }
        public bool TopPlatformSwitch { get; set; }
        public byte[] TopPlatformColor { get; set; }
        public byte[] TopPlatformIndex { get; set; }       
        public byte[] TopClockColor { get; set; }
        public byte[] TopClockStartTime { get; set; }
        public byte[] TopClockEndTime { get; set; } = new byte[] { 0x00 };
        public bool TopClockSwitch { get; set; }

        public WindowActionCode BottomCommandType { get; set; }
        public bool BottomClockSwitch { get; set; }
        public byte[] BottomLeftClockColor { get; set; }
        public byte[] BottomRightClockColor { get; set; }
        public byte[] BottomRightClockStart { get; set; }
        public byte[] BottomRightClockEnd { get; set; } = new byte[] { 0x00 };
        public bool BottomRightTimeSwitch { get; set; }


        // 緊急播放次數
        public DUEnum.EmergencyCommand emergencyMode { get; set; }
        public DuFrame.DUEnum.WindowActionCode Emergency { get; set; }
        public byte[] EmergencyLampStatus { get; set; }
        public byte[] EmergencyTimes { get; set; } = new byte[] { 0x00 };

        protected readonly byte[] MessageEnd = new byte[] { 0x1E };

        #endregion
    }
}