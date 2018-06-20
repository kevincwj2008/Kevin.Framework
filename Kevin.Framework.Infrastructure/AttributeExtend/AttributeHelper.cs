using System;
using System.Reflection;

namespace Kevin.Framework.Infrastructure.AttributeExtend
{
    public class AttributeHelper
    {
        /// <summary>
        /// 获取给定枚举值的界面显示文本
        /// </summary>
        /// <param name="enumValue">枚举</param>
        /// <returns></returns>
        public static string GetRemark(Enum enumValue)
        {
            Type objType = enumValue.GetType();
            string s = enumValue.ToString();
            RemarkAttribute[] remarkAttrAry = (RemarkAttribute[])objType.GetField(s).GetCustomAttributes(typeof(RemarkAttribute), false);
            return remarkAttrAry == null || remarkAttrAry.Length == 0 ? string.Empty : remarkAttrAry[0].Text;
        }

        /// <summary>
        /// 获取给定类属性的界面显示文本
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="mi">类中属性</param>
        /// <returns></returns>
        public static string GetRemark(Type t, MemberInfo mi)
        {
            RemarkAttribute remarkAttr = (RemarkAttribute)Attribute.GetCustomAttribute(mi, typeof(RemarkAttribute), false);
            return remarkAttr == null ? string.Empty : remarkAttr.Text;
        }

        /// <summary>
        /// 获取给定枚举值的界面显示文本
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="Text">文本</param>
        /// <returns></returns>
        public static string GetValue(Type t, string Text)
        {
            FieldInfo[] fieldInfos = t.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                RemarkAttribute[] remarkAttrAry = (RemarkAttribute[])fieldInfo.GetCustomAttributes(typeof(RemarkAttribute), false);
                if (remarkAttrAry != null && remarkAttrAry.Length > 0 && remarkAttrAry[0].Text == Text)
                    return fieldInfo.Name;
            }
            return string.Empty;
        }
    }
}
