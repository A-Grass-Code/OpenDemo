using IZhy.Common.BasicTools;
using IZhy.Common.SimpleTools;
using System;
using System.Reflection;

namespace IZhy.Common.AopProxy
{
    /// <summary>
    /// AOP 代理 通用的装饰器 抽象类 【目前只应用于接口的方法】
    /// <para>注意：凡直接或间接继承 DispatchProxy 类的派生类，不可修饰为密封的【不可用 sealed 修饰符】</para>
    /// </summary>
    public abstract class GeneralDecorator : DispatchProxy
    {
        /// <summary>
        /// 创建 装饰器
        /// </summary>
        /// <typeparam name="TI">目标接口（ 接口类型 ）</typeparam>
        /// <typeparam name="TC">目标接口 TI 的 具体实现类（ 类 类型 ）</typeparam>
        /// <typeparam name="TProxy">AOP 代理 装饰器 实现类，该类型必须直接或间接继承 DispatchProxy；
        /// <para>注意：此类中必须包含 public 修饰的无参构造函数</para>
        /// </typeparam>
        /// <param name="targetImpClass">目标接口 TI 的 具体实现类 对象</param>
        /// <param name="isThrowTargetMethodEx">是否抛出目标方法执行时所引发的异常，默认 false</param>
        /// <param name="isExeWhenException">是否执行 WhenException() 方法，默认 true</param>
        /// <returns></returns>
        public static TI CreateDecorator<TI, TC, TProxy>(TC targetImpClass, bool isThrowTargetMethodEx = false, bool isExeWhenException = true)
            where TProxy : DispatchProxy, new()
            where TC : class, TI
        {
            TI decorator = Create<TI, TProxy>();
            (decorator as GeneralDecorator).TargetFullName = typeof(TI)?.FullName;
            (decorator as GeneralDecorator).TargetImpClass = targetImpClass;
            (decorator as GeneralDecorator).IsThrowTargetMethodEx = isThrowTargetMethodEx;
            (decorator as GeneralDecorator).IsExeWhenException = isExeWhenException;
            return decorator;
        }


        /// <summary>
        /// 目标类，即 具体的实现类
        /// </summary>
        protected object TargetImpClass { get; private set; }

        /// <summary>
        /// 目标（ 接口 ）的完全限定名
        /// </summary>
        protected string TargetFullName { get; private set; }

        /// <summary>
        /// 目标实现类的完全限定名
        /// </summary>
        protected string TargetImpClassFullName => TargetImpClass?.GetType()?.FullName;

        /// <summary>
        /// 是否抛出目标方法执行时所引发的异常，默认 false
        /// </summary>
        protected bool IsThrowTargetMethodEx { get; set; } = false;

        /// <summary>
        /// 是否执行 WhenException() 方法，默认 true
        /// </summary>
        protected bool IsExeWhenException { get; set; } = true;


        /// <summary>
        /// 在执行之前
        /// </summary>
        /// <param name="method">目标的方法信息</param>
        /// <param name="paramArr">目标的参数
        /// <para>
        /// 调用方法或构造函数的参数列表。
        /// 此对象数组在数量、顺序和类型方面与调用的方法或构造函数的参数相同。
        /// 如果不存在任何参数，则 parameters 应为 null。
        /// </para>
        /// </param>
        /// <param name="exeIdNum">方法执行时的标识号（方便在日志中查找）</param>
        protected virtual void Before(MethodInfo method, object[] paramArr, string exeIdNum) { }

        /// <summary>
        /// 执行结束并成功时
        /// </summary>
        /// <param name="method">目标的方法信息</param>
        /// <param name="paramArr">目标的参数
        /// <para>
        /// 调用方法或构造函数的参数列表。
        /// 此对象数组在数量、顺序和类型方面与调用的方法或构造函数的参数相同。
        /// 如果不存在任何参数，则 parameters 应为 null。
        /// </para>
        /// </param>
        /// <param name="result">目标的返回结果</param>
        /// <param name="exeIdNum">方法执行时的标识号（方便在日志中查找）</param>
        protected virtual void Successful(MethodInfo method, object[] paramArr, object result, string exeIdNum) { }

        /// <summary>
        /// 执行完成时，不论结果是 成功 还是 失败 又或是 异常
        /// </summary>
        /// <param name="method">目标的方法信息</param>
        /// <param name="paramArr">目标的参数
        /// <para>
        /// 调用方法或构造函数的参数列表。
        /// 此对象数组在数量、顺序和类型方面与调用的方法或构造函数的参数相同。
        /// 如果不存在任何参数，则 parameters 应为 null。
        /// </para>
        /// </param>
        /// <param name="result">目标的返回结果</param>
        /// <param name="exeIdNum">方法执行时的标识号（方便在日志中查找）</param>
        protected virtual void Completion(MethodInfo method, object[] paramArr, object result, string exeIdNum) { }

        /// <summary>
        /// 执行中发生异常时
        /// </summary>
        /// <param name="method">目标的方法信息</param>
        /// <param name="paramArr">目标的参数
        /// <para>
        /// 调用方法或构造函数的参数列表。
        /// 此对象数组在数量、顺序和类型方面与调用的方法或构造函数的参数相同。
        /// 如果不存在任何参数，则 parameters 应为 null。
        /// </para>
        /// </param>
        /// <param name="ex">目标发生的异常</param>
        /// <param name="exeIdNum">方法执行时的标识号（方便在日志中查找）</param>
        protected virtual void WhenException(MethodInfo method, object[] paramArr, Exception ex, string exeIdNum) { }


        /// <summary>
        /// 每当调用生成的代理类型上的任何方法时，都会调用该方法来调度控制。（ 你可以认为是拦截者调用 ）
        /// </summary>
        /// <param name="targetMethod">目标的方法信息</param>
        /// <param name="args">目标的参数
        /// <para>
        /// 调用方法或构造函数的参数列表。
        /// 此对象数组在数量、顺序和类型方面与调用的方法或构造函数的参数相同。
        /// 如果不存在任何参数，则 parameters 应为 null。
        /// </para>
        /// </param>
        /// <returns>当发生异常时，返回 null</returns>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod.CustomAttributes != null)
            {
                foreach (var item in targetMethod.CustomAttributes)
                {
                    if (item.AttributeType.Name == nameof(IgnoreDecoratorAttribute))
                    {
                        return targetMethod.Invoke(TargetImpClass, args);
                    }
                }
            }

            string exeIdNum = RandomNumTool.NOTimePlusLetterAndNumber();
            object result = default(object); // 返回结果

            try
            {
                Before(targetMethod, args, exeIdNum);
            }
            catch (Exception ex)
            {
                LogsTool.WriteEXLog($"{TargetImpClassFullName}.{targetMethod.Name}() 方法在执行时，装饰器 Before() 函数 发生异常", ex);
            }

            try
            {
                result = targetMethod.Invoke(TargetImpClass, args);

                try
                {
                    Successful(targetMethod, args, result, exeIdNum);
                }
                catch (Exception ex)
                {
                    LogsTool.WriteEXLog($"{TargetImpClassFullName}.{targetMethod.Name}() 方法在执行时，装饰器 Successful() 函数 发生异常", ex);
                }

                return result;
            }
            catch (Exception ex)
            {
                ex = ex.InnerException ?? ex;

                if (IsExeWhenException)
                {
                    try
                    {
                        WhenException(targetMethod, args, ex, exeIdNum);
                    }
                    catch (Exception exx)
                    {
                        LogsTool.WriteEXLog($"{TargetImpClassFullName}.{targetMethod.Name}() 方法在执行时，装饰器 WhenException() 函数 发生异常", exx);
                    }
                }

                if (IsThrowTargetMethodEx)
                {
                    throw new Exception($"装饰器调用被执行方法时，被执行方法抛出了异常", ex);
                }

                return default(object);
            }
            finally
            {
                try
                {
                    Completion(targetMethod, args, result, exeIdNum);
                }
                catch (Exception ex)
                {
                    LogsTool.WriteEXLog($"{TargetImpClassFullName}.{targetMethod.Name}() 方法在执行时，装饰器 Completion() 函数 发生异常", ex);
                }
            }
        }
    }
}
