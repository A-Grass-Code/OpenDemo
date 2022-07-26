using IZhy.Common.Const;
using IZhy.Common.SimpleTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IZhy.Common.BasicTools
{
    /// <summary>
    /// 枚举类 日志类型
    /// </summary>
    public enum ELogType
    {
        /// <summary>
        /// 记录类型
        /// </summary>
        INFO = 1,

        /// <summary>
        /// 异常类型
        /// </summary>
        EX = 2,

        /// <summary>
        /// 警告类型
        /// </summary>
        WARN = 3,

        /// <summary>
        /// 运行时 SQL 记录
        /// </summary>
        SQL = 4,

        /// <summary>
        /// 流程性日志记录
        /// </summary>
        FLOW = 5
    }

    /// <summary>
    /// 日志工具类
    /// </summary>
    public static class LogsTool
    {
        /// <summary>
        /// 写入中（记录性日志） ...
        /// </summary>
        private static readonly object _writeingINFO = new object();

        /// <summary>
        /// 写入中（异常性日志） ...
        /// </summary>
        private static readonly object _writeingEX = new object();

        /// <summary>
        /// 写入中（警告性日志） ...
        /// </summary>
        private static readonly object _writeingWARN = new object();

        /// <summary>
        /// 写入中（运行时 SQL 日志） ...
        /// </summary>
        private static readonly object _writeingSQL = new object();

        /// <summary>
        /// 写入中（流程性日志） ...
        /// </summary>
        private static readonly object _writeingFLOW = new object();


        /// <summary>
        /// 获取日志保存的根目录
        /// </summary>
        /// <returns></returns>
        private static string GetLogSaveRootDirectory()
        {
            try
            {
                string directory = Convert.ToString(CommonConfigTool.GetConfig(CommonConfigFieldsConst.LogSaveRootDirectory));

                if (string.IsNullOrWhiteSpace(directory))
                {
                    return FilesTool.ProgramRootDirectoryLogs;
                }

                if (directory.Last<char>().ToString() != FilesTool.PathSeparator)
                {
                    directory += FilesTool.PathSeparator;
                }

                FilesTool.CreateFilePathDirectory(directory);

                return directory;
            }
            catch (Exception)
            {
                return FilesTool.ProgramRootDirectoryLogs;
            }
        }


        /// <summary>
        /// 获取【记录性 INFO】日志的物理路径
        /// </summary>
        private static string INFOLogPath => $"{GetLogSaveRootDirectory()}{DateTime.Now:yyyy-MM-dd}{FilesTool.PathSeparator}INFO.log";

        /// <summary>
        /// 获取【异常性 EX】日志的物理路径
        /// </summary>
        private static string EXLogPath => $"{GetLogSaveRootDirectory()}{DateTime.Now:yyyy-MM-dd}{FilesTool.PathSeparator}EX.log";

        /// <summary>
        /// 获取【警告性 WARN】日志的物理路径
        /// </summary>
        private static string WARNLogPath => $"{GetLogSaveRootDirectory()}{DateTime.Now:yyyy-MM-dd}{FilesTool.PathSeparator}WARN.log";

        /// <summary>
        /// 获取【运行时 SQL】日志的物理路径
        /// </summary>
        private static string SQLLogPath => $"{GetLogSaveRootDirectory()}{DateTime.Now:yyyy-MM-dd}{FilesTool.PathSeparator}SQL.log";

        /// <summary>
        /// 获取【流程性日志】日志的物理路径
        /// </summary>
        private static string FLOWLogPath => $"{GetLogSaveRootDirectory()}{DateTime.Now:yyyy-MM-dd}{FilesTool.PathSeparator}FLOW.log";


        /// <summary>
        /// 检查单个日志文件是否超出可允许的最大大小
        /// <para>若超出可允许的最大大小（单位 MB），则返回 true</para>
        /// </summary>
        /// <param name="logFilePath">日志文件的绝对路径</param>
        /// <returns></returns>
        private static bool CheckLogFileBeyondMaxSize(string logFilePath)
        {
            if (FilesTool.GetFileSize(logFilePath) >= 1024 * 1024 * LogFileMaxSize())
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 日志记录保存的天数；默认 5 天
        /// <para>允许的设置范围是： 1 ~ 15</para>
        /// </summary>
        public static int LogSaveDays()
        {
            try
            {
                int days = Convert.ToInt32(CommonConfigTool.GetConfig(CommonConfigFieldsConst.LogSaveDays) ?? 5);
                if (days > 15)
                {
                    days = 15;
                }
                else if (days < 1)
                {
                    days = 1;
                }
                return days;
            }
            catch (Exception)
            {
                return 5;
            }
        }

        /// <summary>
        /// 单个日志文件可允许的最大大小，单位：MB，默认 10.0 （ 若超出，则不再向当前日志文件中继续写入 ）
        /// </summary>
        /// <returns></returns>
        public static double LogFileMaxSize()
        {
            try
            {
                double fileSize = Convert.ToDouble(CommonConfigTool.GetConfig(CommonConfigFieldsConst.LogFileMaxSize) ?? 10.0);
                if (fileSize < 10)
                {
                    fileSize = 10.0;
                }
                return fileSize;
            }
            catch (Exception)
            {
                return 10.0;
            }
        }


        /// <summary>
        /// 日志写入时执行
        /// <para>默认 啥也不做</para>
        /// </summary>
        public static Action<Dictionary<string, object>> LogWritingExe { private get; set; } = logInfo =>
        {
            // 默认 啥也不做
        };

        /// <summary>
        /// 清除指定天数前的日志；例如 清除数据库中的日志
        /// <para>默认 啥也不做</para>
        /// </summary>
        public static Action<int> ClearBeforeSpecifiedDaysLog { private get; set; } = logSaveDays =>
        {
            // 默认 啥也不做
        };


        static LogsTool()
        {
            new TimedFixedTask(() =>
            {
                Task.Run(() =>
                {
                    FilesTool.ClearDirectoryBeforeSpecifiedDaysFile(GetLogSaveRootDirectory(), LogSaveDays());
                });
                Task.Run(() =>
                {
                    try
                    {
                        ClearBeforeSpecifiedDaysLog(LogSaveDays());
                        WriteINFOLog("ClearBeforeSpecifiedDaysLog() 清理数据库中的日志记录，执行成功");
                    }
                    catch (Exception ex)
                    {
                        WriteEXLog("ClearBeforeSpecifiedDaysLog() 清理数据库中的日志记录，执行失败", ex);
                    }
                });
            }, ERepetitionTimePeriod.Daily, new FixedTimed(0, 0, 2, 46, 8)).Startup();
        }


        /// <summary>
        /// 输出日志并写入日志文件
        /// </summary>
        /// <param name="logFilePath">日志文件物理路径</param>
        /// <param name="logType">日志类型，枚举值</param>
        /// <param name="exeMethodName">被调用者执行的方法名</param>
        /// <param name="methodExeTime">方法执行的时间</param>
        /// <param name="customMsg">自定义消息</param>
        /// <param name="sysMsg">系统消息</param>
        /// <param name="addContent">附加内容</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        private static void WriteLog(string logFilePath,
                                     ELogType logType,
                                     string exeMethodName,
                                     string methodExeTime,
                                     string customMsg,
                                     string sysMsg = null,
                                     string addContent = null,
                                     string methodExeIdNum = null)
        {
            try
            {
                StringBuilder logContent = new StringBuilder();

                if (string.IsNullOrWhiteSpace(methodExeTime))
                {
                    methodExeTime = DateTime.Now.FormatCorrectToMs6();
                }

                Task.Factory.StartNew(logInfo =>
                {
                    try
                    {
                        LogWritingExe(logInfo as Dictionary<string, object>);
                    }
                    catch (Exception)
                    {
                        // 不做处理
                    }
                }, new Dictionary<string, object>()
                {
                    ["LogType"] = logType.ToString(),
                    ["ExeMethodName"] = exeMethodName,
                    ["MethodExeTime"] = Convert.ToDateTime(methodExeTime),
                    ["CustomMsg"] = customMsg?.Trim(),
                    ["SystemMsg"] = sysMsg?.Trim(),
                    ["AddContent"] = addContent?.Trim(),
                    ["MethodExeIdNum"] = methodExeIdNum
                });

                #region 日志内容
                int _length = 60;

                logContent.AppendLine();
                logContent.Append($"- -  {methodExeTime} - - -");
                for (int i = 0; i < _length; i++)
                {
                    logContent.Append(" -");
                }
                logContent.AppendLine();

                logContent.AppendLine("-");

                if (!string.IsNullOrWhiteSpace(methodExeIdNum))
                {
                    logContent.AppendLine($"方法执行时的标识号： {methodExeIdNum}");
                    logContent.AppendLine("-");
                }

                if (!string.IsNullOrWhiteSpace(exeMethodName))
                {
                    logContent.AppendLine($"{exeMethodName} 被执行");
                    logContent.AppendLine("-");
                }

                switch (logType)
                {
                    default: logContent.AppendLine($"日志类型： 未知的"); break;
                    case ELogType.INFO: logContent.AppendLine("日志类型： 记录"); break;
                    case ELogType.EX: logContent.AppendLine("日志类型： 异常"); break;
                    case ELogType.WARN: logContent.AppendLine("日志类型： 警告"); break;
                    case ELogType.SQL: logContent.AppendLine("日志类型： SQL 记录"); break;
                    case ELogType.FLOW: logContent.AppendLine("日志类型： 流程性"); break;
                }
                logContent.AppendLine("-");

                logContent.AppendLine("自定义消息：");
                logContent.AppendLine(customMsg?.Trim());
                logContent.AppendLine("-");

                if (!string.IsNullOrWhiteSpace(sysMsg))
                {
                    logContent.AppendLine("系统消息：");
                    logContent.AppendLine(sysMsg?.Trim());
                    logContent.AppendLine("-");
                }

                if (!string.IsNullOrWhiteSpace(addContent))
                {
                    logContent.AppendLine("附加内容：");
                    logContent.AppendLine(addContent?.Trim());
                    logContent.AppendLine("-");
                }

                logContent.Append("- - - - - - - - - - - - - - - - - - -");
                for (int i = 0; i < _length; i++)
                {
                    logContent.Append(" -");
                }
                logContent.AppendLine();

                logContent.AppendLine();
                #endregion

                FilesTool.WriteFileAppend(logFilePath, logContent.ToString());
            }
            catch (Exception)
            {
                //忽略异常 不做任何处理
            }
        }


        /// <summary>
        /// 写入【记录性 INFO】日志
        /// </summary>
        /// <param name="customMsg">自定义消息</param>
        /// <param name="addContent">附加内容</param>
        /// <param name="callerMethodName">该方法 调用者的 完全限定名</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        public static void WriteINFOLog(string customMsg,
                                        string addContent = null,
                                        string callerMethodName = null,
                                        string methodExeIdNum = null)
        {
            if (string.IsNullOrWhiteSpace(callerMethodName))
            {
                callerMethodName = GetCallerMethodName(1);
            }

            string methodExeTime = DateTime.Now.FormatCorrectToMs6();

            _ = Task.Run(() =>
            {
                lock (_writeingINFO)
                {
                    if (CheckLogFileBeyondMaxSize(INFOLogPath))
                    {
                        return;
                    }
                    WriteLog(INFOLogPath, ELogType.INFO, callerMethodName, methodExeTime, customMsg, null, addContent, methodExeIdNum);
                }
            });
        }

        /// <summary>
        /// 写入【异常性 EX】日志
        /// </summary>
        /// <param name="customMsg">自定义消息</param>
        /// <param name="sysMsg">系统消息（一般由 Exception 返回）</param>
        /// <param name="addContent">附加内容</param>
        /// <param name="callerMethodName">该方法 调用者的 完全限定名</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        public static void WriteEXLog(string customMsg,
                                      string sysMsg = null,
                                      string addContent = null,
                                      string callerMethodName = null,
                                      string methodExeIdNum = null)
        {
            if (string.IsNullOrWhiteSpace(callerMethodName))
            {
                callerMethodName = GetCallerMethodName(1);
            }

            string methodExeTime = DateTime.Now.FormatCorrectToMs6();

            _ = Task.Run(() =>
            {
                lock (_writeingEX)
                {
                    if (CheckLogFileBeyondMaxSize(EXLogPath))
                    {
                        return;
                    }
                    WriteLog(EXLogPath, ELogType.EX, callerMethodName, methodExeTime, customMsg, sysMsg, addContent, methodExeIdNum);
                }
            });
        }

        /// <summary>
        /// 写入【异常性 EX】日志
        /// </summary>
        /// <param name="customMsg">自定义消息</param>
        /// <param name="sysMsg">Exception 对象，catch {} 处 获取</param>
        /// <param name="addContent">附加内容</param>
        /// <param name="callerMethodName">该方法 调用者的 完全限定名</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        public static void WriteEXLog(string customMsg,
                                      Exception sysMsg,
                                      string addContent = null,
                                      string callerMethodName = null,
                                      string methodExeIdNum = null)
        {
            if (string.IsNullOrWhiteSpace(callerMethodName))
            {
                callerMethodName = GetCallerMethodName(1);
            }

            string methodExeTime = DateTime.Now.FormatCorrectToMs6();

            _ = Task.Run(() =>
            {
                lock (_writeingEX)
                {
                    if (CheckLogFileBeyondMaxSize(EXLogPath))
                    {
                        return;
                    }
                    WriteLog(EXLogPath, ELogType.EX, callerMethodName, methodExeTime, customMsg, sysMsg.ToString(), addContent, methodExeIdNum);
                }
            });
        }

        /// <summary>
        /// 写入【警告性 WARN】日志
        /// </summary>
        /// <param name="customMsg">自定义消息</param>
        /// <param name="addContent">附加内容；也可以是系统消息，由 Exception 返回</param>
        /// <param name="callerMethodName">该方法 调用者的 完全限定名</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        public static void WriteWARNLog(string customMsg,
                                        string addContent = null,
                                        string callerMethodName = null,
                                        string methodExeIdNum = null)
        {
            if (string.IsNullOrWhiteSpace(callerMethodName))
            {
                callerMethodName = GetCallerMethodName(1);
            }

            string methodExeTime = DateTime.Now.FormatCorrectToMs6();

            _ = Task.Run(() =>
            {
                lock (_writeingWARN)
                {
                    if (CheckLogFileBeyondMaxSize(WARNLogPath))
                    {
                        return;
                    }
                    WriteLog(WARNLogPath, ELogType.WARN, callerMethodName, methodExeTime, customMsg, null, addContent, methodExeIdNum);
                }
            });
        }

        /// <summary>
        /// 写入【警告性 WARN】日志
        /// </summary>
        /// <param name="customMsg">自定义消息</param>
        /// <param name="addContent">附加内容；Exception 对象，catch {} 处 获取</param>
        /// <param name="callerMethodName">该方法 调用者的 完全限定名</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        public static void WriteWARNLog(string customMsg,
                                        Exception addContent,
                                        string callerMethodName = null,
                                        string methodExeIdNum = null)
        {
            if (string.IsNullOrWhiteSpace(callerMethodName))
            {
                callerMethodName = GetCallerMethodName(1);
            }

            string methodExeTime = DateTime.Now.FormatCorrectToMs6();

            _ = Task.Run(() =>
            {
                lock (_writeingWARN)
                {
                    if (CheckLogFileBeyondMaxSize(WARNLogPath))
                    {
                        return;
                    }
                    WriteLog(WARNLogPath, ELogType.WARN, callerMethodName, methodExeTime, customMsg, null, addContent.ToString(), methodExeIdNum);
                }
            });
        }

        /// <summary>
        /// 写入【运行时 SQL】日志
        /// </summary>
        /// <param name="sysMsg">系统消息（一般是执行的 sql 语句）</param>
        /// <param name="addContent">附加内容（一般是执行 sql 语句的参数）</param>
        /// <param name="callerMethodName">该方法 调用者的 完全限定名</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        public static void WriteSQLLog(string sysMsg = null,
                                       string addContent = null,
                                       string callerMethodName = null,
                                       string methodExeIdNum = null)
        {
            if (string.IsNullOrWhiteSpace(callerMethodName))
            {
                callerMethodName = GetCallerMethodName(1);
            }

            string methodExeTime = DateTime.Now.FormatCorrectToMs6();

            _ = Task.Run(() =>
            {
                lock (_writeingSQL)
                {
                    if (CheckLogFileBeyondMaxSize(SQLLogPath))
                    {
                        return;
                    }
                    WriteLog(SQLLogPath, ELogType.SQL, callerMethodName, methodExeTime, "运行时 SQL 记录", sysMsg, addContent, methodExeIdNum);
                }
            });
        }


        /// <summary>
        /// 写入【流程性 FLOW】日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="callerMethodName">该方法 调用者的 完全限定名</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        public static void WriteFLOWLog(string msg,
                                        string callerMethodName = null,
                                        string methodExeIdNum = null)
        {
            if (string.IsNullOrWhiteSpace(callerMethodName))
            {
                callerMethodName = GetCallerMethodName(1);
            }

            string methodExeTime = DateTime.Now.FormatCorrectToMs6();

            _ = Task.Run(() =>
            {
                lock (_writeingFLOW)
                {
                    if (CheckLogFileBeyondMaxSize(FLOWLogPath))
                    {
                        return;
                    }
                    WriteLog(FLOWLogPath, ELogType.FLOW, callerMethodName, methodExeTime, msg, null, null, methodExeIdNum);
                }
            });
        }


        /// <summary>
        /// 获取 该方法 调用者的 完全限定名
        /// </summary>
        /// <param name="frameIndex">调用者级别；默认值：0。0 代表当前调用者级别，1 代表上级，2 代表上上级，以此类推。</param>
        /// <returns></returns>
        public static string GetCallerMethodName(int frameIndex = 0)
        {
            try
            {
                // GetFrame(1) 1代表上级，2代表上上级，以此类推
                MethodBase method = new StackTrace().GetFrame(frameIndex + 1).GetMethod();
                //获取被调用者执行的方法完全限定名
                string exeMethodName = $"{method.ReflectedType.FullName}.{method.Name}()";
                return exeMethodName;
            }
            catch (Exception)
            {
                return "[ 未获取到被执行方法的完全限定名 ]";
            }
        }


        /// <summary>
        /// 控制台输出日志，并且写入本地日志文件（默认：流程性日志）
        /// </summary>
        /// <param name="msg">日志消息内容</param>
        /// <param name="isConsole">是否在控制台输出，默认：是 true</param>
        /// <param name="logType">日志类型，默认 FLOW 流程性</param>
        /// <param name="methodExeIdNum">方法执行时的标识号，当一个方法内需要记录两次及两次以上日志时可用到</param>
        public static void ConsoleLog(string msg, bool isConsole = true, ELogType logType = ELogType.FLOW, string methodExeIdNum = null)
        {
            if (isConsole)
            {
                Console.WriteLine(msg);
            }

            string callerMethodName = GetCallerMethodName(1);

            switch (logType)
            {
                case ELogType.INFO:
                    WriteINFOLog(msg, null, callerMethodName, methodExeIdNum);
                    break;

                case ELogType.EX:
                    WriteEXLog(msg, string.Empty, null, callerMethodName, methodExeIdNum);
                    break;

                case ELogType.WARN:
                    WriteWARNLog(msg, string.Empty, callerMethodName, methodExeIdNum);
                    break;

                case ELogType.SQL:
                    WriteSQLLog(msg, null, callerMethodName, methodExeIdNum);
                    break;

                case ELogType.FLOW:
                default:
                    WriteFLOWLog(msg, callerMethodName, methodExeIdNum);
                    break;
            }
        }
    }
}
