using System.Xml.Serialization;

namespace UITest.Controls
{
    /// <summary>
    /// DisplayMessageControl 的可持久化設定。
    /// 以 XmlSerializer 存成 display_settings.xml，不需額外 NuGet 套件。
    /// </summary>
    [XmlRoot("DisplayMessageSettings")]
    public class DisplayMessageSettings
    {
        // ── 上行板型 & 訊息頻型 ──────────────────────────────────
        public int  UpBoardIndex    { get; set; } = 0;   // rdoUpBoard1~8 → 0-based
        public int  UpMsgTypeIndex  { get; set; } = 0;   // 0=一般, 1=預錄

        // ── 下行板型 & 訊息頻型 ──────────────────────────────────
        public int  DnBoardIndex    { get; set; } = 0;
        public int  DnMsgTypeIndex  { get; set; } = 0;

        // ── 訊息文字 ─────────────────────────────────────────────
        public string UpMsg { get; set; } = "";
        public string DnMsg { get; set; } = "";

        // ── 字型參數（ComboBox index）───────────────────────────
        public int UpFontSizeIndex  { get; set; } = 0;
        public int UpFontStyleIndex { get; set; } = 0;
        public int UpColorIndex     { get; set; } = 3;   // clYellow
        public int UpLevelIndex     { get; set; } = 3;   // 最低Level 4

        public int DnFontSizeIndex  { get; set; } = 0;
        public int DnFontStyleIndex { get; set; } = 0;
        public int DnColorIndex     { get; set; } = 3;
        public int DnLevelIndex     { get; set; } = 3;

        // ── 動作方式（0x61~0x67）────────────────────────────────
        public byte UpScrollAction  { get; set; } = 0x61;
        public byte DnScrollAction  { get; set; } = 0x62;

        // ── 動作參數 ─────────────────────────────────────────────
        public int UpSpeed  { get; set; } = 5;
        public int UpPause  { get; set; } = 10;
        public int DnSpeed  { get; set; } = 5;
        public int DnPause  { get; set; } = 8;

        // ── Extra：時間子選項（右側）──────────────────────────────
        public int  UpTimeTypeIndex  { get; set; } = 0;
        public int  UpTimeClrIndex   { get; set; } = 0;
        public bool UpTimeOn         { get; set; } = true;
        public int  DnTimeTypeIndex  { get; set; } = 0;
        public int  DnTimeClrIndex   { get; set; } = 0;
        public bool DnTimeOn         { get; set; } = true;

        // ── Extra：月台碼子選項 ───────────────────────────────────
        public int UpPlatIdx        { get; set; } = 1;
        public int UpPlatClrIndex   { get; set; } = 0;
        public int DnPlatIdx        { get; set; } = 1;
        public int DnPlatClrIndex   { get; set; } = 0;

        // ── Extra：板型7 路線碼（上行）────────────────────────────
        public int  UpRouteIdx      { get; set; } = 1;
        public int  UpRouteClrIndex { get; set; } = 3;   // clYellow
        public bool UpRouteOn       { get; set; } = true;

        // ── Extra：板型7 左側時間（下行）──────────────────────────
        public int  DnTimeLeftTypeIndex { get; set; } = 0;
        public int  DnTimeLeftClrIndex  { get; set; } = 3;   // clYellow
        public bool DnTimeLeftOn        { get; set; } = true;
    }
}
