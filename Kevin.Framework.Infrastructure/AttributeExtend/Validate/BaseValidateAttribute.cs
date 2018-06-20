using System;

namespace Kevin.Framework.Infrastructure.Validate
{
    /// <summary>
    /// 验证基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class BaseValidateAttribute : Attribute
    {
        public abstract bool Validate(object value);
    }
}
