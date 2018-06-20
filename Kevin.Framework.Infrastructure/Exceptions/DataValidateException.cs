using System;

namespace Kevin.Framework.Infrastructure.Exceptions
{
    /// <summary>
    /// 数据验证异常
    /// </summary>
    public class DataValidateException : Exception
    {
        public DataValidateException(string message) : base(message)
        {
        }
    }
}