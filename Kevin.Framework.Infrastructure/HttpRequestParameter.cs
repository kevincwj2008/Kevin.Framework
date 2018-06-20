using System;
using System.Web;

namespace Kevin.Framework.Infrastructure
{
    public class HttpRequestParameter
    {
        /// <summary>
        /// 获取字符串类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <param name="replaceSpecialchar">是否替换特殊字符（默认true）</param>
        /// <returns></returns>
        public static bool GetRequest(string param, out string value, bool replaceSpecialchar = true)
        {
            value = HttpContext.Current.Request[param] ?? "";
            if (replaceSpecialchar)
                value = ReplaceSpecialchar(value);
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 获取Guid类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetRequest(string param, out Guid value)
        {
            var tempStr = string.Empty;
            GetRequest(param, out tempStr, false);
            value = Guid.Empty;
            return Guid.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 获取时间类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetRequest(string param, out DateTime value)
        {
            var tempStr = string.Empty;
            GetRequest(param, out tempStr, false);
            value = DateTime.MinValue;
            return DateTime.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 获取bool类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetRequest(string param, out bool value)
        {
            var tempStr = string.Empty;
            GetRequest(param, out tempStr, false);
            value = false;
            return bool.TryParse(tempStr, out value);
        }
        
        /// <summary>
        /// 获取数字类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetRequest(string param, out int value)
        {
            var tempStr = string.Empty;
            GetRequest(param, out tempStr, false);
            value = 0;
            return Int32.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 获取长整型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetRequest(string param, out long value)
        {
            var tempStr = string.Empty;
            GetRequest(param, out tempStr, false);
            value = 0;
            return long.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 从Form中获取字符串类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <param name="replaceSpecialchar">是否替换特殊字符（默认true）</param>
        /// <returns></returns>
        public static bool GetFormRequest(string param, out string value, bool replaceSpecialchar = true)
        {
            value = HttpContext.Current.Request.Form[param] ?? "";
            if (replaceSpecialchar)
                value = ReplaceSpecialchar(value);
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 从Form中获取Guid类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetFormRequest(string param, out Guid value)
        {
            var tempStr = string.Empty;
            GetFormRequest(param, out tempStr, false);
            value = Guid.Empty;
            return Guid.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 从Form中获取时间类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetFormRequest(string param, out DateTime value)
        {
            var tempStr = string.Empty;
            GetFormRequest(param, out tempStr, false);
            value = DateTime.MinValue;
            return DateTime.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 从Form中获取bool类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetFormRequest(string param, out bool value)
        {
            var tempStr = string.Empty;
            GetFormRequest(param, out tempStr, false);
            value = false;
            return bool.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 从Form中获取数字类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetFormRequest(string param, out int value)
        {
            var tempStr = string.Empty;
            GetFormRequest(param, out tempStr, false);
            value = 0;
            return Int32.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 从Form中获取数字类型的值
        /// </summary>
        /// <param name="param">参数名称</param>
        /// <param name="value">返回参数的值</param>
        /// <returns></returns>
        public static bool GetFormRequest(string param, out long value)
        {
            var tempStr = string.Empty;
            GetFormRequest(param, out tempStr, false);
            value = 0;
            return long.TryParse(tempStr, out value);
        }

        /// <summary>
        /// 替换特殊字符
        /// </summary>
        /// <param name="text">原字符串</param>
        /// <returns></returns>
        private static string ReplaceSpecialchar(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? "" : text.Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
        }
    }
}
