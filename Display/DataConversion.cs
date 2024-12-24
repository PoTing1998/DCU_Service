using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display
{
    static public class DataConversion
    {

        /// <summary>
        /// 倒數用，每五秒為單位轉成byte 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        static public byte[] ConvertToFiveSecondUnitsByteArray(int seconds)
        {
            int unitsOfFiveSeconds = seconds / 5; // 將秒數除以5，以獲得五秒為一個單位的數量   
            string hexadecimalValue = unitsOfFiveSeconds.ToString("X");

            // 將十六進制字符串轉換為 byte[]  
            byte[] byteArray = new byte[1];

            if (byte.TryParse(hexadecimalValue, System.Globalization.NumberStyles.HexNumber, null, out byte result))
            {
                byteArray[0] = result;
            }
            else
            {
                // 轉換失敗時的處理，這裡可以選擇報錯或採取其他操作
                throw new ArgumentException("無法將秒數轉換為十六進制值。");
            }
            return byteArray;
        }
        /// <summary>
        /// 將int轉換為一個byte，僅保留最低8位。
        /// </summary>
        /// <param name="I">要轉換的int值。</param>
        /// <returns>包含一個byte的數組。</returns>
        static public byte[] IntConvertOneByte(int I)
        {
            return new byte[] { (byte)(I & 0xFF) };
        }

        /// <summary>
        /// 轉成RGB使用
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static public byte[] FromRgb(int r, int g, int b)
        {
            byte[] arrReturn = new byte[3];
            arrReturn[0] = Convert.ToByte(r);
            arrReturn[1] = Convert.ToByte(g);
            arrReturn[2] = Convert.ToByte(b);

            return arrReturn;
        }
        /// <summary>
        /// 使用色碼表輸入
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] FromHex(string hex)
        {

            if (string.IsNullOrWhiteSpace(hex) || (hex.Length != 6 && hex.Length != 7))
                throw new ArgumentException("Hex color code must be 6 characters long.");

            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

            return new byte[] { Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b) };
        }
        /// <summary>
        /// 檢查checkSum 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="packetData"></param>
        /// <param name="checkSum"></param>
        /// <returns></returns>
        static public byte[] AppendCheckSum(byte[] packet, byte[] packetData, out byte checkSum)
        {
            ////加總所有的封包內容 
            //checkSum = 0x00;
            //foreach (byte bData in packetData)
            //{
            //    checkSum += bData; 
            //}
            ////將CheckSum加在封包的最後面  
            //System.Array.Resize<byte>(ref packet, packet.Length + 1); 
            //packet[packet.Length - 1] = checkSum;
            //return packet;

            // 使用 LINQ 加總所有的封包內容 
            checkSum = packetData.Aggregate<byte, byte>(0, (current, bData) => (byte)(current + bData));

            // 將 CheckSum 加在封包的最後面
            var packetList = packet.ToList();
            packetList.Add(checkSum);

            return packetList.ToArray();
        }
    }
    #region 偕庭提供function
    class OtherFunction
    {
        #region 斷字測試
        /// <summary>
        /// 抓出一個WORD
        /// </summary>
        struct SeparateString
        {
            string sData;     // 一個WORD
            byte[] bArData;
            int length;     // 一個WORD的長度
            int pixel;      // 一個WORD的PIXEL長度
            public void Set(string str, int length, int pixel)
            {
                this.sData = str;
                bArData = System.Text.Encoding.Default.GetBytes(str);
                this.length = length;
                this.pixel = pixel;
            }
            public void Set(byte[] arByte, int length, int pixel)
            {
                this.sData = null;
                bArData = arByte;
                this.length = length;
                this.pixel = pixel;
            }
            public byte[] GetArByte()
            {
                return bArData;
            }
            public int GetLength()
            {
                return this.length;
            }
            public string GetString()
            {
                return this.sData;
            }
            public int GetPixel()
            {
                return this.pixel;
            }
        }

        /// <summary>
        /// 將一長字串 分為固定長度的短字串陣列 
        /// </summary>
        /// <param name="strData">長字串</param>
        /// <param name="intSep">可顯示之長度 pixel</param>
        /// <param name="fontPixel">一個英文字pixel</param>  
        /// <returns>短字串陣列</returns>
        public string[] SplitX(string strData, int intSep, int fontPixel)
        {
            bool check = false;
            byte bSpace = 0x20;
            byte[] bArData = System.Text.Encoding.Default.GetBytes(strData);
            int iArray = 0;
            int iLoop;
            int iPos = 0;
            int iNext;
            int iEnglishPixel = 0;
            string sStrTrim;
            string sString;
            string sEnglish = "";
            string[] sArResult = new string[1];


            SeparateString oSeparateString;
            SeparateString oSeparateStringBackup = new SeparateString();

            System.Collections.Generic.Queue<SeparateString> oQueue = new System.Collections.Generic.Queue<SeparateString>();

            // 增加中英文混合SHOW出
            // 先將長字串 拆開以一個WORD為單位 用queue存
            for (iLoop = 0; iLoop < bArData.Length; iLoop++)
            {
                // 中文字
                if (bArData[iLoop] >= 128)
                {
                    iNext = 2;
                    iEnglishPixel += iNext * fontPixel;
                    //先儲存前面的英文 如果有
                    if (sEnglish.Length != 0 || iEnglishPixel >= intSep)
                    {
                        oSeparateString = new SeparateString();
                        oSeparateString.Set(sEnglish, sEnglish.Length, sEnglish.Length * fontPixel);
                        oQueue.Enqueue(oSeparateString);
                        sEnglish = "";
                        iEnglishPixel = 0;
                    }
                    oSeparateString = new SeparateString();
                    oSeparateString.Set(System.Text.Encoding.Default.GetString(bArData, iLoop, iNext), iNext, iNext * fontPixel);
                    oQueue.Enqueue(oSeparateString);
                    sEnglish = "";
                    iEnglishPixel = 0;
                    iLoop += 1;
                }
                else
                {
                    //英文字 
                    iNext = 1;
                    iEnglishPixel += iNext * fontPixel;
                    //碰到"空白字元"就儲存
                    if (bArData[iLoop] == bSpace)
                    {
                        //先儲存前面的英文 如果有
                        if (sEnglish.Length != 0 || iEnglishPixel >= intSep)
                        {
                            oSeparateString = new SeparateString();
                            oSeparateString.Set(sEnglish, sEnglish.Length, sEnglish.Length * fontPixel);
                            oQueue.Enqueue(oSeparateString);
                            sEnglish = "";
                            iEnglishPixel = 0;
                        }
                        oSeparateString = new SeparateString();
                        oSeparateString.Set(System.Text.Encoding.Default.GetString(bArData, iLoop, iNext), iNext, iNext * fontPixel);
                        oQueue.Enqueue(oSeparateString);
                        sEnglish = "";
                        iEnglishPixel = 0;
                        //iLoop += 1;
                    }
                    else
                    {
                        //過長英文單字 就直接切
                        if (iEnglishPixel < intSep)
                        {
                            sEnglish += System.Text.Encoding.Default.GetString(bArData, iLoop, iNext);
                        }
                        else
                        {
                            //保留過長word 
                            oSeparateString = new SeparateString();
                            oSeparateString.Set(sEnglish, sEnglish.Length, sEnglish.Length * fontPixel);
                            oQueue.Enqueue(oSeparateString);
                            //保留目前char
                            sEnglish = System.Text.Encoding.Default.GetString(bArData, iLoop, iNext);
                            iEnglishPixel = iNext * fontPixel;
                        }
                    }
                }
                iPos += iNext;
            }
            //儲存英文
            if (sEnglish.Length != 0)
            {
                oSeparateString = new SeparateString();
                oSeparateString.Set(sEnglish, sEnglish.Length, sEnglish.Length * fontPixel);
                oQueue.Enqueue(oSeparateString);
                sEnglish = "";
                iEnglishPixel = 0;
            }

            SeparateString[] temp = oQueue.ToArray();
            string[] temp2 = new string[temp.Length];
            for (int ii = 0; ii < temp2.Length; ii++)
            {
                temp2[ii] = temp[ii].GetString();
            }

            iPos = intSep;
            while (oQueue.Count != 0)// || oSeparateStringBackup.GetString() != "")
            {
                //產生新的字串矩陣
                if (check)
                {
                    check = false;
                    iPos = intSep;
                    Array.Resize<string>(ref sArResult, sArResult.Length + 1);
                    iArray++;
                }
                if (iPos > fontPixel)
                {
                    //檢查保留字串是否為空
                    if (oSeparateStringBackup.GetString() == "" || oSeparateStringBackup.GetString() == null)
                    {
                        if (oQueue.Count > 0)
                        {
                            oSeparateString = oQueue.Dequeue();
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        oSeparateString = oSeparateStringBackup;
                        //清空保留字串
                        oSeparateStringBackup.Set("", 0, 0);
                    }

                    if (iPos >= oSeparateString.GetPixel())
                    {
                        sArResult[iArray] += oSeparateString.GetString();
                        iPos -= oSeparateString.GetPixel();
                    }
                    else
                    {
                        //用空白填補後續
                        while (iPos >= fontPixel)
                        {
                            sArResult[iArray] += " ";
                            iPos -= fontPixel;
                        }

                        //當目前要放入字串的長度過長之時, 要產生新的字串矩陣, 保留字串避免再取queue
                        oSeparateStringBackup = oSeparateString;
                        check = true;
                    }
                }
                else
                {
                    check = true;
                }
            }

            //將最後一個字串加入ARRAY
            if (check)
            {
                check = false;
                iPos = intSep;
                Array.Resize<string>(ref sArResult, sArResult.Length + 1);
                iArray++;
                sArResult[iArray] += oSeparateStringBackup.GetString();
                iPos -= oSeparateStringBackup.GetPixel();
            }

            //用空白填補最後後續 
            iPos -= fontPixel;
            while (iPos >= fontPixel)
            {
                sArResult[sArResult.Length - 1] += " ";
                iPos -= fontPixel;
            }
            //檢查所有ARRAY 將開始的空白移除在後面再加
            for (int ii = 0; ii < sArResult.Length; ii++)
            {
                sString = sArResult[ii];
                if (sString.IndexOf(' ') == 0)
                {
                    sStrTrim = sString.TrimStart(' ');
                    while (sString.Length > sStrTrim.Length)
                    {
                        sStrTrim += " ";
                    }
                    sArResult[ii] = sStrTrim;
                }
            }
            return sArResult;
        }

        /// <summary>
        /// 於看板剩餘部份補上空白
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="intSep"></param>
        /// <param name="fontPixel"></param>
        /// <returns></returns>
        public byte[] AddSpace(byte[] bSourceData, int intSep, int fontPixel)
        {
            bool flag;
            int iCheck;
            int iIndex;
            int iLength;
            byte[] bDestData = null;
            byte[] bDisplay = null;
            try
            {
                bDestData = new byte[0];
                iIndex = 0;
                iLength = (int)(intSep / fontPixel);
                while (iIndex < bSourceData.Length)
                {
                    //設定看板畫面為0x20 = 空白
                    flag = false;
                    bDisplay = new byte[iLength];
                    for (int ii = 0; ii < iLength; ii++)
                    {
                        bDisplay[ii] = 0x20;
                    }

                    for (int ii = 0; ii < iLength; ii++)
                    {
                        //指標已經移到尾巴
                        if (iIndex >= bSourceData.Length)
                        {
                            break;
                        }

                        //中文
                        if (bSourceData[iIndex] > 128)
                        {
                            //放中文字
                            if (ii + 1 < iLength)
                            {
                                flag = true;
                                bDisplay[ii++] = bSourceData[iIndex++];
                                bDisplay[ii] = bSourceData[iIndex++];
                            }
                            else
                            {
                                //換畫面
                                break;
                            }
                        }
                        else if (bSourceData[iIndex] == 0x20)
                        {
                            //空白
                            flag = true;
                            bDisplay[ii] = bSourceData[iIndex++];
                        }
                        else
                        {
                            //英文 要預先check長度
                            iCheck = iIndex;
                            while (iCheck < bSourceData.Length &&
                                bSourceData[iCheck] < 128 &&
                                bSourceData[iCheck] != 0x20 &&
                                (iCheck - iIndex) < (iLength - ii))
                            {
                                iCheck++;
                            }

                            //檢查長度大於看板允許範圍
                            if ((iCheck - iIndex) == (iLength - ii) && ii == 0)
                            {
                                //若在看板起點, 則直接貼上
                                for (int jj = iIndex; jj < iCheck; jj++)
                                {
                                    flag = true;
                                    bDisplay[ii++] = bSourceData[jj];
                                }
                                iIndex = iCheck;
                            }
                            else if ((iCheck - iIndex) < (iLength - ii))
                            {
                                for (int jj = iIndex; jj < iCheck; jj++)
                                {
                                    flag = true;
                                    bDisplay[ii++] = bSourceData[jj];
                                }
                                //多加一, 減回來
                                ii--;
                                iIndex = iCheck;
                            }
                        }
                    }

                    //copy to Dest
                    if (flag)
                    {
                        Array.Resize<byte>(ref bDestData, bDestData.Length + iLength);
                        Array.Copy(bDisplay, 0, bDestData, bDestData.Length - iLength, iLength);
                    }
                    else
                    {
                        //整個畫面都空白
                        break;
                    }
                }

                return bDestData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }

        #endregion
    }
    #endregion
}
