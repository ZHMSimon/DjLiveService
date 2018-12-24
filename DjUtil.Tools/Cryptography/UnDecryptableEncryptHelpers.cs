using System.Security.Cryptography;
using System.Text;

namespace DjUtil.Tools.Cryptography
{
    public class UnDecryptableEncryptHelpers
    {
        /// <summary>
        /// 对字符串SHA1加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>加密后的十六进制字符串</returns>
        public static string Sha1Encrypt(string source, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            // 第一种方式
            byte[] byteArray = encoding.GetBytes(source);
            using (HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider())
            {
                byteArray = hashAlgorithm.ComputeHash(byteArray);
                StringBuilder stringBuilder = new StringBuilder(256);
                foreach (byte item in byteArray)
                {
                    stringBuilder.AppendFormat("{0:x2}", item);
                }
                hashAlgorithm.Clear();
                return stringBuilder.ToString();
            }

            //// 第二种方式
            //using (SHA1 sha1 = SHA1.Create())
            //{
            //    byte[] hash = sha1.ComputeHash(encoding.GetBytes(source));
            //    StringBuilder stringBuilder = new StringBuilder();
            //    for (int index = 0; index < hash.Length; ++index)
            //        stringBuilder.Append(hash[index].ToString("x2"));
            //    sha1.Clear();
            //    return stringBuilder.ToString();
            //}
        }

        public static string Md5Encrypt(string source, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            byte[] byteArray = encoding.GetBytes(source);
            using (HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider())
            {
                byteArray = hashAlgorithm.ComputeHash(byteArray);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte item in byteArray)
                {
                    stringBuilder.AppendFormat("{0:x2}", item);
                }
                hashAlgorithm.Clear();
                return stringBuilder.ToString();
            }
        }
    }
}
