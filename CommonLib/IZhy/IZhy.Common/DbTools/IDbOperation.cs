using System;
using System.Collections.Generic;
using System.Data;

namespace IZhy.Common.DbTools
{
    /// <summary>
    /// 定义的数据库操作接口
    /// </summary>
    public interface IDbOperation : IDisposable
    {
        /// <summary>
        /// 获取 配置文件中连接信息的唯一别名
        /// </summary>
        string ConnUniqueAlias { get; }


        string GetDbName();

        string GetDbVersion();

        string GetDbSource();

        int GetConnTimeout();

        string GetConnString();


        /// <summary>
        /// 打开连接
        /// </summary>
        void OpenConn();

        /// <summary>
        /// 关闭连接
        /// </summary>
        void CloseConn();

        /// <summary>
        /// 开启事务处理
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();


        /// <summary>
        /// sql 查询
        /// </summary>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        dynamic QueryBySql(string sql, object param = null, int sqlExeTimeout = 20);

        /// <summary>
        /// sql 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        List<T> QueryBySql<T>(string sql, object param = null, int sqlExeTimeout = 20);

        /// <summary>
        /// sql 查询，返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        DataTable QueryToDataTable(string sql, object param = null, int sqlExeTimeout = 20);


        /// <summary>
        /// 执行 sql 查询，返回第一行第一列的值，object 对象；例如：COUNT(0) 函数
        /// </summary>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        object ExecuteScalar(string sql, object param = null, int sqlExeTimeout = 20);

        /// <summary>
        /// 执行 sql 查询，返回第一行第一列的值；例如：COUNT(0) 函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, object param = null, int sqlExeTimeout = 20);


        /// <summary>
        /// sql 查询 多个结果集
        /// </summary>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        List<dynamic> QueryMultiple(string sql, object param = null, int sqlExeTimeout = 20);

        /// <summary>
        /// sql 查询 多个结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        List<List<T>> QueryMultiple<T>(string sql, object param = null, int sqlExeTimeout = 20);


        /// <summary>
        /// sql 查询 第一条结果
        /// </summary>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        dynamic QueryFirst(string sql, object param = null, int sqlExeTimeout = 20);

        /// <summary>
        /// sql 查询 第一条结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        T QueryFirst<T>(string sql, object param = null, int sqlExeTimeout = 20);


        /// <summary>
        /// sql 查询 单条结果
        /// <para>若有多条结果则抛异常</para>
        /// </summary>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        dynamic QuerySingle(string sql, object param = null, int sqlExeTimeout = 20);

        /// <summary>
        /// sql 查询 单条结果
        /// <para>若有多条结果则抛异常</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        T QuerySingle<T>(string sql, object param = null, int sqlExeTimeout = 20);


        /// <summary>
        /// 执行 sql 返回受影响行数
        /// </summary>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象，匿名对象的数组或集合（在批量增、删、改时）</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        int ExecuteSql(string sql, object param = null, int sqlExeTimeout = 20);

        /// <summary>
        /// 执行 sql （存储过程） 返回受影响行数
        /// </summary>
        /// <param name="sql">sql 语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        int ExecuteProcedure(string sql, object param = null, int sqlExeTimeout = 20);


        /// <summary>
        /// sql（存储过程）查询
        /// </summary>
        /// <param name="sql">sql（存储过程）语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        dynamic QueryProcedure(string sql, object param = null, int sqlExeTimeout = 20);

        /// <summary>
        /// sql（存储过程）查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql（存储过程）语句或命令</param>
        /// <param name="param">sql 执行时的参数；一般是匿名对象、字典集合、实体对象</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        List<T> QueryProcedure<T>(string sql, object param = null, int sqlExeTimeout = 20);


        /// <summary>
        /// 新增数据 通用方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicFields">字段集合</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        int ExeInsertSql(string tableName, Dictionary<string, object> dicFields, int sqlExeTimeout = 20);

        /// <summary>
        /// 批量新增数据 通用方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="listFields">字段集合（批量操作）</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        int ExeBatchInsertSql(string tableName, List<Dictionary<string, object>> listFields, int sqlExeTimeout = 20);


        /// <summary>
        /// 修改数据 通用方法；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicFields">字段集合</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        int ExeUpdateSql(string tableName, Dictionary<string, object> dicFields,
            Dictionary<string, object> dicWhere, int sqlExeTimeout = 20);

        /// <summary>
        /// 批量修改数据 通用方法；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="listDicFields">字段集合（批量操作）</param>
        /// <param name="listDicWhere">条件集合（批量操作）</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~60</param>
        /// <returns></returns>
        int ExeBatchUpdateSql(string tableName, List<Dictionary<string, object>> listDicFields,
           List<Dictionary<string, object>> listDicWhere, int sqlExeTimeout = 20);


        /// <summary>
        /// 删除数据 通用方法；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        int ExeDeleteSql(string tableName, Dictionary<string, object> dicWhere, int sqlExeTimeout = 20);

        /// <summary>
        /// 批量删除数据 通用方法；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="listWhere">条件集合（批量操作）</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        int ExeBatchDeleteSql(string tableName, List<Dictionary<string, object>> listWhere, int sqlExeTimeout = 20);


        /// <summary>
        /// 检查某一张表中的数据是否已存在；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        bool CheckATableDataExist(string tableName, Dictionary<string, object> dicWhere, int sqlExeTimeout = 20);

        /// <summary>
        /// 查询某一张表中的数据；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="orderBy">字段排序数组，例：new string[] { "Field1 DESC", "Field2 ASC" }</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        dynamic SelectATableByANDEqualSign(string tableName, string[] fields, Dictionary<string, object> dicWhere, string[] orderBy = null, int sqlExeTimeout = 20);

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
        List<T> SelectATableByANDEqualSign<T>(string tableName, string[] fields, Dictionary<string, object> dicWhere, string[] orderBy = null, int sqlExeTimeout = 20);

        /// <summary>
        /// 查询某一张表中的数据；条件字段 AND 连接，“=” 等号运算
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="dicWhere">条件集合</param>
        /// <param name="orderBy">字段排序数组，例：new string[] { "Field1 DESC", "Field2 ASC" }</param>
        /// <param name="sqlExeTimeout">sql 执行的超时时间，单位 秒，默认 20；有效值范围 1~120</param>
        /// <returns></returns>
        DataTable SelectATableByANDEqualSignToDt(string tableName, string[] fields, Dictionary<string, object> dicWhere, string[] orderBy = null, int sqlExeTimeout = 20);


        string ToString();
    }
}
