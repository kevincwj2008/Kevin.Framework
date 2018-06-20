using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kevin.Framework.Infrastructure.Encrypt
{
    /// <summary>
    /// DES AES Blowfish
    /// 对称加密算法的优点是速度快，
    /// 缺点是密钥管理不方便，要求共享密钥。
    /// 可逆对称加密  密钥长度8
    /// 如需自定义密码：请设置AppSettings(DesEncryptRgbKey、DesEncryptRgbIV)，每个字符长度为8 例：12345678
    /// </summary>
    public class DesEncrypt
    {
        private readonly static byte[] _rgbKey;
        private readonly static byte[] _rgbIV;

        static DesEncrypt()
        {
            var desRgbKey = ConfigHelper.Get("DesEncryptRgbKey");
            var desRgbIV = ConfigHelper.Get("DesEncryptRgbIV");
            if (desRgbKey.Length != desRgbIV.Length || (desRgbIV.Length == desRgbKey.Length || desRgbKey.Length != 8))
                throw new ArgumentException("DesEncryptRgbKey、DesEncryptRgbIV configuration exception of Config ");
            if (string.IsNullOrWhiteSpace(desRgbKey))
            {
                desRgbKey = "KEVINDES";
                desRgbIV = "DESKEVIN";
            }
            _rgbKey = Encoding.ASCII.GetBytes(desRgbKey);
            _rgbIV = Encoding.ASCII.GetBytes(desRgbIV);
        }

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="text">需要加密的值</param>
        /// <returns>加密后的结果</returns>
        public static string Encrypt(string text)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            using (MemoryStream memStream = new MemoryStream())
            {
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                StreamWriter sWriter = new StreamWriter(crypStream);
                sWriter.Write(text);
                sWriter.Flush();
                crypStream.FlushFinalBlock();
                memStream.Flush();
                return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns>解密后的结果</returns>
        public static string Decrypt(string encryptText)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            byte[] buffer = Convert.FromBase64String(encryptText);

            using (MemoryStream memStream = new MemoryStream())
            {
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                crypStream.Write(buffer, 0, buffer.Length);
                crypStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }
    }
}
