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
        /// <summary>
        /// 用于 sql 字符串 条件拼接
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="isJoin"></param>
        /// <returns></returns>
        public static string SqlWhere(this string sql, bool isJoin)
        {
            return isJoin ? sql : string.Empty;
        }

        /// <summary>
        /// 用于 sql 字符串 条件拼接
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="isJoin"></param>
        /// <param name="trueExe">成功拼接后需要执行的东西</param>
        /// <returns></returns>
        public static string SqlWhere(this string sql, bool isJoin, Action trueExe)
        {
            if (isJoin)
            {
                trueExe();
            }
            return isJoin ? sql : string.Empty;
        }
    }
}
