using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseNewYear.Countdown
{
    /// <summary>
    /// 一个简单的日期时间工具类
    /// </summary>
    public static class DateTimeTool
    {
        /// <summary> 计算两个日期时间的间隔（返回一个动态对象，包含 年、月、日、时、分、秒 的间隔）
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间（必须大于开始时间）</param>
        /// <returns></returns>
        public static dynamic ComputeHowLong(DateTime beginTime, DateTime endTime)
        {
            //创建匿名对象
            dynamic HowLong = new System.Dynamic.ExpandoObject();

            if (endTime <= beginTime)
            {
                HowLong.cYear = 0;
                HowLong.cMonth = 0;
                HowLong.cDay = 0;
                HowLong.cHour = 0;
                HowLong.cMinute = 0;
                HowLong.cSecond = 0;

                HowLong.Days = 0;
                HowLong.Hours = 0;
                HowLong.Minutes = 0;
                HowLong.Seconds = 0;

                return HowLong;
            }

            #region 算法

            //// 结束日期时间
            int n_y = endTime.Year;
            int n_M = endTime.Month;
            int n_d = endTime.Day;
            int n_H = endTime.Hour;
            int n_m = endTime.Minute;
            int n_s = endTime.Second;

            //// 开始日期时间
            int b_y = beginTime.Year;
            int b_M = beginTime.Month;
            int b_d = beginTime.Day;
            int b_H = beginTime.Hour;
            int b_m = beginTime.Minute;
            int b_s = beginTime.Second;

            //// 相差日期时间
            int c_y;
            int c_M;
            int c_d;
            int c_H;
            int c_m;
            int c_s;

            //// 计算秒
            if (n_s < b_s)
            {
                c_s = n_s + 60 - b_s;
                n_m--;
            }
            else
            {
                c_s = n_s - b_s;
            }
            //// 计算分
            if (n_m < b_m)
            {
                c_m = n_m + 60 - b_m;
                n_H--;
            }
            else
            {
                c_m = n_m - b_m;
            }
            //// 计算时
            if (n_H < b_H)
            {
                c_H = n_H + 24 - b_H;
                n_d--;
            }
            else
            {
                c_H = n_H - b_H;
            }
            //// 计算天
            if (n_d < b_d)
            {
                switch (n_M - 1)
                {
                    case 0:
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        c_d = n_d + 31 - b_d;
                        break;
                    case 2:
                        if ((n_y % 4 == 0 && n_y % 100 != 0) || n_y % 400 == 0)
                        {
                            c_d = n_d + 29 - b_d;
                        }
                        else
                        {
                            c_d = n_d + 28 - b_d;
                        }
                        break;
                    default:
                        c_d = n_d + 30 - b_d;
                        break;
                };
                n_M--;
            }
            else
            {
                c_d = n_d - b_d;
            }
            //// 计算月
            if (n_M < b_M)
            {
                c_M = n_M + 12 - b_M;
                n_y--;
            }
            else
            {
                c_M = n_M - b_M;
            }
            //// 计算年
            c_y = n_y - b_y;

            if (c_d >= 30)
            {
                c_d = 0;
                c_M++;
            }

            if (c_M >= 12)
            {
                c_M = 0;
                c_y++;
            }

            #endregion

            HowLong.cYear = c_y;
            HowLong.cMonth = c_M;
            HowLong.cDay = c_d;
            HowLong.cHour = c_H;
            HowLong.cMinute = c_m;
            HowLong.cSecond = c_s;

            TimeSpan ts = endTime - beginTime;
            HowLong.Days = ts.Days;
            HowLong.Hours = ts.Hours;
            HowLong.Minutes = ts.Minutes;
            HowLong.Seconds = ts.Seconds;

            return HowLong;
        }


        /// <summary>
        /// DateTime 格式化为精确到毫秒6位数
        /// <para>格式： "yyyy-MM-dd HH:mm:ss.ffffff"</para>
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatCorrectToMs6(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
        }
    }
}
