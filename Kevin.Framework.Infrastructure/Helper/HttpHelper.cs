using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Kevin.Framework.Infrastructure
{
    /// <summary>
    /// Http
    /// </summary>
    public static partial class HttpHelper
    {
        /// <summary>
        /// Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        public static string Request(string url, IDictionary<string, string> parameters, EnumHttpMethod method)
        {
            string jsonStr = string.Empty;
            if (method == EnumHttpMethod.Post)
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                Stream reqStream = null;

                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = method.ToString();
                req.KeepAlive = false;
                req.ProtocolVersion = HttpVersion.Version10;
                req.Timeout = 10000;
                req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(string.IsNullOrEmpty(rsp.CharacterSet) ? "utf-8" : rsp.CharacterSet);
                jsonStr = GetResponseAsString(rsp, encoding);
                return jsonStr;

            }
            else
            {
                //创建请求
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters, "utf8");
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters, "utf8");
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                jsonStr = myStreamReader.ReadToEnd();
                return jsonStr;
            }
        }

        /// <summary>
        /// Http (GET/POST) 异步
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="param">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <param name="contentType">响应类型</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string RequestAsync(string url, string param, EnumHttpMethod method, string contentType = "application/json", Encoding encoding = null, int timeout = 10000)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            if (string.IsNullOrEmpty(contentType))
            {
                if (method == EnumHttpMethod.Post)
                {
                    contentType = "application/x-www-form-urlencoded";
                }
                if (method == EnumHttpMethod.Get)
                {
                    contentType = "text/html";
                }
            }

            encoding = encoding ?? Encoding.UTF8;

            if (!contentType.Contains("charset"))
            {
                if (contentType.Substring(contentType.Length - 1, 1) == ";")
                {
                    contentType = contentType + "charset=" + encoding.WebName;
                }
                else
                {
                    contentType = contentType + ";charset=" + encoding.WebName;
                }
            }

            if (!string.IsNullOrEmpty(param))
            {
                if (JsonHelper.IsJson(param))
                {

                }
                else
                {
                    //不是json
                    var paramArr = param.Split('&').ToList();
                    var paramList = new List<Tuple<string, string>>();
                    foreach (var item in paramArr)
                    {
                        var k = item.Split('=')[0];
                        var v = item.Split('=')[1];
                        paramList.Add(new Tuple<string, string>(k, HttpUtility.UrlEncode(v, encoding)));
                    }
                    var paramTemp = (from p in paramList select p.Item1 + "=" + p.Item2).ToList();
                    var paramStr = string.Join("&", paramTemp);
                    param = paramStr;
                }

                if (method == EnumHttpMethod.Get)
                {
                    var split = url.Contains("?") ? "&" : "?";
                    url = url + split + param;
                }
            }
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            webRequest.Method = method.ToString().ToUpper();
            var strResponse = string.Empty;

            if (method == EnumHttpMethod.Post)
            {
                webRequest.Timeout = timeout;
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                webRequest.ContentType = contentType;
                if (!string.IsNullOrEmpty(param))
                {
                    byte[] bytes = encoding.GetBytes(param);
                    using (var stream = webRequest.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            else if (method == EnumHttpMethod.Get)
            {
                webRequest.ReadWriteTimeout = timeout;
                webRequest.ContentType = contentType;
            }

            var complete = false;
            var stime = DateTime.Now;
            var asyncResult = webRequest.BeginGetResponse(new AsyncCallback(t =>
              {
                  using (var webResponse = webRequest.EndGetResponse(t))
                  {
                      using (var webStream = webResponse.GetResponseStream())
                      {
                          if (webResponse.Headers.Get("Content-Encoding") == "gzip")
                          {
                              using (GZipStream gzip = new GZipStream(webStream, CompressionMode.Decompress))
                              {
                                  using (var reader = new StreamReader(gzip, encoding))
                                  {
                                      strResponse = reader.ReadToEndAsync().Result;
                                      complete = true;
                                  }
                              }
                          }
                          else
                          {
                              using (var streamReader = new StreamReader(webStream, encoding))
                              {
                                  strResponse = streamReader.ReadToEndAsync().Result;
                                  complete = true;
                              }
                          }
                      }
                  }

              }), null);

            while (!complete && (DateTime.Now - stime).TotalMilliseconds < timeout)
            {

            }

            return strResponse;
        }

        /// <summary>
        /// Http (GET/POST) 异步
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="param">请求参数 Item1:参数名 Item2:参数值</param>
        /// <param name="method">请求方法</param>
        /// <param name="contentType">响应类型</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static string RequestAsync(string url, IEnumerable<Tuple<string, object>> param, EnumHttpMethod method, string contentType = "application/json", Encoding encoding = null, int timeout = 10000)
        {
            var paramTemp = (from p in param select p.Item1 + "=" + p.Item2.ToString()).ToList();
            var paramStr = string.Join("&", paramTemp);
            return RequestAsync(url, paramStr, method, contentType, encoding, timeout);
        }

        /// <summary>
        /// Http (GET/POST) 异步
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="param">请求参数 key:参数名 value:参数值</param>
        /// <param name="method">请求方法</param>
        /// <param name="contentType">响应类型</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public static string RequestAsync(string url, IDictionary<string, object> param, EnumHttpMethod method, string contentType = "application/json", Encoding encoding = null, int timeout = 10000)
        {
            var paramTemp = (from p in param select p.Key + "=" + p.Value.ToString()).ToList();
            var paramStr = string.Join("&", paramTemp);
            return RequestAsync(url, paramStr, method, contentType, encoding, timeout);
        }

        /// <summary>
        /// 组装普通文本请求参数
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        private static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            if (parameters == null)
                return string.Empty;
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }
    }

    public static partial class HttpHelper
    {
        /// <summary>
        /// 异步http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="timeout">超时时间 单位：秒</param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string url, int timeout = 10)
        {
            return await HttpGet(url, timeout);
        }

        /// <summary>
        /// 异步http请求 使用代理
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="proxy">代理IP</param>
        /// <param name="timeout">超时时间 单位：秒</param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string url, string proxy, int timeout = 10)
        {
            return await HttpGet(url, timeout, proxy);
        }
        /// <summary>
        /// 异步http请求 使用代理
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="proxyIp">代理IP</param>
        /// <param name="proxyPort">代理IP 端口</param>
        /// <param name="timeout">超时时间 单位：秒</param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string url, string proxyIp, int proxyPort, int timeout = 10)
        {
            return await HttpGet(url, timeout, proxyIp + ":" + proxyPort);
        }
        /// <summary>
        /// HttpGet请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="timeout">超时时间 单位：秒</param>
        /// <param name="proxy">代理IP</param>
        /// <returns></returns>
        private static async Task<string> HttpGet(string url, int timeout = 10, string proxy = "")
        {
            HttpClientHandler handler = null;
            HttpClient httpClient = null;
            //Task<string> task = null;
            var cts = new CancellationTokenSource();
            bool isCancel = false;
            try
            {
                if (string.IsNullOrWhiteSpace(proxy))
                {
                    httpClient = new HttpClient();
                }
                else
                {
                    handler = new HttpClientHandler();
                    handler.Proxy = new WebProxy(proxy);
                    handler.UseProxy = true;
                    httpClient = new HttpClient(handler);
                }
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
                httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                httpClient.Timeout = TimeSpan.FromSeconds(timeout);
                var response = await httpClient.GetAsync(new Uri(url), cts.Token);
                return await response.Content.ReadAsStringAsync();
                //if (response.IsSuccessStatusCode)
                //{
                //    task = response.Content.ReadAsStringAsync();
                //}
                //return await task;
            }
            catch (Exception ex)
            {
                if (handler != null)
                {
                    handler.Dispose();
                }
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }
                //if (task != null)
                //{
                //    task.Dispose();
                //}
                if (cts != null)
                {
                    cts.Cancel();
                    cts.Dispose();
                }
                isCancel = true;
                throw ex;
            }
            finally
            {
                if (!isCancel)
                {
                    if (handler != null)
                    {
                        handler.Dispose();
                    }
                    if (httpClient != null)
                    {
                        httpClient.Dispose();
                    }
                    //if (task != null)
                    //{
                    //    task.Dispose();
                    //}
                }
            }
        }
    }
}