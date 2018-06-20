using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Kevin.Framework.Infrastructure
{
    /// <summary>
    /// 文件
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="fullPath">文件全路径</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string ReadFile(string fullPath, string encode = "utf-8")
        {
            try
            {
                StreamReader sr = new StreamReader(fullPath, System.Text.Encoding.GetEncoding(encode));
                string content = sr.ReadToEnd().ToString();
                sr.Close();
                return content;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 读取文本文件类容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static async Task<string> ReadStringAsync(string path)
        {
            using (StreamReader reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// 写入文本文件类容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">类容</param>
        /// <returns></returns>
        public static async Task WriteStringAsync(string path, string content)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                //清空文件
                fileStream.SetLength(0);
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    await streamWriter.WriteAsync(content);
                }
            }
        }
    }
}