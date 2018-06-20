using System.Text.RegularExpressions;

namespace Kevin.Framework.Infrastructure.Helper
{
    public class RegexHelper
    {
        public static bool Match(string regex, string value)
        {
            return Regex.IsMatch(value, regex);
        }
    }
}
