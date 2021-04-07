using IZhy.Common.AopProxy;
using IZhy.Common.BasicTools;
using System;
using System.Reflection;
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
                    string paramStr = string.Empty;
                    for (int i = 1; i < paramArr.Length - 1; i++)
                    {
                        paramStr += $"{JsonTool.ObjectToJson(paramArr[i], true)}{Environment.NewLine}";
                    }

                    LogsTool.WriteSQLLog(Convert.ToString(paramArr[0]),
                                         paramStr,
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
