using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DjUtil.Tools.Cryptography
{
    public class DesEncryptHelper
    {
        /// <summary>
        /// Des默认密钥向量
        /// </summary>
        public static string DesIv => "20160602";
        /// <summary>
        /// Des加解密钥必须8位
        /// </summary>
        public static string DesKey => "20160602";

        /// <summary>
        /// 获取Des8位密钥
        /// </summary>
        /// <param name="key">Des密钥字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>Des8位密钥</returns>
        static byte[] GetDesKey(string key, Encoding encoding)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "Des密钥不能为空");
            }
            if (key.Length > 8)
            {
                key = key.Substring(0, 8);
            }
            if (key.Length < 8)
            {
                // 不足8补全
                key = key.PadRight(8, '0');
            }
            return encoding.GetBytes(key);
        }

        /// <summary>
        /// Des加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptDes(string source, Encoding encoding = null)
        {
            return EncryptDes(source, DesKey, DesIv, encoding);
        }

        /// <summary>
        /// Des加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">des密钥，长度必须8位</param>
        /// <param name="iv">密钥向量</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptDes(string source, string key, string iv, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            byte[] rgbKeys = GetDesKey(key, encoding),
                    rgbIvs = GetDesKey(iv, encoding),
                    inputByteArray = encoding.GetBytes(source);
            using (DESCryptoServiceProvider desProvIder = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                    desProvIder.CreateEncryptor(rgbKeys, rgbIvs), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        // 1.第一种
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();
                        memoryStream.Flush();
                        memoryStream.Close();
                        desProvIder.Clear();
                        string result = Convert.ToBase64String(memoryStream.ToArray());
                        return result;

                        // 2.第二种
                        //StringBuilder result = new StringBuilder();
                        //foreach (byte b in memoryStream.ToArray())
                        //{
                        //    result.AppendFormat("{0:X2}", b);
                        //}
                        //cryptoStream.FlushFinalBlock();
                        //cryptoStream.Close();
                        //memoryStream.Flush();
                        //memoryStream.Close();
                        //desProvIder.Clear();
                        //return result.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Des解密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptDes(string source, Encoding encoding = null)
        {
            return DecryptDes(source, DesKey, DesIv, encoding);
        }

        /// <summary>
        /// Des解密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">des密钥，长度必须8位</param>
        /// <param name="iv">密钥向量</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptDes(string source, string key, string iv, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            byte[] rgbKeys = GetDesKey(key, encoding),
                    rgbIvs = GetDesKey(iv, encoding),
                    inputByteArray = Convert.FromBase64String(source);
            using (DESCryptoServiceProvider desProvIder = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                    desProvIder.CreateDecryptor(rgbKeys, rgbIvs), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();
                        memoryStream.Flush();
                        memoryStream.Close();
                        desProvIder.Clear();
                        byte[] result = memoryStream.ToArray();
                        return encoding.GetString(result);
                    }
                }
            }
        }
    }
}
