using System.Text.RegularExpressions;

namespace Kevin.Framework.Infrastructure.Validate
{
    public class IdCardsAttribute : BaseValidateAttribute
    {
        public override bool Validate(object value)
        {
            var email = value != null ? value.ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(email))
                return true;
            return Regex.IsMatch(email, RegexExpression.regexIdCards);
        }
    }
}
