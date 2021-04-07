using System;
using System.Linq;
using System.Security.Cryptography;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 随机编号工具类
    /// </summary>
    public static class RandomNumTool
    {
        /// <summary>
        /// 为 GUID 创建提供加密的强随机数据。
        /// </summary>
        private static readonly RNGCryptoServiceProvider RandomGenerator = new RNGCryptoServiceProvider();


        /// <summary>
        /// 纯字母 字符数组
        /// </summary>
        private static char[] LetterCharArr => new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'O', 'P', 'Q', 'R', 'S', 'T',
            'o', 'p', 'q', 'r', 's', 't',
            'U', 'V', 'W', 'X', 'Y', 'Z',
            'u', 'v', 'w', 'x', 'y', 'z'
        };

        /// <summary>
        /// 纯数字 字符数组
        /// </summary>
        private static char[] NumberCharArr => new char[]
        {
            '1', '2', '3', '4', '5',
            '6', '7', '8', '9', '0'
        };

        /// <summary>
        /// 字母 + 数字 字符数组
        /// </summary>
        private static char[] LetterAndNumberCharArr => new char[]
        {
            '0', 'A', 'B', 'C', 'D', 'E', 'F', 'G',
            '1', 'a', 'b', 'c', 'd', 'e', 'f', 'g',
            '2', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
            '3', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            '4', '8', 'O', 'P', 'Q', 'R', 'S', 'T',
            '5', '9', 'o', 'p', 'q', 'r', 's', 't',
            '6', '0', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '7', '1', 'u', 'v', 'w', 'x', 'y', 'z',
            '2', '3', '4', '5', '6', '7', '8', '9'
        };



        /// <summary>
        /// 产生种子
        /// </summary>
        /// <returns></returns>
        public static int CreateSeed()
        {
            string newTime = DateTime.Now.Ticks.ToString();
            char[] newTime_arr = newTime.ToCharArray();
            char[] newTime_arr_orderBy = newTime_arr.OrderBy(x => Guid.NewGuid()).ToArray();
            string randomSeed = string.Empty;
            for (int i = 1; i <= 6; i++)
            {
                randomSeed += newTime_arr_orderBy[i];
            }
            return Convert.ToInt32(randomSeed);
        }


        /// <summary>
        /// 获取随机编号，时间 + 数字 // 固定 32 位长度
        /// </summary>
        /// <returns></returns>
        public static string NOTimePlusNumber()
        {
            string strNo = $"{DateTime.Now:yyyyMMddHHmmssfffffff}{NOOnlyNumber(11)}";
            return strNo;
        }

        /// <summary>
        /// 获取随机编号，时间 + 字母 // 固定 32 位长度
        /// </summary>
        /// <param name="isMustUpper">返回的结果是否必须要大写，默认 true</param>
        /// <returns></returns>
        public static string NOTimePlusLetter(bool isMustUpper = true)
        {
            string strNo = $"{DateTime.Now:yyyyMMddHHmmssfffffff}{NOOnlyLetter(11, isMustUpper)}";
            return strNo;
        }

        /// <summary>
        /// 获取随机编号，时间 + 字母和数字组合 // 固定 32 位长度
        /// </summary>
        /// <param name="isMustUpper">返回的结果是否必须要大写，默认 true</param>
        /// <returns></returns>
        public static string NOTimePlusLetterAndNumber(bool isMustUpper = true)
        {
            string strNo = $"{DateTime.Now:yyyyMMddHHmmssfffffff}{NOLetterAndNumber(11, isMustUpper)}";
            return strNo;
        }


        /// <summary>
        /// 获取随机编号（纯数字，默认 6 个数字）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NOOnlyNumber(int length = 6)
        {
            int arrLength = NumberCharArr.Length;
            if (length > arrLength)
            {
                int multiple = (length % arrLength) > 0 ? (length / arrLength) + 1 : length / arrLength;
                var arr = new char[arrLength * multiple];
                int index = 0;
                for (int i = 0; i < multiple; i++)
                {
                    foreach (var item in NumberCharArr)
                    {
                        arr[index] = item;
                        index++;
                    }
                }
                return StringOperationTool.RandomStrFromCharArr(arr, length);
            }
            else
                return StringOperationTool.RandomStrFromCharArr(NumberCharArr, length);
        }

        /// <summary>
        /// 获取随机编号（纯字母，默认 6 个字符）
        /// </summary>
        /// <param name="length"></param>
        /// <param name="isMustUpper">返回的结果是否必须为大写，默认 false</param>
        /// <returns></returns>
        public static string NOOnlyLetter(int length = 6, bool isMustUpper = false)
        {
            int arrLength = LetterCharArr.Length;
            string strNo;
            if (length > arrLength)
            {
                int multiple = (length % arrLength) > 0 ? (length / arrLength) + 1 : length / arrLength;
                var arr = new char[arrLength * multiple];
                int index = 0;
                for (int i = 0; i < multiple; i++)
                {
                    foreach (var item in LetterCharArr)
                    {
                        arr[index] = item;
                        index++;
                    }
                }
                strNo = StringOperationTool.RandomStrFromCharArr(arr, length);
            }
            else
            {
                strNo = StringOperationTool.RandomStrFromCharArr(LetterCharArr, length);
            }

            if (isMustUpper)
            {
                strNo = strNo.ToUpper();
            }
            return strNo;
        }

        /// <summary>
        /// 获取随机编号（字母和数字组合，默认 6 个字符）
        /// </summary>
        /// <param name="length"></param>
        /// <param name="isMustUpper">返回的结果是否必须为大写，默认 false</param>
        /// <returns></returns>
        public static string NOLetterAndNumber(int length = 6, bool isMustUpper = false)
        {
            int arrLength = LetterAndNumberCharArr.Length;
            string strNo;
            if (length > arrLength)
            {
                int multiple = (length % arrLength) > 0 ? (length / arrLength) + 1 : length / arrLength;
                var arr = new char[arrLength * multiple];
                int index = 0;
                for (int i = 0; i < multiple; i++)
                {
                    foreach (var item in LetterAndNumberCharArr)
                    {
                        arr[index] = item;
                        index++;
                    }
                }
                strNo = StringOperationTool.RandomStrFromCharArr(arr, length);
            }
            else
            {
                strNo = StringOperationTool.RandomStrFromCharArr(LetterAndNumberCharArr, length);
            }

            if (isMustUpper)
            {
                strNo = strNo.ToUpper();
            }
            return strNo;
        }


        /// <summary>
        /// 创建 有序的 GUID
        /// </summary>
        /// <param name="isMustUpper">返回的结果是否必须为大写，默认 true，大写的</param>
        /// <param name="guidType">
        /// <para>有序的 GUID 类型，枚举值</para>
        /// <para>默认值：ESequentialGuidType.SequentialAsString</para>
        /// </param>
        /// <param name="toStringFormat">
        /// <para>GUID ToString() 方法的格式化字符串，一般常用 "N"、"D"；默认值 "N"</para>
        /// <para>"N" 形如：00000000000000000000000000000000 32位数字</para>
        /// <para>"D" 形如：00000000-0000-0000-0000-000000000000 由连字符分隔的32位数字</para>
        /// </param>
        /// <returns></returns>
        public static string CreateSequentialGuid(bool isMustUpper = true,
                                                  ESequentialGuidType guidType = ESequentialGuidType.SequentialAsString,
                                                  string toStringFormat = "N")
        {
            // 从16字节加密的强随机数据开始
            byte[] randomBytes = new byte[10];
            RandomGenerator.GetBytes(randomBytes);

            // 另一种方法:使用常规创建的GUID来获取初始值
            // 随机数据:
            // byte[] randomBytes = Guid.NewGuid().ToByteArray();
            // 这比使用RNGCryptoServiceProvider要快，但是我没有
            // 推荐它，因为。net框架不保证
            // GUID数据的随机性，以及未来版本(或不同版本)
            // 像Mono这样的实现可能会使用不同的方法。

            long timestamp = DateTime.UtcNow.Ticks / 10000L;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            // 因为我们是从Int64转换过来的，所以我们必须打开
            // 低位优先的系统。
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            byte[] guidBytes = new byte[16];

            switch (guidType)
            {
                default:
                case ESequentialGuidType.SequentialAsString:
                case ESequentialGuidType.SequentialAsBinary:
                    // 对于字符串和字节数组版本，我们首先复制时间戳，然后复制
                    // 随机数据。
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    // 如果格式化为字符串，我们必须对事实进行补偿
                    // .NET将Data1和Data2块视为Int32和Int16，
                    // 分别。这意味着它在little - endian上切换了顺序
                    // 系统。所以，我们必须再次逆转。
                    if (guidType == ESequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }
                    break;

                case ESequentialGuidType.SequentialAtEnd:
                    // 对于末尾排序的版本，我们首先复制随机数据，
                    // 然后是时间戳。
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }

            string strNo = new Guid(guidBytes).ToString(toStringFormat);
            if (isMustUpper)
            {
                strNo = strNo.ToUpper();
            }
            return strNo;
        }
    }

    /// <summary>
    /// 枚举类 有序的 GUID 类型
    /// </summary>
    public enum ESequentialGuidType
    {
        /// <summary>
        /// 按照字符串顺序排列；适用于 MySQL、PostgreSQL 数据库
        /// </summary>
        SequentialAsString = 1,

        /// <summary>
        /// 按照二进制的顺序排列；适用于 Oracle 数据库
        /// </summary>
        SequentialAsBinary,

        /// <summary>
        /// 按照末尾部分排列；适用于 MS SQL Server 数据库
        /// </summary>
        SequentialAtEnd
    }
}
