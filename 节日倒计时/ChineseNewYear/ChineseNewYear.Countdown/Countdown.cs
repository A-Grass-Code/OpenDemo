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
    internal static class Countdown
    {
        /// <summary>
        /// FestivalInfo.dat 节日信息 文件绝对路径
        /// </summary>
        public static readonly string ConfigPath = $"{FilesTool.ProgramRootDirectory}FestivalInfo.dat";

        /// <summary>
        /// 获取节日信息（ 节日名称 ，日期时间 ）
        /// </summary>
        /// <returns></returns>
        public static (string FestivalName, DateTime FestivalDate) GetFestivalInfo()
        {
            try
            {
                var festivalInfo = FilesTool.ReadFileToAll(ConfigPath).Split(new string[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);
                return (festivalInfo[0].Trim(), Convert.ToDateTime(festivalInfo[1].Trim()));
            }
            catch (Exception)
            {
                return (string.Empty, default(DateTime));
            }
        }
    }
}
