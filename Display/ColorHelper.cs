using System;
using System.Collections.Generic;

namespace Display
{
    /// <summary>
    /// 靜態顏色映射類，提供顏色名稱到 RGB 值的轉換
    /// 不需要資料庫連接，所有顏色定義都在代碼中
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// 顏色名稱到十六進制色碼的映射表
        /// </summary>
        private static readonly Dictionary<string, string> ColorMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // 基本顏色
            { "紅色", "FF0000" },
            { "Red", "FF0000" },
            { "綠色", "00FF00" },
            { "Green", "00FF00" },
            { "藍色", "0000FF" },
            { "Blue", "0000FF" },
            { "白色", "FFFFFF" },
            { "White", "FFFFFF" },
            { "黑色", "000000" },
            { "Black", "000000" },
            { "黃色", "FFFF00" },
            { "Yellow", "FFFF00" },

            // 擴展顏色
            { "橙色", "FFA500" },
            { "Orange", "FFA500" },
            { "紫色", "800080" },
            { "Purple", "800080" },
            { "粉紅", "FFC0CB" },
            { "Pink", "FFC0CB" },
            { "青色", "00FFFF" },
            { "Cyan", "00FFFF" },
            { "品紅", "FF00FF" },
            { "Magenta", "FF00FF" },

            // 灰階
            { "灰色", "808080" },
            { "Gray", "808080" },
            { "Grey", "808080" },
            { "深灰", "404040" },
            { "DarkGray", "404040" },
            { "淺灰", "C0C0C0" },
            { "LightGray", "C0C0C0" },

            // 深色系
            { "深紅", "8B0000" },
            { "DarkRed", "8B0000" },
            { "深綠", "006400" },
            { "DarkGreen", "006400" },
            { "深藍", "00008B" },
            { "DarkBlue", "00008B" },

            // 淺色系
            { "淺紅", "FFB6C1" },
            { "LightRed", "FFB6C1" },
            { "淺綠", "90EE90" },
            { "LightGreen", "90EE90" },
            { "淺藍", "ADD8E6" },
            { "LightBlue", "ADD8E6" },

            // 其他常用顏色
            { "棕色", "A52A2A" },
            { "Brown", "A52A2A" },
            { "金色", "FFD700" },
            { "Gold", "FFD700" },
            { "銀色", "C0C0C0" },
            { "Silver", "C0C0C0" },
            { "天藍", "87CEEB" },
            { "SkyBlue", "87CEEB" },
            { "海軍藍", "000080" },
            { "Navy", "000080" },
            { "橄欖", "808000" },
            { "Olive", "808000" },
            { "紫紅", "C71585" },
            { "MediumVioletRed", "C71585" },
            { "珊瑚", "FF7F50" },
            { "Coral", "FF7F50" },
            { "石灰", "00FF00" },
            { "Lime", "00FF00" },
            { "栗色", "800000" },
            { "Maroon", "800000" },
            { "靛青", "4B0082" },
            { "Indigo", "4B0082" },
            { "綠松石", "40E0D0" },
            { "Turquoise", "40E0D0" }
        };

        /// <summary>
        /// 根據顏色名稱獲取十六進制色碼
        /// </summary>
        /// <param name="colorName">顏色名稱（中文或英文）</param>
        /// <returns>十六進制色碼（例如：FF0000），找不到則返回白色（FFFFFF）</returns>
        public static string GetColorHex(string colorName)
        {
            if (string.IsNullOrWhiteSpace(colorName))
            {
                return "FFFFFF"; // 默認白色
            }

            // 嘗試從映射表中獲取顏色
            if (ColorMap.TryGetValue(colorName.Trim(), out string hex))
            {
                return hex;
            }

            // 如果找不到，返回默認白色
            return "FFFFFF";
        }

        /// <summary>
        /// 根據顏色名稱獲取 RGB 字節數組
        /// </summary>
        /// <param name="colorName">顏色名稱（中文或英文）</param>
        /// <returns>包含 R、G、B 三個字節的數組</returns>
        public static byte[] GetColorBytes(string colorName)
        {
            string hex = GetColorHex(colorName);
            return DataConversion.FromHex(hex);
        }

        /// <summary>
        /// 檢查顏色名稱是否存在於映射表中
        /// </summary>
        /// <param name="colorName">顏色名稱</param>
        /// <returns>true 如果顏色存在，否則 false</returns>
        public static bool IsColorDefined(string colorName)
        {
            if (string.IsNullOrWhiteSpace(colorName))
            {
                return false;
            }

            return ColorMap.ContainsKey(colorName.Trim());
        }

        /// <summary>
        /// 獲取所有支持的顏色名稱列表
        /// </summary>
        /// <returns>顏色名稱的集合</returns>
        public static IEnumerable<string> GetAllColorNames()
        {
            return ColorMap.Keys;
        }

        /// <summary>
        /// 直接從十六進制色碼獲取 RGB 字節數組
        /// 用於向後兼容，如果用戶已經有十六進制色碼
        /// </summary>
        /// <param name="hexColor">十六進制色碼（可選 # 前綴）</param>
        /// <returns>包含 R、G、B 三個字節的數組</returns>
        public static byte[] FromHex(string hexColor)
        {
            return DataConversion.FromHex(hexColor);
        }

        /// <summary>
        /// 從 RGB 值創建字節數組
        /// </summary>
        /// <param name="r">紅色分量 (0-255)</param>
        /// <param name="g">綠色分量 (0-255)</param>
        /// <param name="b">藍色分量 (0-255)</param>
        /// <returns>包含 R、G、B 三個字節的數組</returns>
        public static byte[] FromRgb(int r, int g, int b)
        {
            return DataConversion.FromRgb(r, g, b);
        }
    }
}
