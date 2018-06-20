using System.Text.RegularExpressions;

namespace Kevin.Framework.Infrastructure.Validate
{
    /// <summary>
    /// 邮箱验证
    /// </summary>
    public class EmailAttribute : BaseValidateAttribute
    {
        public override bool Validate(object value)
        {
            var email = value != null ? value.ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(email))
                return true;
            return Regex.IsMatch(email, RegexExpression.regexEmail);
        }
    }
}
