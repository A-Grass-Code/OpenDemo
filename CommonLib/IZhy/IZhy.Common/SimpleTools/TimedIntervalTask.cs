using System;
using System.Threading.Tasks;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 时间间隔模式的定时任务类
    /// </summary>
    public sealed class TimedIntervalTask
    {
        /// <summary>
        /// 初始化 时间间隔模式的定时任务类；须给定一些必要参数
        /// </summary>
        /// <param name="action">
        /// 定时任务的方法行为，不可为 null；此方法里须加入 try {} catch {} 异常处理，以保证其稳定运行
        /// </param>
        /// <param name="intervalTime">定时任务的间隔时间，单位：毫秒</param>
        /// <param name="whetherToRunFirst">
        /// 是否先运行行为；默认为 true，先运行行为，再等待时间间隔；若设为 false，则先等待时间间隔，再运行行为
        /// </param>
        public TimedIntervalTask(Action action, int intervalTime, bool whetherToRunFirst = true)
        {
            TimedAction = action;
            IntervalTime = intervalTime;
            WhetherToRunFirst = whetherToRunFirst;
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
        /// 行为运行的间隔时间，单位：毫秒
        /// </summary>
        public int IntervalTime { get; set; }

        /// <summary>
        /// 是否先运行 行为，默认 true
        /// <para>
        /// 若为 true，则先运行行为，再等待时间间隔；若设为 false，则先等待时间间隔，再运行行为
        /// </para>
        /// </summary>
        public bool WhetherToRunFirst { get; set; }

        /// <summary>
        /// 是否已经启动，true 已启动，false 未启动；默认 false
        /// </summary>
        public bool IsStarted { get; private set; } = false;

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
                    IntervalTime = IntervalTime < 1 ? 1 : IntervalTime;
                    if (IsPause)
                    {
                        IsRunning = false;
                        await Task.Delay(IntervalTime);
                        continue;
                    }
                    else
                    {
                        IsRunning = true;
                    }

                    if (WhetherToRunFirst)
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
                        await Task.Delay(IntervalTime);
                    }
                    else
                    {
                        await Task.Delay(IntervalTime);
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
                    }
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
}
