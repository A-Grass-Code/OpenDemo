﻿using IZhy.Common.SimpleTools;
using System;

namespace IZhy.Common.BasicTools
{
    /// <summary>
    /// GC 回收处理器 初始化类；用在程序启动处
    /// </summary>
    public static class GcProcessor
    {
        private static bool _isRunning = false;

        private static readonly object _lock = new object();

        /// <summary>
        /// GC 回收器 初始化； 默认 每 45 秒 执行一次 GC.Collect();
        /// </summary>
        /// <param name="intervalTime">间隔时间，单位秒；默认 45 s</param>
        public static void Init(int intervalTime = 45)
        {
            lock (_lock)
            {
                if (_isRunning)
                {
                    return;
                }
                else
                {
                    _isRunning = true;
                    new TimedIntervalTask(() =>
                    {
                        GC.Collect();
                    }, 1000 * intervalTime, false).Startup();
                }
            }
        }
    }
}
