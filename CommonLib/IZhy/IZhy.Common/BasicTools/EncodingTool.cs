using System;
using System.Text;

namespace IZhy.Common.BasicTools
{
    /// <summary>
    /// Encoding 工具类
    /// </summary>
    public static class EncodingTool
    {
        static EncodingTool()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Encoding 初始化
        /// </summary>
        public static void EncodingInit() { }

        /// <summary>
        /// 获取一个指定的字符编码
        /// </summary>
        /// <param name="name">编码名称；例如 GBK 、 GB2312</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string name)
        {
            try
            {
                return Encoding.GetEncoding(name);
            }
            catch (Exception)
            {
                return Encoding.Default;
            }
        }
    }
}
