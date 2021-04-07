using System;
using System.Text;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 一个简单的字符串操作工具类
    /// </summary>
    public static class StringOperationTool
    {
        /// <summary>
        /// 每固定长度截取字符串转换为字符串数组
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="fixLength">固定长度（不可小于2，小于2无意义）</param>
        /// <returns></returns>
        public static string[] PerFixedLengthSplit(string sourceStr, int fixLength)
        {
            if (string.IsNullOrWhiteSpace(sourceStr) || fixLength < 2)
            {
                return null;
            }
            int length = sourceStr.Length % fixLength == 0 ? sourceStr.Length / fixLength : sourceStr.Length / fixLength + 1;
            string[] strArr = new string[length];
            for (int i = 0; i < strArr.Length; i++)
            {
                if (i == strArr.Length - 1)
                {
                    strArr[i] = sourceStr.Substring(i * fixLength);
                }
                else
                {
                    strArr[i] = sourceStr.Substring(i * fixLength, fixLength);
                }
            }
            return strArr;
        }

        /// <summary>
        /// 用一个字符数组 char[]，从中产生一个指定长度的随机字符串
        /// </summary>
        /// <param name="charArr">char[] 字符数组</param>
        /// <param name="length">指定返回字符串的长度（length 要 大于 0，小于 charArr.Length）</param>
        /// <returns></returns>
        public static string RandomStrFromCharArr(char[] charArr, int length)
        {
            if (charArr == null || charArr.Length < 1)
            {
                return null;
            }
            if (length < 1 || length > charArr.Length)
            {
                return null;
            }

            StringBuilder sb_randomStr = new StringBuilder();

            int lastIndex = -1; //上一个index数
            do
            {
                Random rd = new Random(RandomNumTool.CreateSeed()); //随机数对象
                //int index = rd.Next(0, 10); //产生[0~10]之间的随机数，包含0，不包含10
                int index = rd.Next(0, charArr.Length);
                if (index == lastIndex)
                {
                    continue;
                }
                else
                {
                    if (charArr[index] == ' ' || charArr[index] == '\0')
                    {
                        sb_randomStr.Append('K');
                    }
                    else
                    {
                        sb_randomStr.Append(charArr[index]);
                    }
                    lastIndex = index;
                }
            } while (sb_randomStr.Length < length);

            return sb_randomStr.ToString();
        }

        /// <summary>
        /// 对一个字符串截取指定两个字符之间的内容
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="beginChar">开始字符</param>
        /// <param name="endChar">结束字符</param>
        /// <param name="isTrim">是否去除后结果字符串的前后空格。true:是去除；false:不去除。默认值：false</param>
        /// <returns></returns>
        public static string StrCutOutBetweenTwoChar(string sourceStr, char beginChar, char endChar, bool isTrim = false)
        {
            if (string.IsNullOrWhiteSpace(sourceStr))
            {
                return string.Empty;
            }

            int strLength = sourceStr.Length;
            int index = sourceStr.IndexOf(beginChar); //取得“开始字符”在“源字符串”中的索引位置
            int end = sourceStr.IndexOf(endChar); //取得“结束字符”在“源字符串”中的索引位置

            string newStr;
            int length;
            if (index < 0 && end < 0)
            {
                newStr = sourceStr;
            }
            else if (index < 0 && end >= 0)
            {
                index = 0;
                length = strLength - 1 - end;
                if (length < 1)
                {
                    newStr = string.Empty;
                }
                else
                {
                    newStr = sourceStr.Substring(index, length);
                }
            }
            else if (index >= 0 && end < 0)
            {
                index++; //开始索引值加1，用于忽略截取“开始字符”
                length = strLength - index;
                if (length < 1)
                {
                    newStr = string.Empty;
                }
                else
                {
                    newStr = sourceStr.Substring(index, length);
                }
            }
            else
            {
                index++; //开始索引值加1，用于忽略截取“开始字符”
                length = end - index;
                if (length < 1)
                {
                    newStr = string.Empty;
                }
                else
                {
                    newStr = sourceStr.Substring(index, length);
                }
            }

            if (isTrim)
            {
                newStr = newStr.Trim();
                return newStr;
            }
            else
            {
                return newStr;
            }
        }


        /// <summary>
        /// 字符串脱敏处理；
        /// 把源字符串，从 [index] 索引处开始，向后 [length] 长度，替换为 [newChar] 新字符
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="index">开始处索引</param>
        /// <param name="length">向后替换的长度</param>
        /// <param name="newChar">替换的新字符</param>
        /// <returns></returns>
        public static string StrDesensitization(string sourceStr, int index = 0, int length = 3, char newChar = '*')
        {
            if (string.IsNullOrWhiteSpace(sourceStr))
            {
                return string.Empty;
            }
            char[] sourceStrArr = sourceStr.Trim().ToCharArray();
            for (int i = index; i < length; i++)
            {
                sourceStrArr[i] = newChar;
            }
            return new string(sourceStrArr);
        }
    }
}
