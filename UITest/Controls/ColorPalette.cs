using System;
using System.Drawing;

namespace UITest.Controls
{
    /// <summary>
    /// 顏色 ComboBox 共用的顏色定義。
    /// 集中管理，讓所有需要顏色下拉的控制項共享同一份資料。
    /// </summary>
    public static class ColorPalette
    {
        /// <summary>顏色定義（名稱 + Color 值），共 30 色。</summary>
        public static readonly (string Name, Color Value)[] Entries =
        {
            ("clBlack",       Color.Black),
            ("clWhite",       Color.White),
            ("clRed",         Color.FromArgb(0xFF, 0x00, 0x00)),
            ("clYellow",      Color.FromArgb(0xFF, 0xFF, 0x00)),
            ("clGreen",       Color.FromArgb(0x00, 0xFF, 0x00)),
            ("clBlue",        Color.FromArgb(0x00, 0x00, 0xFF)),
            ("clCyan",        Color.FromArgb(0x00, 0xFF, 0xFF)),
            ("clMagenta",     Color.FromArgb(0xFF, 0x00, 0xFF)),
            ("clOrange",      Color.FromArgb(0xFF, 0xA5, 0x00)),
            ("clGold",        Color.FromArgb(0xFF, 0xD7, 0x00)),
            ("clPink",        Color.FromArgb(0xFF, 0xC0, 0xCB)),
            ("clDeepPink",    Color.FromArgb(0xFF, 0x14, 0x93)),
            ("clCoral",       Color.FromArgb(0xFF, 0x7F, 0x50)),
            ("clSalmon",      Color.FromArgb(0xFA, 0x80, 0x72)),
            ("clTomato",      Color.FromArgb(0xFF, 0x63, 0x47)),
            ("clOrangeRed",   Color.FromArgb(0xFF, 0x45, 0x00)),
            ("clLime",        Color.FromArgb(0x32, 0xCD, 0x32)),
            ("clSpringGreen", Color.FromArgb(0x00, 0xFF, 0x7F)),
            ("clTurquoise",   Color.FromArgb(0x40, 0xE0, 0xD0)),
            ("clSkyBlue",     Color.FromArgb(0x87, 0xCE, 0xEB)),
            ("clDodgerBlue",  Color.FromArgb(0x1E, 0x90, 0xFF)),
            ("clNavy",        Color.FromArgb(0x00, 0x00, 0x80)),
            ("clTeal",        Color.FromArgb(0x00, 0x80, 0x80)),
            ("clPurple",      Color.FromArgb(0x80, 0x00, 0x80)),
            ("clViolet",      Color.FromArgb(0xEE, 0x82, 0xEE)),
            ("clIndigo",      Color.FromArgb(0x4B, 0x00, 0x82)),
            ("clMaroon",      Color.FromArgb(0x80, 0x00, 0x00)),
            ("clOlive",       Color.FromArgb(0x80, 0x80, 0x00)),
            ("clGray",        Color.FromArgb(0x80, 0x80, 0x80)),
            ("clSilver",      Color.FromArgb(0xC0, 0xC0, 0xC0)),
        };

        /// <summary>ComboBox.Items.AddRange 用的 object[]，只建立一次。</summary>
        public static readonly object[] ComboItems =
            Array.ConvertAll(Entries, e => (object)e.Name);

        /// <summary>依名稱取得 Color，找不到回傳 White。</summary>
        public static Color GetColor(string name)
        {
            if (name == null) return Color.White;
            foreach (var e in Entries)
                if (e.Name == name) return e.Value;
            return Color.White;
        }

        /// <summary>依名稱取得 6 位大寫 Hex 字串（不含 #）。</summary>
        public static string GetHex(string name)
        {
            Color c = GetColor(name);
            return $"{c.R:X2}{c.G:X2}{c.B:X2}";
        }
    }
}
