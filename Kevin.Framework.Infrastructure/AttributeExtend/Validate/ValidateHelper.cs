using System;
using System.Linq;

namespace Kevin.Framework.Infrastructure.Validate
{
    public static class ValidateHelper
    {
        public static bool Validate<T>(this T tModel) where T : class, new()
        {
            Type type = tModel.GetType();
            var propList = type.GetProperties().ToList();
            if (propList == null || propList.Count == 0)
                return true;
            foreach (var prop in propList)
            {
                if (prop.IsDefined(typeof(BaseValidateAttribute), true))
                {
                    object[] attributeArray = prop.GetCustomAttributes(typeof(BaseValidateAttribute), true);
                    foreach (BaseValidateAttribute attribute in attributeArray)
                    {
                        if (!attribute.Validate(prop.GetValue(tModel)))
                        {
                            return false;//表示终止
                            //throw new Exception($"{prop.Name}的值{prop.GetValue(tModel)}不对");
                        }
                    }
                }
            }
            return true;
        }
    }
}
