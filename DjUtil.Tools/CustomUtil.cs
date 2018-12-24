using System;
using System.Security.Cryptography;
using System.Text;

namespace DjUtil.Tools
{
    public static class CustomUtil
    {
        public static long Unix13TimeStamp(this DateTime dateTime)
        {
            TimeSpan ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds * 1000);
        }
        public static long Unix10TimeStamp(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        public static byte[] Hmacsha256String(this string value, string secret)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(value);
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] hashmessage;
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                hashmessage = hmacsha256.ComputeHash(messageBytes);
                hmacsha256.Clear();
            }
            return hashmessage;
        }
        public static string Hexbytes2String(this byte[] hashmessage)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            foreach (byte item in hashmessage)
            {
                stringBuilder.AppendFormat("{0:x2}", item);
            }
            return stringBuilder.ToString();
        }
        public static string Md5Encrypt(this string input, Encoding encode)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encode.GetBytes(input));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }
        public static string Str(this Guid guid)
        {
            return guid.ToString("N");
        }
        public static bool IsStringNull(this string value)
        {
            if (value == null) return true;
            if (value.Equals("null", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}