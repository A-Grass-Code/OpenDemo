using IZhy.Common.BasicTools;
using IZhy.Common.DbTools;
using IZhy.Common.SysEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello World!{Environment.NewLine}");

            Console.WriteLine(Environment.CurrentDirectory);

            // 日志测试
            {
                LogsTool.LogWritingExe = logInfo =>
                {
                    Console.WriteLine($"日志写入时执行 | {DateTime.Now.ToString("HH:mm:ss.ffff")}");
                    Console.WriteLine($"{JsonTool.ObjectToJson(logInfo, true)} | {DateTime.Now.ToString("HH:mm:ss.ffff")}");
                };

                LogsTool.ClearBeforeSpecifiedDaysLog = saveDays =>
                {
                    Console.WriteLine($"清除指定天数前的日志 => 天数： {saveDays} | {DateTime.Now.ToString("HH:mm:ss.ffff")}");
                };

                for (int i = 0; i < 100; i++)
                {
                    Task.Factory.StartNew(num =>
                    {
                        LogsTool.ConsoleLog($"日志测试 {num} | {DateTime.Now.ToString("HH:mm:ss.ffff")}");
                    }, i + 1);

                    Task.Run(() =>
                    {
                        LogsTool.ConsoleLog($"日志测试 {i} | {DateTime.Now.ToString("HH:mm:ss.ffff")}");
                    });
                }
            }

            // 数据库操作测试
            {
                //string sql = " sql ".SqlWhere(true);
                //sql += " test1 ".SqlWhere(false);
                //sql += " test2 ".SqlWhere(true);
                //Console.WriteLine(sql);

                //try
                //{
                //    using (var db = InvokeDbOperTool.DbOperTool())
                //    {
                //        StringBuilder sql = new StringBuilder();
                //        sql.AppendLine("SELECT * FROM test WHERE `Name` LIKE '%1';");
                //        sql.AppendLine("SELECT * FROM test WHERE Sex = '女';");
                //        sql.AppendLine("SELECT * FROM test WHERE Age > 23;");

                //        var dbConnInfo = db.GetConnStringInfo();
                //        Console.WriteLine(JsonTool.ObjectToJson(dbConnInfo, true));

                //        Console.WriteLine();

                //        var data = db.QueryMultiple<DbTestEntity>(sql.ToString());
                //        Console.WriteLine(JsonTool.ObjectToJson(data, true));
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}

                //try
                //{
                //    sql = $" SELECT * FROM test "
                //        + $" WHERE `Name` LIKE @Name "
                //        + $" AND Sex = @Sex ".SqlWhere(true);

                //    using (var db = InvokeDbOperTool.DbOperTool())
                //    {
                //        var res = db.QueryBySql(sql, new { Name = "%1", Sex = "女" });
                //        Console.WriteLine(JsonTool.ObjectToJson(res, true));
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
            }


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
