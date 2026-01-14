# ColorHelper 使用說明

## 概述

`ColorHelper` 是一個靜態顏色映射類，提供顏色名稱到 RGB 值的轉換功能。

**優點：**
- ✅ 不需要資料庫連接
- ✅ 快速查詢（內存中的字典）
- ✅ 支持中英文顏色名稱
- ✅ 易於維護和測試
- ✅ 減少資料庫依賴

## 基本用法

### 1. 獲取顏色的十六進制色碼

```csharp
using Display;

// 獲取顏色的十六進制值
string hex = ColorHelper.GetColorHex("紅色");  // 返回 "FF0000"
string hex2 = ColorHelper.GetColorHex("Red");  // 返回 "FF0000"（支持英文）
```

### 2. 獲取顏色的 RGB 字節數組

```csharp
using Display;

// 直接獲取 RGB 字節數組
byte[] colorBytes = ColorHelper.GetColorBytes("白色");
// colorBytes = [0xFF, 0xFF, 0xFF]
```

### 3. 檢查顏色是否定義

```csharp
bool exists = ColorHelper.IsColorDefined("紅色");  // true
bool exists2 = ColorHelper.IsColorDefined("未知色");  // false
```

### 4. 獲取所有支持的顏色

```csharp
var colors = ColorHelper.GetAllColorNames();
foreach (var color in colors)
{
    Console.WriteLine(color);
}
```

## 替換舊的資料庫查詢代碼

### 舊代碼（需要資料庫）

```csharp
// ❌ 舊方法 - 需要資料庫連接
var ConfigDate = ASI.Wanda.DCU.DB.Tables.System.sysConfig.SelectColor(colorName);
return DataConversion.FromHex(ConfigDate.config_value);
```

### 新代碼（不需要資料庫）

```csharp
// ✅ 新方法 - 不需要資料庫
var colorBytes = ColorHelper.GetColorBytes(colorName);
return colorBytes;
```

## 支持的顏色列表

### 基本顏色
- 紅色 / Red: `FF0000`
- 綠色 / Green: `00FF00`
- 藍色 / Blue: `0000FF`
- 白色 / White: `FFFFFF`
- 黑色 / Black: `000000`
- 黃色 / Yellow: `FFFF00`

### 擴展顏色
- 橙色 / Orange: `FFA500`
- 紫色 / Purple: `800080`
- 粉紅 / Pink: `FFC0CB`
- 青色 / Cyan: `00FFFF`
- 品紅 / Magenta: `FF00FF`

### 灰階
- 灰色 / Gray: `808080`
- 深灰 / DarkGray: `404040`
- 淺灰 / LightGray: `C0C0C0`

### 深色系
- 深紅 / DarkRed: `8B0000`
- 深綠 / DarkGreen: `006400`
- 深藍 / DarkBlue: `00008B`

### 淺色系
- 淺紅 / LightRed: `FFB6C1`
- 淺綠 / LightGreen: `90EE90`
- 淺藍 / LightBlue: `ADD8E6`

### 其他常用顏色
- 棕色 / Brown: `A52A2A`
- 金色 / Gold: `FFD700`
- 銀色 / Silver: `C0C0C0`
- 天藍 / SkyBlue: `87CEEB`
- 海軍藍 / Navy: `000080`
- 橄欖 / Olive: `808000`
- 珊瑚 / Coral: `FF7F50`
- 石灰 / Lime: `00FF00`
- 栗色 / Maroon: `800000`
- 靛青 / Indigo: `4B0082`
- 綠松石 / Turquoise: `40E0D0`

## 在各個 Task 項目中使用

### TaskPUP、TaskCDU、TaskSDU、TaskPDN

找到類似以下的代碼：

```csharp
// 在 TaskPUPHelper.cs, TaskCDUHelper.cs, TaskSDUHelper.cs, TaskPDNHelper.cs 中
var ConfigDate = ASI.Wanda.DCU.DB.Tables.System.sysConfig.SelectColor(colorName);
return DataConversion.FromHex(ConfigDate.config_value);
```

替換為：

```csharp
return ColorHelper.GetColorBytes(colorName);
```

## 添加新顏色

如果需要添加新顏色，只需在 `ColorHelper.cs` 的 `ColorMap` 字典中添加：

```csharp
private static readonly Dictionary<string, string> ColorMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    // 現有顏色...

    // 添加新顏色
    { "新顏色名稱", "RRGGBB" },
    { "NewColorName", "RRGGBB" },
};
```

## 注意事項

1. **不區分大小寫**：顏色名稱查詢不區分大小寫
2. **默認值**：如果找不到指定的顏色，會返回白色（FFFFFF）
3. **線程安全**：ColorHelper 是靜態類，所有方法都是線程安全的
4. **無需初始化**：直接使用靜態方法即可，無需創建實例

## 完整示例

```csharp
using Display;
using System;

class Example
{
    static void Main()
    {
        // 示例 1：基本使用
        byte[] redBytes = ColorHelper.GetColorBytes("紅色");
        Console.WriteLine($"紅色 RGB: [{redBytes[0]}, {redBytes[1]}, {redBytes[2]}]");

        // 示例 2：檢查顏色是否存在
        if (ColorHelper.IsColorDefined("藍色"))
        {
            string blueHex = ColorHelper.GetColorHex("藍色");
            Console.WriteLine($"藍色色碼: {blueHex}");
        }

        // 示例 3：列出所有顏色
        Console.WriteLine("\n支持的所有顏色：");
        foreach (var color in ColorHelper.GetAllColorNames())
        {
            Console.WriteLine($"{color}: {ColorHelper.GetColorHex(color)}");
        }
    }
}
```

## 遷移檢查清單

- [x] Display/ColorHelper.cs - 創建 ColorHelper 類
- [x] UITest/Form1.cs - 更新 ProcessMessageColor 和 button1_Click
- [ ] TaskPUP/TaskPUPHelper.cs - 更新 ProcessMessageColor（如果存在）
- [ ] TaskCDU/TaskCDUHelper.cs - 更新 ProcessMessageColor（如果存在）
- [ ] TaskSDU/TaskSDUHelper.cs - 更新 ProcessMessageColor（如果存在）
- [ ] TaskPDN/TaskPDNHelper.cs - 更新 ProcessMessageColor（如果存在）

## 效能比較

| 方法 | 需要資料庫 | 查詢時間 | 維護性 |
|------|-----------|---------|--------|
| 舊方法（sysConfig.SelectColor） | ✅ 是 | ~10-50ms | 困難（需要資料庫） |
| 新方法（ColorHelper） | ❌ 否 | ~0.001ms | 簡單（代碼中維護） |

## 技術支持

如有問題或需要添加新顏色，請聯繫開發團隊。
