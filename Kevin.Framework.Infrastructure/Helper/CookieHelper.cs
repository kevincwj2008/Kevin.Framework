using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Kevin.Framework.Infrastructure
{
    /// <summary>
    /// 管理和操作Cookie的类。
    /// </summary>
    public class CookieManagement
    {
        /// <summary>
        /// 为了兼容WS/CS程序，把原来BS程序中保存在Cookie中的信息保存在这个本地变量中
        /// </summary>
        private Dictionary<string, NameValueCollection> _LocalCookie = new Dictionary<string, NameValueCollection>();

        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="strSource">Cookie属性字符串</param>
        /// <returns></returns>
        public delegate string EncryptCookie(string strSource);

        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="strEncrypt">Cookie加密字符串</param>
        /// <returns></returns>
        public delegate string DecryptCookie(string strEncrypt);

        private EncryptCookie encryptCookie = null;

        private DecryptCookie decryptCookie = null;

        #region 属性
        /// <summary>
        /// Cookie名称
        /// </summary>
        public string CookieName { get; set; }

        /// <summary>
        /// Cookie的域名
        /// </summary>
        public string CookieDomain { get; set; }

        /// <summary>
        /// Cookie的过期时间(以天为单位)
        /// </summary>
        public int CookieExpires { get; set; }
        #endregion 属性

        #region 构造函数
        private CookieManagement()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cName">要操作的cookie名称</param>
        /// <param name="topDomain">顶级域名</param>
        public CookieManagement(string cName, string topDomain)
        {
            CookieName = cName;
            CookieDomain = topDomain;
            CookieExpires = 1;
        }

        /// <summary>
        /// 构造函数(encryptCookie与decryptCookie必须同时赋值)
        /// </summary>
        /// <param name="cName">要操作的cookie名称</param>
        /// <param name="topDomain">顶级域名</param>
        /// <param name="encryptCookie">加密函数</param>
        /// <param name="decryptCookie">解密函数</param>
        public CookieManagement(string cName, string topDomain, EncryptCookie encryptCookie, DecryptCookie decryptCookie)
            : this(cName, topDomain)
        {
            this.encryptCookie = encryptCookie;
            this.decryptCookie = decryptCookie;
            if (this.encryptCookie == null && this.decryptCookie != null)
                throw new Exception();
            if (this.encryptCookie != null && this.decryptCookie == null)
                throw new Exception();
        }
        #endregion 构造函数

        #region 操作
        /// <summary>
        /// 将当前对象写入客户端Cookie
        /// </summary>
        /// <param name="namesvalues">要写入客户端的名值对集合</param>
        /// <returns>成功返回true，失败返回false</returns>
        public void DoWrite(NameValueCollection namesvalues)
        {
            // BS程序
            HttpCookie aCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (aCookie == null)
                aCookie = new HttpCookie(CookieName);
            else
                DoClear();
            if (!string.IsNullOrEmpty(CookieDomain))
            {
                aCookie.Domain = CookieDomain;
            }
            aCookie.Expires = DateTime.Now.AddDays(CookieExpires);
            foreach (string onename in namesvalues.AllKeys)
            {
                aCookie.Values[onename] = HttpUtility.UrlEncode(namesvalues[onename]);
            }
            if (encryptCookie != null)
                aCookie.Value = encryptCookie(aCookie.Value);
            HttpContext.Current.Response.Cookies.Add(aCookie);
        }

        /// <summary>
        /// 将当前对象写入客户端Cookie
        /// </summary>
        /// <param name="namesvalues">要写入客户端的名值对集合</param>
        /// <param name="domain">域名</param>
        /// <returns>成功返回true，失败返回false</returns>
        public void DoWrite(NameValueCollection namesvalues, string domain)
        {
            // BS程序
            HttpCookie aCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (aCookie == null)
                aCookie = new HttpCookie(CookieName);
            else
                DoClear();
            if (!string.IsNullOrEmpty(CookieDomain))
                aCookie.Domain = CookieDomain;
            aCookie.Expires = DateTime.Now.AddDays(CookieExpires);
            foreach (string onename in namesvalues.AllKeys)
                aCookie.Values[onename] = HttpUtility.UrlEncode(namesvalues[onename]);
            if (encryptCookie != null)
                aCookie.Value = encryptCookie(aCookie.Value);
            HttpContext.Current.Response.Cookies.Add(aCookie);
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public void DoClear()
        {
            if (HttpContext.Current == null)// CS程序                
                _LocalCookie.Clear();
            else// BS程序
            { 
                HttpCookie aCookie = HttpContext.Current.Request.Cookies[CookieName];
                if (aCookie != null)
                {
                    if (!string.IsNullOrEmpty(CookieDomain))
                        aCookie.Domain = CookieDomain;
                    aCookie.Expires = DateTime.Now.AddDays(-1);
                    aCookie.Values.Clear();
                    HttpContext.Current.Response.Cookies.Set(aCookie);
                }
            }
        }

        /// <summary>
        /// 从当前Cookie中读取解密后所有Cookie属性字符串
        /// </summary>
        /// <returns>Cookie属性字符串</returns>
        public string GetValue()
        {
            if (HttpContext.Current == null)
            {
                // CS程序   暂未实现
                return string.Empty;
            }
            else
            {
                // BS程序
                HttpCookie aCookie = HttpContext.Current.Request.Cookies[CookieName];
                if (aCookie != null)
                {
                    if (decryptCookie != null)
                        return decryptCookie(aCookie.Value);
                    else
                        return aCookie.Value;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 从当前Cookie中读取某属性的值
        /// </summary>
        /// <param name="name">要读取的属性名</param>
        /// <returns>属性值</returns>
        public string GetValueByName(string name)
        {
            // BS程序
            HttpCookie aCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (aCookie != null)
            {
                if (decryptCookie != null)
                {
                    string strCookie = decryptCookie(aCookie.Value);
                    foreach (string item in strCookie.Split(new Char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (item.Split('=').Length > 1 && item.Split('=')[0] == name)
                            return HttpUtility.UrlDecode(item.Split('=')[1]);
                    }
                    return string.Empty;
                }
                else
                {
                    return HttpUtility.UrlDecode(aCookie[name]);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 从当前Cookie中写入某属性的值
        /// </summary>
        /// <param name="name">要写入的属性名</param>
        /// <param name="value">要写入的属性值</param>
        public void SetValueByName(string name, string value)
        {
            // BS程序
            HttpCookie aCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (aCookie != null)
            {
                if (encryptCookie != null && decryptCookie != null)
                {
                    string strCookie = decryptCookie(aCookie.Value);
                    aCookie.Value = string.Empty;
                    foreach (string item in strCookie.Split(new Char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (item.Split('=').Length > 1)
                            aCookie[item.Split('=')[0]] = item.Split('=')[1];
                    }
                    aCookie[name] = HttpUtility.UrlEncode(value);
                    aCookie.Value = encryptCookie(aCookie.Value);
                }
                else
                {
                    aCookie[name] = HttpUtility.UrlEncode(value);
                }
            }
            if (!string.IsNullOrEmpty(CookieDomain))
            {
                aCookie.Domain = CookieDomain;
            }
            aCookie.Expires = DateTime.Now.AddDays(CookieExpires);
            HttpContext.Current.Response.Cookies.Add(aCookie);
        }

        /// <summary>
        /// 判断当前客户端是否已存在某名称的cookie。
        /// </summary>
        /// <returns>已存在返回true，否则返回false。</returns>
        public bool IsExist()
        {
            if (HttpContext.Current == null)
            {
                // CS程序
                return true;
            }
            else
            {
                // BS程序
                return HttpContext.Current.Request.Cookies[CookieName] != null;
            }
        }
        #endregion 操作
    }
}