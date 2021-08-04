using Dapper;
using IZhy.Common.BasicTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace IZhy.Common.DbTools
{
    /// <summary>
    /// 数据库操作工具类
    /// </summary>
    internal sealed class DbOperationTool : IDbOperation
    {
        /// <summary>
        /// 用来存放数据库连接对象
        /// </summary>
        private DbConnection _dbConn = null;

        /// <summary>
        /// 获取数据库连接对象
        /// </summary>
        private DbConnection DbConn
        {
            get
            {
                if (_dbConn == null)
                {
                    _dbConn = DbConnFactory.GetDbConnection(ConnUniqueAlias);
                }
                return _dbConn;
            }
            set => _dbConn = value;
        }


        /// <summary>
        /// 内部的事务处理对象
        /// </summary>
        private DbTransaction Transaction { get; set; } = null;

        /// <summary>
        /// 释放 DbTransaction 事务对象 （ 如果有中断的事务处理，则对其回滚后再释放 ）
        /// </summary>
        private void DisposeTransaction()
        {
            try
            {
                Transaction?.Rollback();
                Transaction?.Dispose();
                Transaction = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 回滚或释放事务处理，发生异常。{Environment.NewLine}{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 释放资源，等同于 Dispose()
        /// </summary>
        private void Dis()
        {
            CloseConn();
            DbConn.Dispose();
            DbConn = null;
        }


        /// <summary>
        /// 生成通用的 insert sql 语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicFields">字段集合</param>
        /// <returns></returns>
        private string InsertSql(string tableName, Dictionary<string, object> dicFields)
        {
            StringBuilder sqlStr = new StringBuilder();

            if (DbConn is Npgsql.NpgsqlConnection)
            {
                sqlStr.AppendLine($"INSERT INTO \"{tableName}\"");
            }
            else
                sqlStr.AppendLine($"INSERT INTO {tableName}");

            sqlStr.AppendLine($"(");

            int i = 0;
            foreach (var item in dicFields)
            {
                if (i == 0)
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"      \"{item.Key}\"");
                    }
                    else
                        sqlStr.AppendLine($"      {item.Key}");
                }
                else
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"    , \"{item.Key}\"");
                    }
                    else
                        sqlStr.AppendLine($"    , {item.Key}");
                }
                i++;
            }

            sqlStr.AppendLine($")");
            sqlStr.AppendLine($"VALUES");
            sqlStr.AppendLine($"(");

            i = 0;
            foreach (var item in dicFields)
            {
                if (i == 0)
                {
                    sqlStr.AppendLine($"      @{item.Key}");
                }
                else
                {
                    sqlStr.AppendLine($"    , @{item.Key}");
                }
                i++;
            }

            sqlStr.AppendLine($")");

            return sqlStr.ToString();
        }

        /// <summary>
        /// 生成通用的 update sql 语句；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicFields">字段集合</param>
        /// <param name="dicWhere">条件集合</param>
        /// <returns></returns>
        private string UpdateSql(string tableName, Dictionary<string, object> dicFields, Dictionary<string, object> dicWhere)
        {
            StringBuilder sqlStr = new StringBuilder();

            if (DbConn is Npgsql.NpgsqlConnection)
            {
                sqlStr.AppendLine($"UPDATE \"{tableName}\"");
            }
            else
                sqlStr.AppendLine($"UPDATE {tableName}");

            sqlStr.AppendLine($"SET");

            int i = 0;
            foreach (var item in dicFields)
            {
                if (i == 0)
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"      \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"      {item.Key} = @{item.Key}");
                }
                else
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"    , \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"    , {item.Key} = @{item.Key}");
                }
                i++;
            }

            sqlStr.AppendLine($"WHERE");

            i = 0;
            foreach (var item in dicWhere)
            {
                if (i == 0)
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"    \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"    {item.Key} = @{item.Key}");
                }
                else
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"AND \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"AND {item.Key} = @{item.Key}");
                }
                i++;
            }

            return sqlStr.ToString();
        }

        /// <summary>
        /// 生成通用的 delete sql 语句；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicWhere">条件集合</param>
        /// <returns></returns>
        private string DeleteSql(string tableName, Dictionary<string, object> dicWhere)
        {
            StringBuilder sqlStr = new StringBuilder();

            if (DbConn is Npgsql.NpgsqlConnection)
            {
                sqlStr.AppendLine($"DELETE FROM \"{tableName}\"");
            }
            else
                sqlStr.AppendLine($"DELETE FROM {tableName}");

            sqlStr.AppendLine($"WHERE");

            int i = 0;
            foreach (var item in dicWhere)
            {
                if (i == 0)
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"    \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"    {item.Key} = @{item.Key}");
                }
                else
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"AND \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"AND {item.Key} = @{item.Key}");
                }
                i++;
            }

            return sqlStr.ToString();
        }

        /// <summary>
        /// 生成只查询某一张表数据的 select 语句；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="orderBy">字段排序数组，例：new string[] { "Field1 DESC", "Field2 ASC" }</param>
        /// <returns></returns>
        private string SelectATableByANDEqualSignSql(string tableName, string[] fields, Dictionary<string, object> dicWhere, string[] orderBy = null)
        {
            StringBuilder sqlStr = new StringBuilder();

            sqlStr.AppendLine($"SELECT");

            int i = 0;
            for (; i < fields.Length; i++)
            {
                if (i == 0)
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"      \"{fields[i]}\"");
                    }
                    else
                        sqlStr.AppendLine($"      {fields[i]}");
                }
                else
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"    , \"{fields[i]}\"");
                    }
                    else
                        sqlStr.AppendLine($"    , {fields[i]}");
                }
            }

            if (DbConn is Npgsql.NpgsqlConnection)
            {
                sqlStr.AppendLine($"FROM \"{tableName}\"");
            }
            else
                sqlStr.AppendLine($"FROM {tableName}");

            sqlStr.AppendLine($"WHERE");

            i = 0;
            foreach (var item in dicWhere)
            {
                if (i == 0)
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"    \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"    {item.Key} = @{item.Key}");
                }
                else
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"AND \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"AND {item.Key} = @{item.Key}");
                }
                i++;
            }

            if (orderBy?.Length > 0)
            {
                sqlStr.Append("ORDER BY");
                for (i = 0; i < orderBy.Length; i++)
                {
                    if (i == 0)
                    {
                        sqlStr.Append($" {orderBy[i]}");
                    }
                    else
                    {
                        sqlStr.Append($", {orderBy[i]}");
                    }
                }
                sqlStr.AppendLine();
            }

            return sqlStr.ToString();
        }


        /// <summary>
        /// 用于日志记录 sql
        /// <para>形如：[换行符] 运行时 SQL => [换行符] sql</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static string LogSql(string sql)
        {
            return $"{Environment.NewLine}运行时 SQL =>{Environment.NewLine}{sql}";
        }

        /// <summary>
        /// 用于日志记录 param
        /// <para>形如：[换行符] 运行时 参数 => [换行符] param</para>
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string LogParam(object param)
        {
            return $"{Environment.NewLine}运行时 参数 =>{Environment.NewLine}{JsonTool.ObjectToJson(param, true)}";
        }

        /// <summary>
        /// 有效的 sql 执行时间，单位 秒
        /// <para>有效值范围 1~60</para>
        /// </summary>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒；默认 20</param>
        /// <returns></returns>
        private int ValidSqlExeTime(int sqlExeTimeout = 20)
        {
            if (sqlExeTimeout < 1)
            {
                sqlExeTimeout = 1;
            }
            else if (sqlExeTimeout > 60)
            {
                sqlExeTimeout = 60;
            }
            return sqlExeTimeout;
        }


        /// <summary>
        /// 获取 配置文件中连接信息的唯一别名
        /// </summary>
        public string ConnUniqueAlias { get; } = DbConnFactory.DefaultConnUniqueAlias;


        /// <summary>
        /// 无参构造函数，初始化 DbOperationTool 类的新示例
        /// <para>属性 ConnUniqueAlias 连接信息的唯一别名是默认值 : "default"</para>
        /// </summary>
        public DbOperationTool()
        {
            ConnUniqueAlias = DbConnFactory.DefaultConnUniqueAlias;
        }

        /// <summary>
        /// 有参构造函数，初始化 DbOperationTool 类的新示例
        /// <para>设置 属性 ConnUniqueAlias 连接信息的唯一别名</para>
        /// </summary>
        /// <param name="connUniqueAlias">属性 ConnUniqueAlias 连接信息的唯一别名</param>
        public DbOperationTool(string connUniqueAlias)
        {
            ConnUniqueAlias = connUniqueAlias;
        }


        public string GetDbName()
        {
            return DbConn?.Database;
        }

        public string GetDbVersion()
        {
            return DbConn?.ServerVersion;
        }

        public string GetDbSource()
        {
            return DbConn?.DataSource;
        }

        public int GetConnTimeout()
        {
            return DbConn?.ConnectionTimeout ?? -1;
        }

        public string GetConnString()
        {
            return DbConn?.ConnectionString;
        }


        /// <summary>
        /// 打开连接
        /// </summary>
        public void OpenConn()
        {
            DisposeTransaction();
            try
            {
                switch (DbConn.State)
                {
                    case ConnectionState.Closed:
                        DbConn.Open();
                        break;
                    case ConnectionState.Broken:
                        DbConn.Close();
                        DbConn.Open();
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 打开数据库连接，发生异常。{Environment.NewLine}{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConn()
        {
            DisposeTransaction();
            try
            {
                if (DbConn.State != ConnectionState.Closed)
                {
                    DbConn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 关闭数据库连接，发生异常。{Environment.NewLine}{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 开启事务处理
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                Transaction = DbConn.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 开启事务处理，发生异常。{Environment.NewLine}{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                Transaction?.Commit();
                Transaction?.Dispose();
                Transaction = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 提交事务处理，发生异常。{Environment.NewLine}{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            DisposeTransaction();
        }


        /// <summary>
        /// sql 查询
        /// </summary>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public dynamic QueryBySql(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null
                dynamic dyData = DbConn.Query(sql, param, Transaction, true, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return dyData;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 dynamic QueryBySql() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }

        /// <summary>
        /// sql 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public List<T> QueryBySql<T>(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null
                var data = DbConn.Query<T>(sql, param, Transaction, true, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 List<T> QueryBySql<T>() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }

        /// <summary>
        /// sql 查询，返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public DataTable QueryToDataTable(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var data = DbConn.ExecuteReader(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                DataTable dataTable = new DataTable();
                dataTable.Load(data);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 DataTable QueryToDataTable() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }


        /// <summary>
        /// 执行 sql 查询，返回第一行第一列的值，object 对象；例如：COUNT(0) 函数
        /// </summary>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, object param = null, int sqlExeTimeout = 20)
        {
            // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
            try
            {
                object obj = DbConn.ExecuteScalar(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 object ExecuteScalar() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }

        /// <summary>
        /// 执行 sql 查询，返回第一行第一列的值；例如：COUNT(0) 函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                T obj = DbConn.ExecuteScalar<T>(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 T ExecuteScalar<T>() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }


        /// <summary>
        /// sql 查询 多个结果集
        /// </summary>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public List<dynamic> QueryMultiple(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                List<dynamic> results = new List<dynamic>();
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var res = DbConn.QueryMultiple(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                while (!res.IsConsumed)
                {
                    results.Add(res.Read());
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 List<dynamic> QueryMultiple() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }

        /// <summary>
        /// sql 查询 多个结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public List<List<T>> QueryMultiple<T>(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                List<List<T>> results = new List<List<T>>();
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var res = DbConn.QueryMultiple(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                while (!res.IsConsumed)
                {
                    results.Add(res.Read<T>().ToList());
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 List<List<T>> QueryMultiple<T>() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }


        /// <summary>
        /// sql 查询 第一条结果
        /// </summary>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public dynamic QueryFirst(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                dynamic dyData = DbConn.QueryFirstOrDefault(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return dyData;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 dynamic QueryFirst() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }

        /// <summary>
        /// sql 查询 第一条结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public T QueryFirst<T>(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                T dyData = DbConn.QueryFirstOrDefault<T>(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return dyData;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 T QueryFirst<T>() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }


        /// <summary>
        /// sql 查询 单条结果
        /// <para>若有多条结果则抛异常</para>
        /// </summary>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public dynamic QuerySingle(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                dynamic dyData = DbConn.QuerySingleOrDefault(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return dyData;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 dynamic QuerySingle() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }

        /// <summary>
        /// sql 查询 单条结果
        /// <para>若有多条结果则抛异常</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                T dyData = DbConn.QuerySingleOrDefault<T>(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return dyData;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 T QuerySingle<T>() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }


        /// <summary>
        /// 执行 sql 返回受影响行数
        /// </summary>
        /// <param name="sql">sql 语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象，匿名对象的数组或集合（在批量增、删、改时）</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public int ExecuteSql(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var rows = DbConn.Execute(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 int ExecuteSql() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }

        /// <summary>
        /// 执行 sql （存储过程） 返回受影响行数
        /// </summary>
        /// <param name="sql">sql（存储过程）语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public int ExecuteProcedure(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var rows = DbConn.Execute(sql, param, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.StoredProcedure);
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 int ExecuteProcedure() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }


        /// <summary>
        /// sql（存储过程）查询
        /// </summary>
        /// <param name="sql">sql（存储过程）语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public dynamic QueryProcedure(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null
                dynamic dyData = DbConn.Query(sql, param, Transaction, true, ValidSqlExeTime(sqlExeTimeout), CommandType.StoredProcedure);
                return dyData;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 dynamic QueryProcedure() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }

        /// <summary>
        /// sql（存储过程）查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql（存储过程）语句 / 命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public List<T> QueryProcedure<T>(string sql, object param = null, int sqlExeTimeout = 20)
        {
            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null
                var data = DbConn.Query<T>(sql, param, Transaction, true, ValidSqlExeTime(sqlExeTimeout), CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 List<T> QueryProcedure<T>() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(param)}", ex);
            }
        }


        /// <summary>
        /// 新增数据 通用方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicFields">字段集合</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public int ExeInsertSql(string tableName, Dictionary<string, object> dicFields, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (dicFields == null || dicFields.Count < 1)
            {
                throw new Exception("字段参数错误");
            }

            string sql = InsertSql(tableName, dicFields);

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var rows = DbConn.Execute(sql, dicFields, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 int ExeInsertSql() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(dicFields)}", ex);
            }
        }

        /// <summary>
        /// 批量新增数据 通用方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="listFields">字段集合（批量操作）</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public int ExeBatchInsertSql(string tableName, List<Dictionary<string, object>> listFields, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (listFields == null || listFields.Count < 1)
            {
                throw new Exception("字段参数错误");
            }

            Dictionary<string, object> dicFields = listFields[0];

            if (dicFields == null || dicFields.Count < 1)
            {
                throw new Exception("字段参数错误");
            }

            string sql = InsertSql(tableName, dicFields);

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var rows = DbConn.Execute(sql, listFields, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 int ExeBatchInsertSql() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(listFields)}", ex);
            }
        }


        /// <summary>
        /// 修改数据 通用方法；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicFields">字段集合</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public int ExeUpdateSql(string tableName, Dictionary<string, object> dicFields,
            Dictionary<string, object> dicWhere, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (dicFields == null || dicFields.Count < 1)
            {
                throw new Exception("字段参数错误");
            }

            if (dicWhere == null || dicWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            string sql = UpdateSql(tableName, dicFields, dicWhere);

            Dictionary<string, object> dicParams = dicFields.Union(dicWhere).ToDictionary(kv => kv.Key, kv => kv.Value);

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var rows = DbConn.Execute(sql, dicParams, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 int ExeUpdateSql() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(dicParams)}", ex);
            }
        }

        /// <summary>
        /// 批量修改数据 通用方法；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="listDicFields">字段集合（批量操作）</param>
        /// <param name="listDicWhere">条件集合（批量操作）</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public int ExeBatchUpdateSql(string tableName, List<Dictionary<string, object>> listDicFields,
            List<Dictionary<string, object>> listDicWhere, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (listDicFields == null || listDicFields.Count < 1)
            {
                throw new Exception("字段参数错误");
            }

            if (listDicWhere == null || listDicWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            if (listDicFields.Count != listDicWhere.Count)
            {
                throw new Exception("字段参数和条件参数不匹配");
            }

            string sql = UpdateSql(tableName, listDicFields[0], listDicWhere[0]);

            List<Dictionary<string, object>> listDicParams = new List<Dictionary<string, object>>();
            for (int i = 0; i < listDicFields.Count; i++)
            {
                listDicParams.Add(listDicFields[i].Union(listDicWhere[i]).ToDictionary(kv => kv.Key, kv => kv.Value));
            }

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var rows = DbConn.Execute(sql, listDicParams, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 int ExeBatchUpdateSql() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(listDicParams)}", ex);
            }
        }


        /// <summary>
        /// 删除数据 通用方法；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public int ExeDeleteSql(string tableName, Dictionary<string, object> dicWhere, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (dicWhere == null || dicWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            string sql = DeleteSql(tableName, dicWhere);

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var rows = DbConn.Execute(sql, dicWhere, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 int ExeDeleteSql() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(dicWhere)}", ex);
            }
        }

        /// <summary>
        /// 批量删除数据 通用方法；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="listWhere">条件集合（批量操作）</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public int ExeBatchDeleteSql(string tableName, List<Dictionary<string, object>> listWhere, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (listWhere == null || listWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            Dictionary<string, object> dicWhere = listWhere[0];

            if (dicWhere == null || dicWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            string sql = DeleteSql(tableName, dicWhere);

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var rows = DbConn.Execute(sql, listWhere, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 int ExeBatchDeleteSql() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(listWhere)}", ex);
            }
        }


        /// <summary>
        /// 检查某一张表中的数据是否已存在；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public bool CheckATableDataExist(string tableName, Dictionary<string, object> dicWhere, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (dicWhere == null || dicWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            StringBuilder sqlStr = new StringBuilder();

            sqlStr.AppendLine($"SELECT COUNT(0) Count");

            if (DbConn is Npgsql.NpgsqlConnection)
            {
                sqlStr.AppendLine($"FROM \"{tableName}\"");
            }
            else
                sqlStr.AppendLine($"FROM {tableName}");

            sqlStr.AppendLine($"WHERE");

            int i = 0;
            foreach (var item in dicWhere)
            {
                if (i == 0)
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"    \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"    {item.Key} = @{item.Key}");
                }
                else
                {
                    if (DbConn is Npgsql.NpgsqlConnection)
                    {
                        sqlStr.AppendLine($"AND \"{item.Key}\" = @{item.Key}");
                    }
                    else
                        sqlStr.AppendLine($"AND {item.Key} = @{item.Key}");
                }
                i++;
            }

            string sql = sqlStr.ToString();

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                int count = DbConn.ExecuteScalar<int>(sql, dicWhere, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                if (count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 bool CheckATableDataExist() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(dicWhere)}", ex);
            }
        }

        /// <summary>
        /// 查询某一张表中的数据；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="orderBy">字段排序数组，例：new string[] { "Field1 DESC", "Field2 ASC" }</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        public dynamic SelectATableByANDEqualSign(string tableName, string[] fields, Dictionary<string, object> dicWhere, string[] orderBy = null, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (fields == null || fields.Length < 1)
            {
                throw new Exception("字段参数错误");
            }

            if (dicWhere == null || dicWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            string sql = SelectATableByANDEqualSignSql(tableName, fields, dicWhere, orderBy);

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null
                dynamic dyData = DbConn.Query(sql, dicWhere, Transaction, true, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return dyData;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 dynamic SelectATableByANDEqualSign() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(dicWhere)}", ex);
            }
        }

        /// <summary>
        /// 查询某一张表中的数据；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="orderBy">字段排序数组，例：new string[] { "Field1 DESC", "Field2 ASC" }</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        public List<T> SelectATableByANDEqualSign<T>(string tableName, string[] fields, Dictionary<string, object> dicWhere, string[] orderBy = null, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (fields == null || fields.Length < 1)
            {
                throw new Exception("字段参数错误");
            }

            if (dicWhere == null || dicWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            string sql = SelectATableByANDEqualSignSql(tableName, fields, dicWhere, orderBy);

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null
                var data = DbConn.Query<T>(sql, dicWhere, Transaction, true, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 List<T> SelectATableByANDEqualSign<T>() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(dicWhere)}", ex);
            }
        }

        /// <summary>
        /// 查询某一张表中的数据；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="orderBy">字段排序数组，例：new string[] { "Field1 DESC", "Field2 ASC" }</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        public DataTable SelectATableByANDEqualSignToDt(string tableName, string[] fields, Dictionary<string, object> dicWhere, string[] orderBy = null, int sqlExeTimeout = 20)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("表名错误");
            }

            if (fields == null || fields.Length < 1)
            {
                throw new Exception("字段参数错误");
            }

            if (dicWhere == null || dicWhere.Count < 1)
            {
                throw new Exception("条件参数错误");
            }

            string sql = SelectATableByANDEqualSignSql(tableName, fields, dicWhere, orderBy);

            try
            {
                // string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null
                var data = DbConn.ExecuteReader(sql, dicWhere, Transaction, ValidSqlExeTime(sqlExeTimeout), CommandType.Text);
                DataTable dataTable = new DataTable();
                dataTable.Load(data);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception($"【DbOperationTool】 DataTable SelectATableByANDEqualSignToDt() 运行时发生异常。" +
                    $"{Environment.NewLine}{ex.Message}{LogSql(sql)}{LogParam(dicWhere)}", ex);
            }
        }


        public override string ToString()
        {
            return JsonTool.ObjectToJson(new
            {
                ConnUniqueAlias,
                DbName = GetDbName(),
                DbSource = GetDbSource(),
                DbVersion = GetDbVersion(),
                ConnTimeout = GetConnTimeout(),
                ConnString = GetConnString()
            }, true);
        }

        public void Dispose()
        {
            Dis();
        }
    }
}
