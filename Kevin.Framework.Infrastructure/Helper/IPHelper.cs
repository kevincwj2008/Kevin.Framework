using System.Web;

namespace Kevin.Framework.Infrastructure
{
    /// <summary>
    /// IP
    /// </summary>
    public class IPHelper
    {
        /// <summary>
        /// 从请求中获取客户端IP地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetIPFromHttpContext(HttpContext context)
        {
            if (context == null)
                return "127.0.0.1";

            string ip = context.Request.UserHostAddress;
            if (string.IsNullOrEmpty(ip) || ip.Length < 7)
                ip = "127.0.0.1";

            return ip;
        }
    }
}
