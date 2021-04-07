using System;
using System.Text;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 一个 JavaScript 的工具类
    /// </summary>
    public static class JavaScriptUtil
    {
        /// <summary>
        /// 计算机起始时间 【 1970, 1, 1, 0, 0, 0, 0 】
        /// </summary>
        private static readonly DateTime ComputerStartTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);


        /// <summary>
        /// 获取 js 的时间戳（ 13 位 长度 ）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long JsTimestamp13(this DateTime dateTime)
        {
            long t = (long)(dateTime - ComputerStartTime).TotalMilliseconds;
            return t;
        }

        /// <summary>
        /// 获取 js 的时间戳（ 10 位 长度 ）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long JsTimestamp10(this DateTime dateTime)
        {
            long t = (long)(dateTime - ComputerStartTime).TotalSeconds;
            return t;
        }


        /// <summary>
        /// 对正常的字符串转换为 Unicode 的字符串
        /// </summary>
        /// <param name="normalStr">正常的字符串</param>
        /// <param name="isUpperCaseU">是否大写U字母 ‘\U’；默认 false ‘\u’</param>
        /// <returns></returns>
        public static string ToUnicode(this string normalStr, bool isUpperCaseU = false)
        {
            if (string.IsNullOrWhiteSpace(normalStr))
            {
                return string.Empty;
            }

            StringBuilder strResult = new StringBuilder();
            for (int i = 0; i < normalStr.Length; i++)
            {
                if (isUpperCaseU)
                {
                    strResult.Append("\\U");
                }
                else
                {
                    strResult.Append("\\u");
                }
                strResult.Append(((int)normalStr[i]).ToString("x"));
            }
            return strResult.ToString();
        }

        /// <summary>
        /// 对 Unicode 的字符串解码
        /// </summary>
        /// <param name="unicodeStr">Unicode 字符串</param>
        /// <returns></returns>
        public static string UnicodeDecode(this string unicodeStr)
        {
            if (string.IsNullOrWhiteSpace(unicodeStr) || !unicodeStr.Contains("\\u") || !unicodeStr.Contains("\\U"))
            {
                return unicodeStr;
            }

            StringBuilder strResult = new StringBuilder();
            string[] strArr = unicodeStr.Split('\\');
            for (int i = 0; i < strArr.Length; i++)
            {
                if (strArr[i].Length < 5)
                {
                    continue;
                }
                int code = Convert.ToInt32(strArr[i].Remove(0, 1), 16);
                strResult.Append((char)code);
            }
            return strResult.ToString();
        }
    }
}
