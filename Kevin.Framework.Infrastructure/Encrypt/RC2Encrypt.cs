using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kevin.Framework.Infrastructure.Encrypt
{
    public class RC2Encrypt
    {
        /// <summary>
        /// 获取密钥
        /// Key为密钥，Value为向量(IV)
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, string> RC2Key()
        {
            RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
            return new KeyValuePair<string, string>(Convert.ToBase64String(rc2.Key), Convert.ToBase64String(rc2.IV));
        }

        /// <summary>
        /// RC2加密
        /// </summary>
        /// <param name="Rc2Key">密钥</param>
        /// <param name="Rc2IV">向量</param>
        /// <param name="text">原字符串</param>
        /// <returns></returns>
        public static string Encrypt(string Rc2Key, string Rc2IV, string text)
        {
            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
            byte[] key = Convert.FromBase64String(Rc2Key);
            byte[] IV = Convert.FromBase64String(Rc2IV);
            ICryptoTransform encryptor = rc2CSP.CreateEncryptor(key, IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                byte[] toEncrypt = Encoding.ASCII.GetBytes(text);
                csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                csEncrypt.FlushFinalBlock();
                byte[] encrypted = msEncrypt.ToArray();
                return Convert.ToBase64String(encrypted);
            }
        }

        /// <summary>
        /// RC2解密
        /// </summary>
        /// <param name="Rc2Key">密钥</param>
        /// <param name="Rc2IV">向量</param>
        /// <param name="text">原字符串</param>
        /// <returns></returns>
        public static string Decrypt(string Rc2Key, string Rc2IV, string text)
        {
            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
            byte[] key = Convert.FromBase64String(Rc2Key);
            byte[] IV = Convert.FromBase64String(Rc2IV);
            byte[] encrypted = Convert.FromBase64String(text);
            ICryptoTransform decryptor = rc2CSP.CreateDecryptor(key, IV);
            using (MemoryStream msDecrypt = new MemoryStream(encrypted))
            {
                CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                StringBuilder roundtrip = new StringBuilder();
                int b = 0;
                do
                {
                    b = csDecrypt.ReadByte();
                    if (b != -1)
                    {
                        roundtrip.Append((char)b);
                    }
                } while (b != -1);
                return roundtrip.ToString();
            }
        }
    }
}
