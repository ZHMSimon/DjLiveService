using System;
using System.Web;
using DjUtil.Tools;
using DjUtil.Tools.Cryptography;
using Newtonsoft.Json;

namespace DjLive.ControlPanel.WebUtil
{
    public class CookieHelper
    {
        public enum CryptType
        {
            Aes,
            Des,
            Sha1,
            Md5
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Domain = "";
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        /// <summary>
        /// 添加加密cookie
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strName">名称</param>
        /// <param name="value">需要加密的对象</param>
        /// <param name="expires">过期时间(分钟)</param>
        /// <param name="cryptType">加密方式</param>
        public static void WriteCryptCookie<T>(string strName, T value,int expires = 8*60, CryptType cryptType= CryptType.Aes)
        {
            string json = JsonConvert.SerializeObject(value);
            string cryptString = null;
            switch (cryptType)
            {
                    case CryptType.Aes:
                {
                        cryptString = AesEncryptHelper.EncryptAes(json);

                        break;
                }
                    case CryptType.Des:
                {
                        cryptString = DesEncryptHelper.EncryptDes(json);

                        break;
                }
                    case CryptType.Sha1:
                {
                    cryptString = UnDecryptableEncryptHelpers.Sha1Encrypt(json);
                        break;
                }
                case CryptType.Md5:
                    {
                        cryptString = UnDecryptableEncryptHelpers.Md5Encrypt(json);
                        break;
                    }
                default:
                {
                    cryptString = EncryptUtils.Base64Encrypt(json);
                    break;
                }
            }
            #if DEBUG
                        cryptString = json;
            #endif
            WriteCookie(strName, cryptString, expires);
        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.Cookies[strName] != null)
                {
                    return HttpContext.Current.Request.Cookies[strName].Value.ToString();
                }
            }
            return "";
        }
        /// <summary>
        /// 读取加密cookie
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strName">名称</param>
        /// <param name="cryptType">加密方式</param>
        /// <returns>加密前的对象</returns>
        public static T GetCryptCookie<T>(string strName, CryptType cryptType = CryptType.Aes)
        {
            try
            {
                string jsonString = null;
                string cryptString = GetCookie(strName);
                #if DEBUG
                jsonString = cryptString;
                if (string.IsNullOrWhiteSpace(jsonString)) return default(T);
                return JsonConvert.DeserializeObject<T>(jsonString);
                #endif
                switch (cryptType)
                {
                    case CryptType.Aes:
                        {
                            jsonString = AesEncryptHelper.DecryptAes(cryptString);
                            break;
                        }
                    case CryptType.Des:
                        {
                            jsonString = DesEncryptHelper.DecryptDes(cryptString);
                            break;
                        }
                    default:
                        {
                            jsonString = EncryptUtils.Base64Decrypt(cryptString);
                            break;
                        }
                }

                if (string.IsNullOrWhiteSpace(jsonString)) return default(T);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception e)
            {
                LogHelper.Error($@"CookieHelper ReadError {e.Message}",e);
                return default(T);
            }
        }


    }
}