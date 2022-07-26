using System;
using System.Collections.Generic;
using System.Text;

namespace IZhy.Common.Const
{
    /// <summary>
    /// appsettings.json 文件里的 字段名 常量 抽象类 （ 提供继承支持 ）
    /// </summary>
    public abstract class AppsettingsJsonFieldsConst
    {
        /// <summary>
        /// 文件上传的根目录
        /// </summary>
        public const string FilesUploadRootDirectory = "FilesUploadRootDirectory";

        /// <summary>
        /// 上传文件的大小限制
        /// </summary>
        public const string UploadFileSizeLimit = "UploadFileSizeLimit";

        /// <summary>
        /// 文件服务的 URL 根地址
        /// </summary>
        public const string FilesServiceUrlRoot = "FilesServiceUrlRoot";

        /// <summary>
        /// 是否启用数据库日志记录器
        /// </summary>
        public const string IsEnableDbLogRecorder = "IsEnableDbLogRecorder";
    }
}
