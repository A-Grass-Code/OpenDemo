using System;
using System.Collections.Generic;
using System.Text;

namespace IZhy.Common.DbTools
{
    /// <summary>
    /// sql 字符串 扩展方法 工具类
    /// </summary>
    public static class SqlStringExtension
    {
        public static string SqlWhere(this string sql, bool isJoin)
        {
            return isJoin ? sql : string.Empty;
        }
    }
}
