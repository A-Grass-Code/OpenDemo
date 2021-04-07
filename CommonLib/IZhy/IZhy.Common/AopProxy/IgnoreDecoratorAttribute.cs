using System;

namespace IZhy.Common.AopProxy
{
    /// <summary>
    /// 忽略 装饰者 特性类 （ 只作用于方法 ）
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class IgnoreDecoratorAttribute : Attribute { }
}
