using System.Text.RegularExpressions;

namespace Kevin.Framework.Infrastructure.Validate
{
    /// <summary>
    /// 自定义正则验证
    /// </summary>
    public class RegexAttribute : BaseValidateAttribute
    {
        /// <summary>
        /// 自定义表达式
        /// </summary>
        private string _RegexExpression { get; set; }

        /// <summary>
        /// 表达式验证（默认单个）
        /// </summary>
        /// <param name="RegexExpression"></param>
        public RegexAttribute(string RegexExpression)
        {
            _RegexExpression = RegexExpression;
        }

        public override bool Validate(object value)
        {
            var str = value != null ? value.ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(str))
                return true;
            return Regex.IsMatch(str, _RegexExpression);
        }
    }
}
