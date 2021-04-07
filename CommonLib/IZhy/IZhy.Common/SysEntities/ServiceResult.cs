using IZhy.Common.BasicTools;

namespace IZhy.Common.SysEntities
{
    /// <summary>
    /// 服务接口 结果 实体类
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucceed { get; set; } = false;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "无消息";

        /// <summary>
        /// 结果对象
        /// </summary>
        public object Result { get; set; } = null;


        public ServiceResult() { }

        public ServiceResult(bool isSucc, object result = null)
        {
            IsSucceed = isSucc;
            if (isSucc)
            {
                Message = "成功的";
            }
            else
            {
                Message = "失败的";
            }
            Result = result;
        }

        public ServiceResult(string msg, bool isSucc = false)
        {
            Message = msg;
            IsSucceed = isSucc;
        }

        public ServiceResult(bool isSucc, string msg, object result)
        {
            IsSucceed = isSucc;
            Message = msg;
            Result = result;
        }


        public override string ToString()
        {
            return JsonTool.ObjectToJson(this);
        }

        /// <summary>
        /// 把 ToString() 字符串中的特殊字符转换成 HTML 可识别的字符
        /// </summary>
        /// <returns></returns>
        public string ToHtml()
        {
            string html = this.ToString();

            if (html.Contains("<"))
            {
                html = html.Replace("<", "&lt;");
            }

            if (html.Contains(">"))
            {
                html = html.Replace(">", "&gt;");
            }

            if (html.Contains(@"\r\n"))
            {
                html = html.Replace(@"\r\n", "<br/>");
            }

            if (html.Contains(@"\n"))
            {
                html = html.Replace(@"\n", "<br/>");
            }

            return html;
        }
    }
}
