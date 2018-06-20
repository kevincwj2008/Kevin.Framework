using System;

namespace Kevin.Framework.Infrastructure.Exceptions
{
    /// <summary>
    /// 业务逻辑异常
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}