using IZhy.Common.BasicTools;
using System.Text;

namespace IZhy.Common.SysEntities
{
    /// <summary>
    /// 服务接口 结果 实体类
    /// </summary>
    public class ServiceResultEntity
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


        public ServiceResultEntity() { }

        public ServiceResultEntity(bool isSucc, object result = null)
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

        public ServiceResultEntity(string msg, bool isSucc = false)
        {
            Message = msg;
            IsSucceed = isSucc;
        }

        public ServiceResultEntity(bool isSucc, string msg, object result)
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
            StringBuilder sbHtml = new StringBuilder(this.ToString());

            sbHtml.Replace("<", "&lt;");
            sbHtml.Replace(">", "&gt;");

            sbHtml.Replace(@"\r\n", "<br/>");
            sbHtml.Replace(@"\n\r", "<br/>");
            sbHtml.Replace(@"\n", "<br/>");

            // &nbsp; 字符：不断行的空白格，该空格占据的宽度受字体影响(一个字符宽度)。
            // &ensp; 字符：相当全角状态键入半个“空格”键（半个汉字的宽度，一个字符宽度）。
            // &emsp; 字符：相当全角状态键入“空格”键（1个汉字的宽度，两个字符宽度）。

            sbHtml.Replace(" ", "&ensp;");

            return sbHtml.ToString();
        }
    }
}
