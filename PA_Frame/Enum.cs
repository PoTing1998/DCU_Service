using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.PA
{

    /// <summary>
    /// 定義列舉值轉換至資料庫的值
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DbValueAttribute : Attribute
    {
        public DbValueAttribute(string value)
        {
            Value = value;
        }
        /// <summary>
        /// 讀寫 Database 存入值
        /// </summary>
        public string Value { get; set; }
    }
    public static class EnumExtensions
    {
        public static object ToDbValue(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attr = field.GetCustomAttribute<DbValueAttribute>();
                if (attr != null)
                {
                    return attr.Value;
                }
            }
            return Convert.ToInt32(value);
        }

      


       
      
        /// <summary>
        /// 取得 從 Database 之值轉換成指定列舉
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">字串值</param>
        /// <returns>T.</returns>
        public static T? FromDbValue<T>(this string value) where T : struct, IConvertible
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                var enumType = typeof(T);
                var underlyingType = Nullable.GetUnderlyingType(enumType);
                if (underlyingType != null && underlyingType.IsEnum)
                {
                    enumType = underlyingType;
                }

                var enumValuesWithAttributes = from T enumValue in Enum.GetValues(enumType)
                                               let enumMember = enumType.GetMember(enumValue.ToString())[0]
                                               let dbValueAttribute = enumMember.GetCustomAttribute<DbValueAttribute>()
                                               select new
                                               {
                                                   EnumValue = enumValue,
                                                   DbValueAttribute = dbValueAttribute
                                               };

                var matchingEnumValue = enumValuesWithAttributes
                    .FirstOrDefault(item => (item.DbValueAttribute != null ? item.DbValueAttribute.Value : item.EnumValue.ToString()) == value);

                return matchingEnumValue?.EnumValue;
            }
        }
    }
    /// <summary>
    /// 火災探測器動作人員確認中請勿驚慌
    /// 發生火災請依照指引鎮定逃生離開
    /// 火災警報已解除
    /// 經確認先前火災探測器動作已解除請安心 
    /// </summary>
    public enum fireContent : byte
    {
        confirming = 0x81,
        exit = 0x82,
        remove = 0x83,
        reassure = 0x84
    }
    public class PA_Enum
    {
        #region DMD TO PA 
        /// <summary>
        /// 車站名稱代碼  
        /// </summary>
        public enum station : byte
        {
            本站 = 0x30,
            LG01 = 0x31,
            LG02 = 0x32,
            LG03 = 0x33,
            LG04 = 0x34,
            LG05 = 0x35,
            LG06 = 0x36,
            LG07 = 0x37,
            LG08 = 0x38,
            LG08A = 0x39,
            LG09 = 0x40,
            LG010 = 0x41,
            LG11 = 0x42,
            LG12 = 0x43,
            LG13 = 0x44,
            LG14 = 0x45,
            LG15 = 0x46,
            LG16 = 0x47,
            LG17 = 0x48,
            LG18 = 0x49,
            LG19 = 0x4A,
            LG20 = 0x4B,
            LG21 = 0x4C,

        }
        /// <summary>
        /// 月台代碼
        /// 正常營運-全部月台
        /// 正常營運-一月台
        /// 正常營運-二月台
        /// 降級營運-全部月台
        /// 降級營運-一月台
        /// 降級營運-二月台
        /// </summary>
        public enum platform : byte
        {
            正常營運_全部月台 = 0xC0,
            正常營運_一月台 = 0xC1,
            正常營運_二月台 = 0xC2,
            降級營運_全部月台 = 0xE0,
            降級營運_一月台 = 0xE1,
            降級營運_二月台 = 0xC2,
        }
        /// <summary> 
        /// 列車狀況代碼
        /// 列車將進站
        /// 列車停靠
        /// 列車將離站
        /// 末班車將進站
        /// 末班車停靠
        /// 末班車將離站
        /// 本月台已無列車
        /// 臨時停車
        /// 不提供載客服務
        /// 因列車調度，本車不停靠
        /// </summary>
        public enum situation : byte
        {
            列車將進站 = 0xF0,
            列車停靠 = 0xF1,
            列車將離站 = 0xF2,
            末班車將進站 = 0xF3,
            末班車停靠 = 0xF4,
            末班車將離站 = 0xF5,
            本月台已無列車 = 0xF6,
            臨時停車 = 0xF7,
            不提供載客服務 = 0xF8,
            本車不停靠 = 0xF9,
        }
        #endregion
        #region PA TO DMD
        /// <summary>
        /// 訊息代碼表
        /// </summary>
        public enum floor : byte
        {
            underground_one = 0x01,
            underground_two = 0x02,
            underground_three = 0x03,
            underground_four = 0x04,
            underground_five = 0x05,
            underground_six = 0x06,
            underground_seven = 0x07,
            underground_eight = 0x08,
            underground_nine = 0x09,
            underground_ten = 0x0A,
            floor_one = 0x0B,
            floor_two = 0x0C,
            floor_three = 0x0D,
            floor_four = 0x0E,
            floor_five = 0x0F,
            floor_six = 0x11,
            floor_seven = 0x12,
            floor_eight = 0x13,
            floor_nine = 0x14,
            floor_ten = 0x15,
            lobby = 0x16,
            platform_one = 0x17,
            platform_two = 0x18,
            equipment = 0x19

        }


        public static floor GetFloorFromValue(byte floorValue)
        {
            if (Enum.IsDefined(typeof(floor), floorValue))
            {
                return (floor)floorValue;
            }
            else
            {
                return floor.underground_one; 
            }
        }
        /// <summary>
        /// 火災訊息
        /// 火災探測器動作人員確認中請勿驚慌
        /// 發生火災請依照指引鎮定逃生離開
        /// 火災警報已解除
        /// 經確認先前火災探測器動作已解除請安心
        /// </summary>
        public enum fireMsg : byte
        {
            confirming = 0x81,
            exit = 0x82,
            remove = 0x83,
            reassure = 0x84
        }
        public static readonly Dictionary<floor, string> floorMap = new Dictionary<floor, string>
        {
             { floor.underground_one, "目前所在為地下1樓" },
             { floor.underground_two, "目前所在為地下2樓" },
             { floor.underground_three, "目前所在為地下3樓" },
             { floor.underground_four, "目前所在為地下4樓" },
             { floor.underground_five, "目前所在為地下5樓" },
             { floor.underground_six, "目前所在為地下6樓" },
             { floor.underground_seven, "目前所在為地下7樓" },
             { floor.underground_eight, "目前所在為地下8樓" },
             { floor.underground_nine, "目前所在為地下9樓" },
             { floor.underground_ten, "目前所在為地下10樓" },
             { floor.floor_one, "目前所在為1樓" },
             { floor.floor_two, "目前所在為2樓" },
             { floor.floor_three, "目前所在為3樓" },
             { floor.floor_four, "目前所在為4樓" },
             { floor.floor_five, "目前所在為5樓" },
             { floor.floor_six, "目前所在為6樓" },
             { floor.floor_seven, "目前所在為7樓" },
             { floor.floor_eight, "目前所在為8樓" },
             { floor.floor_nine, "目前所在為9樓" },
             { floor.floor_ten, "目前所在為10樓" },
             { floor.lobby, "目前所在大廳" },
             { floor.platform_one, "目前所在一月台" },
             { floor.platform_two, "目前所在二月台" },
             { floor.equipment, "目前所在設備層" }
        };
        // 儲存 Enum 成員和對應屬性值的字典
        public static readonly Dictionary<fireContent, string> InitialFireContentValues = new Dictionary<fireContent, string>
        {
            { fireContent.confirming, "" },
            { fireContent.exit,       "" },
            { fireContent.remove ,    "" },
            { fireContent.reassure ,  "" }
        };
        public static void InsertValueIntoDbAttribute(fireContent enumValue, string newValue)
        {
            // 如果 DbValues 中包含 Enum 成員，則更新對應的屬性值 
            if (InitialFireContentValues.ContainsKey(enumValue))
            {
                InitialFireContentValues[enumValue] = newValue;
            }
        }
        public static fireContent GetFirContentFromValue(byte floorValue)
        {
            if (Enum.IsDefined(typeof(fireContent), floorValue))
            {
                return (fireContent)floorValue;
            }
            else
            {
                return fireContent.confirming; 
            }
        }
        public static string GetDbValue(fireContent enumValue)
        {
            // 從 DbValues 中獲取對應的屬性值
            return InitialFireContentValues.TryGetValue(enumValue, out var value) ? value : enumValue.ToString();
        }
        #endregion
    }


}
