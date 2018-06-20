using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kevin.Framework.Infrastructure.Validate
{
    /// <summary>
    /// 电话验证
    /// </summary>
    public class PhoneAttribute : BaseValidateAttribute
    {
        /// <summary>
        /// 是否多号码验证，必须以逗号分隔
        /// </summary>
        private bool _Multiple { get; set; }

        /// <summary>
        /// 号码类型，手机、座机、手机和座机
        /// </summary>
        private EnumPhoneType _PhoneType { get; set; }

        /// <summary>
        /// 电话验证
        /// </summary>
        /// <param name="PhoneType">电话类型（默认是手机）</param>
        /// <param name="Multiple">是否多个（默认单个）</param>
        public PhoneAttribute(EnumPhoneType PhoneType = EnumPhoneType.Cellphone, bool Multiple = false)
        {
            _PhoneType = PhoneType;
            _Multiple = Multiple;
        }

        public override bool Validate(object value)
        {
            var phone = value != null ? value.ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(phone))
                return true;
            List<string> phoneList;
            if (_Multiple)
                phoneList = phone.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            else
                phoneList = new List<string>() { phone };
            var flag = true;
            Func<string,bool> func;
            switch (_PhoneType)
            {
                case EnumPhoneType.Phone:
                    func = phonenumber => Regex.IsMatch(phonenumber, RegexExpression.regexCellphone) || Regex.IsMatch(phonenumber, RegexExpression.regexTelephone);
                    break;
                case EnumPhoneType.Cellphone:
                    func = phonenumber => Regex.IsMatch(phonenumber, RegexExpression.regexCellphone);
                    break;
                case EnumPhoneType.Telephone:
                    func = phonenumber => Regex.IsMatch(phonenumber, RegexExpression.regexTelephone);
                    break;
                default:
                    func = phonenumber => false;
                    break;
            }

            foreach (var item in phoneList)
            {
                if (!func.Invoke(item))
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
    }
}
