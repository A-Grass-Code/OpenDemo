using IZhy.Common.BasicTools;
using System;

namespace IZhy.Common.SysEntities
{
    /// <summary>
    /// Controller 层 参数实体类
    /// </summary>
    public class ControllerParamEntity
    {
        #region 业务逻辑所需的通用字段

        /// <summary>
        /// 用户 ID 号
        /// </summary>
        public object UID { get; set; } = null;

        /// <summary>
        /// 用于分页查询，每页的数据量
        /// </summary>
        public int? PageSize { get; set; } = PagingDataEntity.DefaultPageSize;

        /// <summary>
        /// 用于分页查询，当前页码号
        /// </summary>
        public int? PageNum { get; set; } = 1;

        /// <summary>
        /// 开始时间，条件查询用
        /// </summary>
        public DateTime? BeginTime { get; set; } = null;

        /// <summary>
        /// 终止时间，条件查询用
        /// </summary>
        public DateTime? EndTime { get; set; } = null;

        #endregion


        /// <summary>
        /// Json 格式的参数 （ 字符串形式 ）
        /// </summary>
        public string JsonParam { get; set; }


        /// <summary>
        /// 用 JsonParam 字符串参数，把 Json 格式的字符串转成 dynamic 对象
        /// </summary>
        public dynamic ParamToDynamic()
        {
            return JsonTool.JsonToDynamic(JsonParam);
        }

        /// <summary>
        /// 用 JsonParam 字符串参数，把 Json 格式的字符串转成 T 类型的实体对象
        /// </summary>
        /// <typeparam name="T">泛型 类型</typeparam>
        /// <returns></returns>
        public T ParamToEntity<T>()
        {
            return JsonTool.JsonToObject<T>(JsonParam);
        }


        public override string ToString()
        {
            return $"{nameof(UID)} = <{UID}> , "
                 + $"{nameof(PageSize)} = <{PageSize}> , "
                 + $"{nameof(PageNum)} = <{PageNum}> , "
                 + $"{nameof(BeginTime)} = <{BeginTime?.ToString("yyyy-MM-dd HH:mm:ss")}> , "
                 + $"{nameof(EndTime)} = <{EndTime?.ToString("yyyy-MM-dd HH:mm:ss")}>"
                 + $"{Environment.NewLine}"
                 + $"{nameof(JsonParam)} = <{JsonParam}>";
        }
    }
}
