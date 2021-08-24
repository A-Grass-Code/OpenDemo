using System;
using System.Threading.Tasks;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 时间定点模式的定时任务类
    /// <para>定点精确到秒级</para>
    /// </summary>
    public sealed class TimedFixedTask
    {
        /// <summary>
        /// 初始化 时间定点模式的定时任务类；须给定一些必要参数
        /// </summary>
        /// <param name="action">
        /// 定时任务的方法行为，不可为 null；此方法里须加入 try {} catch {} 异常处理，以保证其稳定运行
        /// </param>
        /// <param name="timePeriod">重复的时间周期，每月、每天、每周 等</param>
        /// <param name="fixedTime">定点时间</param>
        public TimedFixedTask(Action action, ERepetitionTimePeriod timePeriod, FixedTimed fixedTime)
        {
            TimedAction = action;
            TimePeriod = timePeriod;
            Fixedtime = fixedTime;
            Restore();
        }

        /// <summary>
        /// 还原
        /// </summary>
        private void Restore()
        {
            IsPause = false;
            IsStop = false;
            IsRunning = false;
            IsStarted = false;
        }

        /// <summary>
        /// 定时的行为
        /// </summary>
        private Action TimedAction { get; set; }

        /// <summary>
        /// 是否暂停行为，默认 false
        /// <para>
        /// 若为 true，则暂停定时行为的运行；若再设为 false，则继续定时行为的运行
        /// </para>
        /// </summary>
        public bool IsPause { get; private set; } = false;

        /// <summary>
        /// 是否终止定时任务，默认 false
        /// </summary>
        public bool IsStop { get; private set; } = false;

        /// <summary>
        /// 是否已经启动，true 已启动，false 未启动；默认 false
        /// </summary>
        public bool IsStarted { get; private set; } = false;

        /// <summary>
        /// 重复的时间周期，每月、每天、每周 等
        /// </summary>
        private ERepetitionTimePeriod TimePeriod { get; set; }

        /// <summary>
        /// 定点时间
        /// </summary>
        private FixedTimed Fixedtime { get; set; }

        /// <summary>
        /// 定时任务是否在运行中；true 是，false 否；默认 false
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// 启动任务运行
        /// <para>注意：再次调用此方法需要先执行 Stop() 终止定时任务</para>
        /// </summary>
        public void Startup()
        {
            if (TimedAction == null || IsStarted)
            {
                return;
            }

            Task.Run(async () =>
            {
                IsStarted = true;

                while (!IsStop)
                {
                    if (!IsPause)
                    {
                        IsRunning = true;
                        DateTime time = DateTime.Now;
                        switch (TimePeriod)
                        {
                            case ERepetitionTimePeriod.Yearly:
                                if ($"{time.Month}{time.Day}{time.Hour}{time.Minute}{time.Second}"
                                    == $"{Fixedtime.Month}{Fixedtime.Day}{Fixedtime.Hour}{Fixedtime.Minute}{Fixedtime.Second}")
                                {
                                    _ = Task.Run(() =>
                                    {
                                        try
                                        {
                                            TimedAction();
                                        }
                                        catch (Exception)
                                        {
                                            // 不做处理
                                        }
                                    });
                                    await Task.Delay(1000);
                                }
                                break;

                            case ERepetitionTimePeriod.Monthly:
                                if ($"{time.Day}{time.Hour}{time.Minute}{time.Second}"
                                    == $"{Fixedtime.Day}{Fixedtime.Hour}{Fixedtime.Minute}{Fixedtime.Second}")
                                {
                                    _ = Task.Run(() =>
                                    {
                                        try
                                        {
                                            TimedAction();
                                        }
                                        catch (Exception)
                                        {
                                            // 不做处理
                                        }
                                    });
                                    await Task.Delay(1000);
                                }
                                break;

                            case ERepetitionTimePeriod.Daily:
                                if ($"{time.Hour}{time.Minute}{time.Second}"
                                    == $"{Fixedtime.Hour}{Fixedtime.Minute}{Fixedtime.Second}")
                                {
                                    _ = Task.Run(() =>
                                    {
                                        try
                                        {
                                            TimedAction();
                                        }
                                        catch (Exception)
                                        {
                                            // 不做处理
                                        }
                                    });
                                    await Task.Delay(1000);
                                }
                                break;

                            case ERepetitionTimePeriod.Hourly:
                                if ($"{time.Minute}{time.Second}"
                                    == $"{Fixedtime.Minute}{Fixedtime.Second}")
                                {
                                    _ = Task.Run(() =>
                                    {
                                        try
                                        {
                                            TimedAction();
                                        }
                                        catch (Exception)
                                        {
                                            // 不做处理
                                        }
                                    });
                                    await Task.Delay(1000);
                                }
                                break;

                            case ERepetitionTimePeriod.Minutely:
                                if ($"{time.Second}"
                                    == $"{Fixedtime.Second}")
                                {
                                    _ = Task.Run(() =>
                                    {
                                        try
                                        {
                                            TimedAction();
                                        }
                                        catch (Exception)
                                        {
                                            // 不做处理
                                        }
                                    });
                                    await Task.Delay(1000);
                                }
                                break;

                            case ERepetitionTimePeriod.Weekly:
                                if ($"{time.DayOfWeek}{time.Hour}{time.Minute}{time.Second}"
                                    == $"{Fixedtime.Week}{Fixedtime.Hour}{Fixedtime.Minute}{Fixedtime.Second}")
                                {
                                    _ = Task.Run(() =>
                                    {
                                        try
                                        {
                                            TimedAction();
                                        }
                                        catch (Exception)
                                        {
                                            // 不做处理
                                        }
                                    });
                                    await Task.Delay(1000);
                                }
                                break;
                            default:
                                throw new Exception("ERepetitionTimePeriod 枚举值无效");
                        }
                    }
                    else
                    {
                        IsRunning = false;
                    }
                    await Task.Delay(200);
                }

                Restore();
            });
        }

        /// <summary>
        /// 终止定时任务
        /// <para>注意：调用该方法后，定时任务会在下一个运行周期终止定时任务，这可能会产生一个等待期；</para>
        /// <para>可通过 IsStarted 属性判断，当值为 false 时（未启动），则表示已终止定时任务，可通过 Startup() 重新启动运行</para>
        /// </summary>
        public void Stop()
        {
            IsStop = true;
        }

        /// <summary>
        /// 暂停定时任务
        /// </summary>
        public void Pause()
        {
            IsPause = true;
        }

        /// <summary>
        /// 继续定时任务
        /// </summary>
        public void GoOn()
        {
            IsPause = false;
        }
    }


    /// <summary>
    /// 枚举类 重复的时间周期，每月、每天、每周 等 （ 该枚举类应当配合 FixedTimed 结构体使用 ）
    /// </summary>
    public enum ERepetitionTimePeriod
    {
        /// <summary>
        /// 每年
        /// </summary>
        Yearly = 1,

        /// <summary>
        /// 每月
        /// </summary>
        Monthly,

        /// <summary>
        /// 每日
        /// </summary>
        Daily,

        /// <summary>
        /// 每时
        /// </summary>
        Hourly,

        /// <summary>
        /// 每分
        /// </summary>
        Minutely,

        /// <summary>
        /// 每周
        /// </summary>
        Weekly
    }

    /// <summary>
    /// 定点时间 结构体 （ 该结构体应当配合 ERepetitionTimePeriod 枚举类使用 ）
    /// </summary>
    public struct FixedTimed
    {
        /// <summary>
        /// 定点时刻（ 参数：月、天、时、分、秒 ）
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public FixedTimed(int month, int day, int hour, int minute, int second)
        {
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Week = DayOfWeek.Monday;
        }

        /// <summary>
        /// 定点时刻（ 参数：星期、时、分、秒 ）
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public FixedTimed(DayOfWeek week, int hour, int minute, int second)
        {
            Month = 0;
            Day = 0;
            Hour = hour;
            Minute = minute;
            Second = second;
            Week = week;
        }


        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 时
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// 分
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// 秒
        /// </summary>
        public int Second { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public DayOfWeek Week { get; set; }


        public override string ToString()
        {
            return $"0000-{Month}-{Day} {Week} {Hour}:{Minute}:{Second}";
        }
    }
}
