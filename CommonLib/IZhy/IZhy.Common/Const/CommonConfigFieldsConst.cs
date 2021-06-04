namespace IZhy.Common.Const
{
    /// <summary>
    /// CommonConfig.json 公共配置文件里的字段名 常量 抽象类 （ 提供继承支持 ）
    /// </summary>
    public abstract class CommonConfigFieldsConst
    {
        /// <summary>
        /// 日志记录保存的天数
        /// </summary>
        public const string LogSaveDays = "LogSaveDays";

        /// <summary>
        /// 单个日志文件可允许的最大大小
        /// </summary>
        public const string LogFileMaxSize = "LogFileMaxSize";

        /// <summary>
        /// 日志保存的根目录
        /// </summary>
        public const string LogSaveRootDirectory = "LogSaveRootDirectory";

        /// <summary>
        /// 是否启用 Redis
        /// </summary>
        public const string IsEnableRedis = "IsEnableRedis";

        /// <summary>
        /// Redis 连接字符串
        /// </summary>
        public const string RedisConnString = "RedisConnString";

        /// <summary>
        /// 是否启用数据库连接
        /// </summary>
        public const string IsEnableDbConn = "IsEnableDbConn";
    }
}
