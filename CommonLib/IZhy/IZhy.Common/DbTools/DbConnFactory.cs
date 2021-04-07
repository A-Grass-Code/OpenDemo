using IZhy.Common.BasicTools;
using IZhy.Common.Const;
using IZhy.Common.SimpleTools;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace IZhy.Common.DbTools
{
    /// <summary>
    /// 数据库连接工厂
    /// </summary>
    public static class DbConnFactory
    {
        /// <summary>
        /// 默认的 DB连接配置 的 唯一别名
        /// </summary>
        public const string DefaultConnUniqueAlias = "default";

        /// <summary>
        /// 获取或设置 数据库连接信息 配置文件 的 绝对路径
        /// </summary>
        public static string DbConnConfigPath { get; set; } = $"{FilesTool.ProgramRootDirectoryCommonConfig}{FileNameConst.DbConnInfoJson}";

        /// <summary>
        /// 用来存放 DbConnInfo.json 配置文件 的 最后修改时间
        /// </summary>
        public static DateTime DbConnInfoLastChangeTime { get; private set; }

        /// <summary>
        /// 获取一个 bool 值，表示 DbConnInfo.json 配置文件 是否已被重新修改；true/false 是/否
        /// </summary>
        public static bool IsChangeConfigFile()
        {
            if (!File.Exists(DbConnConfigPath))
            {
                throw new Exception($"【DbConnFactory】 数据库连接信息的配置文件不存在，请检查 => [ {DbConnConfigPath} ]");
            }

            try
            {
                FileInfo fileInfo = new FileInfo(DbConnConfigPath);
                if (fileInfo.LastWriteTime != DbConnInfoLastChangeTime)
                {
                    DbConnInfoLastChangeTime = fileInfo.LastWriteTime;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogsTool.WriteEXLog("【DbConnFactory】 获取一个 bool 值，表示 DbConnInfo.json 配置文件 是否已被重新修改，发生异常", ex.ToString());
                return true;
            }
            return false;
        }


        private static Dictionary<string, DbConnInfo> _dbConnInfo = new Dictionary<string, DbConnInfo>();

        /// <summary>
        /// 获取 DbConnInfo.json 配置文件里的数据库连接信息
        /// </summary>
        public static Dictionary<string, DbConnInfo> GetDbConnInfo()
        {
            try
            {
                if (_dbConnInfo == null || _dbConnInfo.Count < 1 || IsChangeConfigFile())
                {
                    _dbConnInfo = JsonTool.JsonFileToObject<Dictionary<string, DbConnInfo>>(DbConnConfigPath);
                }
            }
            catch (Exception ex)
            {
                LogsTool.WriteEXLog("【DbConnFactory】 获取 DbConnInfo.json 配置文件里的配置信息，发生异常", ex);
                _dbConnInfo = new Dictionary<string, DbConnInfo>();
            }
            return _dbConnInfo;
        }


        /// <summary>
        /// 依据 唯一别名 获取 数据库连接对象
        /// <para>默认值 : "default"</para>
        /// </summary>
        /// <param name="uniqueAlias">连接配置的唯一别名</param>
        /// <returns></returns>
        internal static DbConnection GetDbConnection(string uniqueAlias = null)
        {
            if (string.IsNullOrWhiteSpace(uniqueAlias))
            {
                uniqueAlias = DefaultConnUniqueAlias;
            }

            Dictionary<string, DbConnInfo> dicConn = GetDbConnInfo();

            if (dicConn.ContainsKey(uniqueAlias))
            {
                var dbConnInfo = dicConn[uniqueAlias];

                switch (dbConnInfo.DbType)
                {
                    case DbTypeConst.MySQL:
                        return new MySqlConnection(dbConnInfo.ConnString);

                    case DbTypeConst.MSSqlServer:
                        return new SqlConnection(dbConnInfo.ConnString);

                    case DbTypeConst.PostgreSQL:
                        return new NpgsqlConnection(dbConnInfo.ConnString);

                    case DbTypeConst.Oracle:
                        return new OracleConnection(dbConnInfo.ConnString);

                    default:
                        throw new Exception($"暂不支持【 {dbConnInfo.DbType} 】数据库");
                }
            }
            else
                throw new Exception($"未找到唯一别名【 {uniqueAlias} 】下的数据库连接信息");
        }
    }


    /// <summary>
    /// 数据库连接信息 实体类
    /// </summary>
    public sealed class DbConnInfo
    {
        private string _dbType = string.Empty;

        /// <summary>
        /// 数据库类型；参照 DbTypeConst 数据库类型常量类
        /// </summary>
        public string DbType
        {
            get => _dbType?.ToLower() ?? string.Empty;
            set => _dbType = value ?? string.Empty;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnString { get; set; }
    }

    /// <summary>
    /// 数据库类型常量类；常量值必须设置为小写字母，以便做等值比较
    /// </summary>
    public static class DbTypeConst
    {
        /// <summary>
        /// 代表 MySQL 数据库
        /// </summary>
        public const string MySQL = "mysql";

        /// <summary>
        /// 代表 MS SqlServer 数据库
        /// </summary>
        public const string MSSqlServer = "mssqlserver";

        /// <summary>
        /// 代表 PostgreSQL 数据库
        /// </summary>
        public const string PostgreSQL = "postgresql";

        /// <summary>
        /// 代表 Oracle 数据库
        /// </summary>
        public const string Oracle = "oracle";
    }
}
