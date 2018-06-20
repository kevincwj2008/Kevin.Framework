﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kevin.Framework.Infrastructure.Encrypt
{
    /// <summary>
    /// 不可逆加密
    /// 1 防止被篡改
    /// 2 防止明文存储
    /// 3 防止抵赖，数字签名
    /// </summary>
    public class MD5Encrypt
    {
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同,
        /// 使用的UTF8编码
        /// </summary>
        /// <param name="source">待加密字串</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public static string Encrypt(string source, int length = 32)//默认参数
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            HashAlgorithm provider = CryptoConfig.CreateFromName("MD5") as HashAlgorithm;
            byte[] bytes = Encoding.UTF8.GetBytes(source);//这里需要区别编码的
            byte[] hashValue = provider.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (length)
            {
                case 16://16位密文是32位密文的9到24位字符
                    for (int i = 4; i < 12; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                case 32:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取文件的MD5摘要
        /// </summary>
        /// <param name="filePath">文件全路径，包含文件名</param>
        /// <returns></returns>
        public static string Abstract(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                return Abstract(file);
            }
        }

        /// <summary>
        /// 根据stream获取摘要
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static string Abstract(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));
            return sb.ToString();
        }
    }
}
