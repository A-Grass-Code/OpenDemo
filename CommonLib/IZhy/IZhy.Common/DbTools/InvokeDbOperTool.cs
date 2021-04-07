using IZhy.Common.AopProxy;
using IZhy.Common.BasicTools;
using IZhy.Common.Const;
using System;

namespace IZhy.Common.DbTools
{
    /// <summary>
    /// 调用数据库操作工具类
    /// </summary>
    public static class InvokeDbOperTool
    {
        /// <summary>
        /// 是否启用数据库连接；若为 false 则不再执行与数据库的操作，适用于不依赖数据库的项目
        /// </summary>
        public static bool IsEnableDbConn()
        {
            try
            {
                return Convert.ToBoolean(CommonConfigTool.GetConfig(CommonConfigFieldsConst.IsEnableDbConn) ?? false);
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 获取一个 DbOperationTool 的新实例
        /// <para>属性 ConnUniqueAlias 连接信息的唯一别名是默认值 : "default"</para>
        /// </summary>
        /// <returns></returns>
        public static IDbOperation DbOperTool()
        {
            if (!IsEnableDbConn())
            {
                return null;
            }
            var db = GeneralDecorator.CreateDecorator<IDbOperation, DbOperationTool, DbOperToolDecorator>(new DbOperationTool());
            db.OpenConn();
            return db;
        }

        /// <summary>
        /// 获取一个 DbOperationTool 类的新实例
        /// <para>并设置 属性 ConnUniqueAlias 连接信息的唯一别名</para>
        /// </summary>
        /// <param name="connUniqueAlias">设置 属性 ConnUniqueAlias 连接信息的唯一别名</param>
        /// <returns></returns>
        public static IDbOperation DbOperTool(string connUniqueAlias)
        {
            if (!IsEnableDbConn())
            {
                return null;
            }
            var db = GeneralDecorator.CreateDecorator<IDbOperation, DbOperationTool, DbOperToolDecorator>(new DbOperationTool(connUniqueAlias));
            db.OpenConn();
            return db;
        }
    }
}
