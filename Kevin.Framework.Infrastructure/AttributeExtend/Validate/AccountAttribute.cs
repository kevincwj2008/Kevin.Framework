using System.Text.RegularExpressions;

namespace Kevin.Framework.Infrastructure.Validate
{
    /// <summary>
    /// 帐号验证(字母开头，允许5-16字节，允许字母数字下划线)
    /// </summary>
    public class AccountAttribute : BaseValidateAttribute
    {
        public override bool Validate(object value)
        {
            var email = value != null ? value.ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(email))
                return true;
            return Regex.IsMatch(email, RegexExpression.regexAccount);
        }
    }
}
