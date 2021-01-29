using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseNewYear.Countdown
{
    /// <summary>
    /// 倒计时 类
    /// </summary>
    public static class Countdown
    {
        /// <summary>
        /// 用来存放 NextChineseNewYearDate.txt 配置文件 的 最后修改时间
        /// </summary>
        public static DateTime ConfigLastChangeTime { get; set; }

        /// <summary>
        /// 获取一个 bool 值，表示 NextChineseNewYearDate.txt 配置文件 是否已被重新修改；true/false 是/否
        /// </summary>
        private static bool IsChangeConfigFile
        {
            get
            {
                if (!File.Exists(FilesTool.ConfigPath))
                {
                    throw new Exception($"NextChineseNewYearDate.txt 配置文件不存在，请检查 => [ {FilesTool.ConfigPath} ]");
                }

                try
                {
                    FileInfo fileInfo = new FileInfo(FilesTool.ConfigPath);
                    if (fileInfo.LastWriteTime != ConfigLastChangeTime)
                    {
                        ConfigLastChangeTime = fileInfo.LastWriteTime;
                        return true;
                    }
                }
                catch (Exception)
                {
                    return true;
                }
                return false;
            }
        }


        private static DateTime _nextChineseNewYearDate;
        /// <summary>
        /// 下一次春节日期
        /// </summary>
        public static DateTime NextChineseNewYearDate
        {
            get
            {
                if (IsChangeConfigFile)
                {
                    _nextChineseNewYearDate = Convert.ToDateTime(FilesTool.ReadFileToAll(FilesTool.ConfigPath));
                }
                return _nextChineseNewYearDate;
            }
        }
    }
}
