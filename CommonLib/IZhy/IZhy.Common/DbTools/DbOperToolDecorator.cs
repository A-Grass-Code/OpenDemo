using IZhy.Common.AopProxy;
using IZhy.Common.BasicTools;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IZhy.Common.DbTools
{
    /// <summary>
    /// 数据库操作工具类 的 装饰器
    /// </summary>
    public class DbOperToolDecorator : GeneralDecorator
    {
        protected override void Before(MethodInfo method, object[] paramArr, string exeIdNum)
        {
            if (paramArr != null && paramArr.Length >= 3)
            {
                _ = Task.Run(() =>
                {
                    StringBuilder paramSb = new StringBuilder();
                    for (int i = 1; i < paramArr.Length - 1; i++)
                    {
                        object temp = paramArr[i];
                        if (temp == null)
                        {
                            continue;
                        }
                        paramSb.AppendLine(JsonTool.ObjectToJson(temp, true));
                    }
                    paramSb.AppendLine($"sqlExeTimeout = {paramArr[paramArr.Length - 1]}");

                    LogsTool.WriteSQLLog(Convert.ToString(paramArr[0]),
                                         paramSb.ToString(),
                                         $"{TargetImpClassFullName}.{method.Name}()",
                                         exeIdNum);
                });
            }
        }

        protected override void WhenException(MethodInfo method, object[] paramArr, Exception ex, string exeIdNum)
        {
            _ = Task.Run(() =>
            {
                LogsTool.WriteEXLog("数据库操作时发生异常",
                                    ex.ToString(),
                                    null,
                                    $"{TargetImpClassFullName}.{method.Name}()",
                                    exeIdNum);
            });
        }
    }
}
