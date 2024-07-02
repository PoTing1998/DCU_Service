using System.Collections.Generic;

namespace DuFrame.MSG
{

    public  class StringMode 
    {
        #region property
        protected readonly byte[] StringEnd= new byte[] { 0x1F };

        public byte[] StringColor { get; set; }
        public List<string> contentText { get; set; }
        public List<string> BottomRowMessags { get; set; }
        public string ScrollMode { get; set; }
        public byte[] ScrollSpeed { get; set; }
        public byte[] ScrollPauseTime { get; set; }
        public int Level { get; set; }

        public byte[] IndexNumber { get; set; }
        public byte[] TextColor { get; set; }
        public int GraphicStartIndex { get; set; }
        public int GraphicStartIndex2 { get; set; }
        public byte[] GraphicNumber { get; set; }

        public byte[] GraphicNumber2 { get; set; }
        public byte[] GraphicColor { get; set; }
        public string FirstStation { get; set; }
        public string FirstStationMode { get; set; }
        public string SecondStation { get; set; }
        public string SecondStationMode { get; set; }
        public string LocalStation { get; set; }
        public string LocalStationMode { get; set; }

        #endregion
        #region Constructors
        /// <summary>
        /// 文字靜態封包格式  
        /// </summary>
        public StringMode(
            int level,
            string scrollMode,
            byte[] scrollSpeed,
            byte[] scrollPauseTime,
            byte[] stringColor,
            List<string> topRowMessages
            )
        {
            Level = level;
            ScrollMode = scrollMode;
            ScrollPauseTime = scrollPauseTime;
            ScrollSpeed = scrollSpeed;
            StringColor = stringColor;
            contentText = topRowMessages;
          
        }
        /// <summary>
        /// 文字靜態封包格式 兩排一起傳送的 
        /// </summary>
        public StringMode(
            int level,
            string scrollMode,
            byte[] scrollSpeed,
            byte[] scrollPauseTime,
            byte[] stringColor,
            List<string> topRowMessages,
            List<string> bottomRowMessags
            )
        {
            Level = level;
            ScrollMode = scrollMode;
            ScrollPauseTime = scrollPauseTime;
            ScrollSpeed = scrollSpeed;
            StringColor = stringColor;
            contentText = topRowMessages;
            BottomRowMessags = bottomRowMessags;

        }
        /// <summary>
        /// 顯示預錄訊息的內容 
        /// </summary>
        public StringMode(
            int level,
            string scrollMode,
            byte[] scrollSpeed,
            byte[] scrollPauseTime,
            byte[] IndexNumber,
            byte[] TextColor
            )
        {
            Level = level;
            ScrollMode = scrollMode;
            ScrollPauseTime = scrollPauseTime;
            ScrollSpeed = scrollSpeed;
            this.IndexNumber = IndexNumber;
            this.TextColor = TextColor;
        }
        /// <summary>
        /// 列車動態模式封包格式
        /// </summary> 

        public StringMode(
            int level,
            string scrollMode,
            byte[] scrollSpeed,
            byte[] scrollPauseTime,
            byte[] stringColor,
            int graphicStartIndex,
            byte[] graphicNumber,
            int graphicStartIndex2,
            byte[] graphicNumber2,
            byte[] graphicColor,
            string firstStation = "",
            string firstStationMode="",
            string secondStation = "",
            string secondStationMode="",
            string thirdStation = "",
            string  thirdStationMode = "" )
        {
            Level = level;
            ScrollMode = scrollMode;
            ScrollPauseTime = scrollPauseTime;
            ScrollSpeed = scrollSpeed;
            StringColor = stringColor;
            FirstStation = firstStation;
            FirstStationMode = firstStationMode;
            SecondStation = secondStation;
            SecondStationMode = secondStationMode;
            LocalStation = thirdStation;
            LocalStationMode = thirdStationMode;
            GraphicStartIndex =graphicStartIndex;
            GraphicNumber = graphicNumber;
            GraphicStartIndex2 = graphicStartIndex2;
            GraphicNumber2 = graphicNumber2;
            GraphicColor= graphicColor;
        }

        #endregion
    }
}
