using System;
using System.Text;

namespace Kevin.Framework.Infrastructure.Encrypt
{
    public class Base64Encrypt
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : Convert.ToBase64String(Encoding.Default.GetBytes(text)).Replace("+", "%2B");
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">需要解密的字符串</param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : Encoding.Default.GetString(Convert.FromBase64String(text.Replace("%2B", "+")));
        }
    }
}
