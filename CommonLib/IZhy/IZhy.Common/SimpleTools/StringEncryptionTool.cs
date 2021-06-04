using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 一个简单的字符串加密工具类
    /// </summary>
    public static class StringEncryptionTool
    {
        /// <summary>
        /// 字符对 // 切勿随意更改
        /// <para>策略：该字符串常量的奇数位固定为0-9，顺序可变；偶数位必须是不重复的英文字母</para>
        /// </summary>
        private const string _characterPairs = "0L1y2e3S4s5W6l7Q8b9J";

        /// <summary>
        /// DES 加密的默认秘钥 // 字符对 _characterPairs 的 前 8 位字符
        /// </summary>
        private static readonly string _privateKey;

        /// <summary>
        /// Dictionary 字符配对字典 // 为 字符串简单加密 服务
        /// </summary>
        private static readonly Dictionary<char, char> _dicCharacterPairs;

        static StringEncryptionTool()
        {
            _privateKey = _characterPairs.Substring(0, 8);

            Dictionary<char, char> keyValues = new Dictionary<char, char>();
            char[] vs = _characterPairs.ToCharArray();
            for (int i = 0; i < _characterPairs.Length; i++)
            {
                keyValues.Add(vs[i], vs[++i]);
            }
            _dicCharacterPairs = keyValues;
        }


        #region DES 加/解密

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <param name="privateKey">秘钥，必须是 8 位的字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string plaintext, Encoding encoding = null, string privateKey = null)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                throw new Exception("空的明文，不合法");
            }

            if (privateKey == null)
            {
                privateKey = _privateKey;
            }

            if (privateKey.Length != 8)
            {
                throw new Exception("秘钥不是 8 位的有效字符串，不合法");
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] byKey = encoding.GetBytes(privateKey);
            byte[] inputByteArray = encoding.GetBytes(plaintext);
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        result = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <param name="privateKey">秘钥，必须是 8 位的字符串</param>
        /// <returns></returns>
        public static string DESDecrypt(string ciphertext, Encoding encoding = null, string privateKey = null)
        {
            if (string.IsNullOrWhiteSpace(ciphertext))
            {
                throw new Exception("空的密文，不合法");
            }

            if (privateKey == null)
            {
                privateKey = _privateKey;
            }

            if (privateKey.Length != 8)
            {
                throw new Exception("秘钥不是 8 位的有效字符串，不合法");
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] byKey = encoding.GetBytes(privateKey);
            byte[] inputByteArray = Convert.FromBase64String(ciphertext);
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        result = ms.ToArray();
                    }
                }
            }
            return encoding.GetString(result);
        }

        #endregion


        #region 字符串简单加密

        /// <summary>
        /// 字符串简单加密，与 StrSimpleDecryption() 方法连用
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <returns></returns>
        public static string StrSimpleEncryption(string plaintext, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                throw new Exception("空的明文，不合法");
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            string ciphertext;

            #region 简单加密算法

            char[] plaintextCharArr = plaintext.ToArray(); //转换为 char[]
            StringBuilder ciphertextSb = new StringBuilder(); //密文字符串
            for (int i = 0; i < plaintextCharArr.Length; i++)
            {
                if ((plaintextCharArr.Length + i + 1) > ushort.MaxValue)
                {
                    ushort remainder = (ushort)((plaintextCharArr.Length + i + 1) % ushort.MaxValue);
                    if ((plaintextCharArr[i] + remainder) > ushort.MaxValue)
                    {
                        ciphertextSb.Append(((plaintextCharArr[i] + remainder) - ushort.MaxValue).ToString().PadLeft(5, '0'));
                    }
                    else
                    {
                        ciphertextSb.Append((plaintextCharArr[i] + remainder).ToString().PadLeft(5, '0'));
                    }
                }
                else
                {
                    if ((plaintextCharArr[i] + (plaintextCharArr.Length + i + 1)) > ushort.MaxValue)
                    {
                        ciphertextSb.Append(((plaintextCharArr[i] + (plaintextCharArr.Length + i + 1)) - ushort.MaxValue).ToString().PadLeft(5, '0'));
                    }
                    else
                    {
                        ciphertextSb.Append((plaintextCharArr[i] + (plaintextCharArr.Length + i + 1)).ToString().PadLeft(5, '0'));
                    }
                }
            }

            StringBuilder ciphertextSbResult = new StringBuilder();
            for (int i = 0; i < ciphertextSb.Length; i++)
            {
                ciphertextSbResult.Append((9 - Convert.ToInt32(ciphertextSb[i].ToString())));
            }

            Dictionary<char, char> dic = _dicCharacterPairs;

            for (int i = 0; i < ciphertextSbResult.Length; i++)
            {
                ciphertextSbResult[i] = dic[ciphertextSbResult[i]];
            }

            ciphertext = DESEncrypt(ciphertextSbResult.ToString(), encoding);

            #endregion

            return ciphertext;
        }

        /// <summary>
        /// 字符串简单解密，与 StrSimpleEncryption() 方法连用
        /// </summary>
        /// <param name="ciphertext">密文，必须是通过 StrSimpleEncryption() 方法加密所得到的密文</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <returns></returns>
        public static string StrSimpleDecryption(string ciphertext, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(ciphertext))
            {
                throw new Exception("空的密文，不合法");
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            #region 简单解密算法

            ciphertext = DESDecrypt(ciphertext, encoding);

            StringBuilder ciphertextSb = new StringBuilder();

            Dictionary<char, char> dic = new Dictionary<char, char>();
            foreach (var item in _dicCharacterPairs)
            {
                dic.Add(item.Value, item.Key);
            }

            for (int i = 0; i < ciphertext.Length; i++)
            {
                ciphertextSb.Append(dic[ciphertext[i]]);
            }

            ciphertext = ciphertextSb.ToString();
            ciphertextSb.Clear();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                ciphertextSb.Append((9 - Convert.ToInt32(ciphertext[i].ToString())));
            }

            string[] ciphertextStrArr = StringOperationTool.PerFixedLengthSplit(ciphertextSb.ToString(), 5);
            StringBuilder recoverSb = new StringBuilder(); //复原密文
            for (int i = 0; i < ciphertextStrArr.Length; i++)
            {
                if ((ciphertextStrArr.Length + i + 1) > ushort.MaxValue)
                {
                    ushort remainder = (ushort)((ciphertextStrArr.Length + i + 1) % ushort.MaxValue);
                    if (Convert.ToUInt16(ciphertextStrArr[i]) - remainder < 0)
                    {
                        recoverSb.Append((char)(Convert.ToUInt16(ciphertextStrArr[i]) - remainder + ushort.MaxValue));
                    }
                    else
                    {
                        recoverSb.Append((char)(Convert.ToUInt16(ciphertextStrArr[i]) - remainder));
                    }
                }
                else
                {
                    if (Convert.ToUInt16(ciphertextStrArr[i]) - (ciphertextStrArr.Length + i + 1) < 0)
                    {
                        recoverSb.Append((char)(Convert.ToUInt16(ciphertextStrArr[i]) - (ciphertextStrArr.Length + i + 1) + ushort.MaxValue));
                    }
                    else
                    {
                        recoverSb.Append((char)(Convert.ToUInt16(ciphertextStrArr[i]) - (ciphertextStrArr.Length + i + 1)));
                    }
                }
            }

            #endregion

            return recoverSb.ToString();
        }

        #endregion


        #region MD5 加密

        /// <summary>
        /// MD5 加密，结果是 32 位字符；默认小写字母（ 推荐使用 ）
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="isMustUpper">加密后的结果是否必须为大写的字母；默认 false，小写字母</param>
        /// <returns></returns>
        public static string MD5Encrypt(string plaintext, bool isMustUpper = false)
        {
            string password = string.Empty;
            using (MD5 md5 = MD5.Create()) //实例化一个md5对像
            {
                // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(plaintext));

                // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                for (int i = 0; i < s.Length; i++)
                {
                    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                    password += s[i].ToString("x2");
                }
            }

            if (isMustUpper)
            {
                password = password.ToUpper();
            }
            return password;
        }

        /// <summary>
        /// MD5 Base64 加密，结果是 Base64 形式；默认小写字母
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="isMustUpper">加密后的结果是否必须为大写的字母；默认 false，小写字母</param>
        /// <returns></returns>
        public static string MD5EncryptBase64(string plaintext, bool isMustUpper = false)
        {
            string password = string.Empty;
            using (MD5 md5 = MD5.Create()) //实例化一个md5对像
            {
                // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(plaintext));

                password = Convert.ToBase64String(s);
            }

            if (isMustUpper)
            {
                password = password.ToUpper();
            }
            return password;
        }

        #endregion
    }
}
