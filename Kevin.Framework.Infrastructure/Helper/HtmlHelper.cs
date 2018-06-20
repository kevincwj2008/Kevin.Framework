using System.Text.RegularExpressions;

namespace Kevin.Framework.Infrastructure
{
    /// <summary>
    /// HTML
    /// </summary>
    public static class HtmlHelper
    {
        /// <summary>
        /// 去除html标记
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string html, int length = 0)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }
            string strText = Regex.Replace(html, "<[^>]+>", "");
            strText = Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }

        /// <summary>
        /// 获取html中图片地址
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetImgUrl(string html)
        {
            string str = string.Empty;
            //string sPattern = @"^<img\s+[^>]*>";
            Regex r = new Regex(@"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>",
              RegexOptions.Compiled);
            Match m = r.Match(html.ToLower());
            if (m.Success)
                str = m.Result("${url}");
            return str;
        }
    }
}
