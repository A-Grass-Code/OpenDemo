using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 自动功能控制类
    /// </summary>
    public sealed class AutoActionControl
    {
        /// <summary>
        /// AutoActionControl 无参构造函数
        /// </summary>
        public AutoActionControl() { }

        /// <summary>
        /// AutoActionControl 构造函数
        /// </summary>
        /// <param name="action">
        /// <para>Func 委托，功能函数；传入一个 object 参数，返回一个 Task 结果</para>
        /// <para>注：这个函数最好加上 try{} catch (Exception ex) { //这里可以写异常日志 }</para>
        /// </param>
        public AutoActionControl(Func<object, Task> action)
        {
            AutoAction = action;
        }

        /// <summary>
        /// AutoActionControl 构造函数
        /// </summary>
        /// <param name="action">
        /// <para>Func 委托，功能函数；传入一个 object 参数，返回一个 Task 结果</para>
        /// <para>注：这个函数最好加上 try{} catch (Exception ex) { //这里可以写异常日志 }</para>
        /// </param>
        /// <param name="paramsList">
        /// <para>参数集合 【 AutoAction 所需要的参数 】</para>
        /// <para>当 AutoAction 功能函数不需要参数时，此属性赋值为 null （也可以不用设置，默认就是 null 值）</para>
        /// <para>
        /// 当 MaxExecuteCount != 0 时（有限执行），且 AutoAction 功能函数需要参数，
        /// 此属性赋值为一个参数集合，集合的元素个数（Count 值）须等于 MaxExecuteCount
        /// </para>
        /// <para>当 MaxExecuteCount == 0 时（无限执行），此属性无意义；这种情况应当直接把“传参过程”写在 AutoAction 功能函数里</para>
        /// </param>
        public AutoActionControl(Func<object, Task> action, List<object> paramsList)
        {
            AutoAction = action;
            ParamsList = paramsList;
        }

        /// <summary>
        /// AutoActionControl 构造函数
        /// </summary>
        /// <param name="action">
        /// <para>Func 委托，功能函数；传入一个 object 参数，返回一个 Task 结果</para>
        /// <para>注：这个函数最好加上 try{} catch (Exception ex) { //这里可以写异常日志 }</para>
        /// </param>
        /// <param name="paramsList">
        /// <para>参数集合 【 AutoAction 所需要的参数 】</para>
        /// <para>当 AutoAction 功能函数不需要参数时，此属性赋值为 null （也可以不用设置，默认就是 null 值）</para>
        /// <para>
        /// 当 MaxExecuteCount != 0 时（有限执行），且 AutoAction 功能函数需要参数，
        /// 此属性赋值为一个参数集合，集合的元素个数（Count 值）须等于 MaxExecuteCount
        /// </para>
        /// <para>当 MaxExecuteCount == 0 时（无限执行），此属性无意义；这种情况应当直接把“传参过程”写在 AutoAction 功能函数里</para>
        /// </param>
        /// <param name="maxExecuteCount">
        /// <para>最大执行数量；也就是 功能函数最多运行几次</para>
        /// <para>如果设为 0，则不计总量的一直运行；属性 RunAlready 和 RunCompleted 也将没有意义。</para>
        /// </param>
        /// <param name="simultaneousExecute">同时执行数量；也就是 最多同时运行几个功能函数</param>
        /// <param name="scanningFrequency">扫描频率，单位：毫秒（每多少毫秒扫描一次）</param>
        public AutoActionControl(Func<object, Task> action, List<object> paramsList,
                                 int maxExecuteCount, int simultaneousExecute, int scanningFrequency)
        {
            AutoAction = action;
            ParamsList = paramsList;
            MaxExecuteCount = maxExecuteCount;
            SimultaneousExecute = simultaneousExecute;
            ScanningFrequency = scanningFrequency;
        }


        /// <summary>
        /// 声明一个加锁对象，用来控制同时执行的任务数量
        /// </summary>
        private readonly object _lockTaskList = new object();

        /// <summary>
        /// Task 任务集合
        /// </summary>
        private readonly List<Task> _taskList = new List<Task>();

        /// <summary>
        /// 声明一个 bool 变量，用来指示是否停止任务的运行。true/false，是/否
        /// </summary>
        private bool _isStopTask = true;


        #region 核心属性

        /// <summary>
        /// 正在运行的数量（该属性值 只会大于等于 0 且 小于等于 SimultaneousExecute 值）
        /// </summary>
        public int RunningCount
        {
            get
            {
                return _taskList.Count;
            }
        }

        /// <summary>
        /// 指示自动功能是否已全部完成【在调用 StartExecute() 方法前调用该属性，无意义】
        /// </summary>
        public bool IsFinish
        {
            get
            {
                if (MaxExecuteCount != 0)
                {
                    return (RunCompleted >= MaxExecuteCount);
                }
                else
                {
                    return (_isStopTask == true && _taskList.Count == 0);
                }
            }
        }

        /// <summary>
        /// <para>Func 委托，功能函数；传入一个 object 参数，返回一个 Task 结果</para>
        /// <para>注：这个函数最好加上 try{} catch (Exception ex) { //这里可以写异常日志 }</para>
        /// </summary>
        public Func<object, Task> AutoAction { get; set; } = null;

        /// <summary>
        /// <para>参数集合 【 AutoAction 所需要的参数 】</para>
        /// <para>当 AutoAction 功能函数不需要参数时，此属性赋值为 null （也可以不用设置，默认就是 null 值）</para>
        /// <para>
        /// 当 MaxExecuteCount != 0 时（有限执行），且 AutoAction 功能函数需要参数，
        /// 此属性赋值为一个参数集合，集合的元素个数（Count 值）须等于 MaxExecuteCount
        /// </para>
        /// <para>当 MaxExecuteCount == 0 时（无限执行），此属性无意义；这种情况应当直接把“传参过程”写在 AutoAction 功能函数里</para>
        /// </summary>
        public List<object> ParamsList { get; set; } = null;



        private int runAlready = 0;

        /// <summary>
        /// <para>已经运行的数量（只要是已开始执行的任务，无需完成，该属性值就增加计数）</para>
        /// <para>当 MaxExecuteCount = 0 时，该属性无意义，返回 -1</para>
        /// </summary>
        public int RunAlready
        {
            get
            {
                return MaxExecuteCount != 0 ? runAlready : -1;
            }
            private set
            {
                runAlready = value;
            }
        }


        private int runCompleted = 0;

        /// <summary>
        /// <para>运行已完成的数量（只有运行已完成的任务，该属性值才会增加计数）</para>
        /// <para>当 MaxExecuteCount = 0 时，该属性无意义，返回 -1</para>
        /// </summary>
        public int RunCompleted
        {
            get
            {
                return MaxExecuteCount != 0 ? runCompleted : -1;
            }
            private set
            {
                runCompleted = value;
            }
        }


        private int maxExecuteCount = 100;

        /// <summary>
        /// <para>最大执行数量（默认 100 个）；也就是 功能函数最多运行几次</para>
        /// <para>如果设为 0，则不计总量的一直运行；属性 RunAlready 和 RunCompleted 也将没有意义。</para>
        /// </summary>
        public int MaxExecuteCount
        {
            get
            {
                return maxExecuteCount;
            }
            set
            {
                maxExecuteCount = value < 0 ? 100 : value;
            }
        }


        private int simultaneousExecute = 10;

        /// <summary>
        /// 同时执行数量（默认 10 个）；也就是 最多同时运行几个功能函数 （类似池的概念，有空闲的就自动加入下一个）
        /// </summary>
        public int SimultaneousExecute
        {
            get
            {
                if (simultaneousExecute > MaxExecuteCount)
                {
                    simultaneousExecute = MaxExecuteCount;
                }
                return simultaneousExecute;
            }
            set
            {
                simultaneousExecute = value < 1 ? 10 : value;
            }
        }


        private int scanningFrequency = 500;

        /// <summary>
        /// 扫描频率，单位：毫秒（每多少毫秒扫描一次）。默认：500
        /// </summary>
        public int ScanningFrequency
        {
            get
            {
                return scanningFrequency;
            }
            set
            {
                scanningFrequency = value < 1 ? 500 : value;
            }
        }

        #endregion


        /// <summary>
        /// 任务调度（控制同时执行数）
        /// </summary>
        private void TaskScheduling()
        {
            if (MaxExecuteCount != 0)
            {
                if (RunAlready >= MaxExecuteCount)
                {
                    _isStopTask = true;
                    if (_taskList.Count < 1)
                    {
                        // 已经执行完毕
                        return;
                    }
                }
            }

            if (_taskList.Count > 0)
            {
                if (_taskList.Count <= SimultaneousExecute)
                {
                    int i = 0;
                    while (i < _taskList.Count)
                    {
                        if (_taskList[i].IsCompleted)
                        {
                            _taskList[i].Dispose();
                            _taskList.Remove(_taskList[i]);
                            if (MaxExecuteCount != 0)
                            {
                                RunCompleted++;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }

                    JoinExecuteTaskAsync().Wait();
                }
            }
            else
            {
                JoinExecuteTaskAsync().Wait();
            }
        }

        /// <summary>
        /// 加入执行任务
        /// </summary>
        private async Task JoinExecuteTaskAsync()
        {
            if (!_isStopTask)
            {
                int residue = SimultaneousExecute - _taskList.Count; //剩余数
                if (MaxExecuteCount != 0)
                {
                    while (residue > 0 && RunAlready < MaxExecuteCount)
                    {
                        _taskList.Add(await Task.Factory.StartNew(AutoAction, ParamsList?[RunAlready]));
                        RunAlready++;
                        residue--;
                    }
                }
                else
                {
                    while (residue > 0)
                    {
                        _taskList.Add(await Task.Factory.StartNew(AutoAction, null));
                        residue--;
                    }
                }
            }
        }


        /// <summary>
        /// 启动执行（该方法会重新启动执行）
        /// </summary>
        public AutoActionControl StartExecute()
        {
            if (AutoAction == null)
            {
                throw new Exception("不存在自动功能。");
            }

            if (MaxExecuteCount != 0)
            {
                if (ParamsList != null && ParamsList.Count != MaxExecuteCount)
                {
                    throw new Exception("功能函数的参数集合不符合规格。");
                }
            }

            _isStopTask = false;
            RunAlready = 0;
            RunCompleted = 0;
            _taskList.Clear();

            Task.Run(async () =>
            {
                while (true)
                {
                    lock (_lockTaskList)
                    {
                        TaskScheduling();
                    }
                    if (_taskList.Count < 1)
                    {
                        break;
                    }
                    await Task.Delay(ScanningFrequency);
                }
            });

            return this;
        }

        /// <summary>
        /// 停止执行（正常情况会返回 true，无实际意义）
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StopExecuteAsync()
        {
            _isStopTask = true;

            await Task.Run(() =>
            {
                while (true)
                {
                    if (_taskList.Count < 1)
                    {
                        break;
                    }
                }
            });

            return true;
        }

        /// <summary>
        /// 继续执行
        /// </summary>
        public void GoOnExecute()
        {
            if (AutoAction == null)
            {
                throw new Exception("不存在自动功能。");
            }

            if (MaxExecuteCount != 0)
            {
                if (ParamsList != null && ParamsList.Count != MaxExecuteCount)
                {
                    throw new Exception("功能函数的参数集合不符合规格。");
                }

                if (RunAlready >= MaxExecuteCount)
                {
                    _isStopTask = true;
                    return;
                }
            }

            _isStopTask = false;

            Task.Run(async () =>
            {
                while (true)
                {
                    lock (_lockTaskList)
                    {
                        TaskScheduling();
                    }
                    if (_taskList.Count < 1)
                    {
                        break;
                    }
                    await Task.Delay(ScanningFrequency);
                }
            });
        }
    }
}
