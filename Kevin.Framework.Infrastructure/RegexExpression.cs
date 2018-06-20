namespace Kevin.Framework.Infrastructure
{
    public class RegexExpression
    {
        /// <summary>
        /// 正则：帐号
        /// </summary>
        public readonly static string regexAccount = @"^[a-zA-Z][a-zA-Z0-9_]{4,15}$";

        /// <summary>
        /// 正则：邮箱
        /// </summary>
        public readonly static string regexEmail = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// 正则：身份证
        /// </summary>
        public readonly static string regexIdCards = @"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}$";

        /// <summary>
        /// 正则：手机
        /// </summary>
        public readonly static string regexCellphone = @"^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\\d{8}$";

        /// <summary>
        /// 正则：座机
        /// </summary>
        public readonly static string regexTelephone = @"^(0[0-9]{2,3}/-)?([2-9][0-9]{6,7})+(/-[0-9]{1,4})?$";
    }
}
