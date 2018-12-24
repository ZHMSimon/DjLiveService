using System.Configuration;
using System.IO;

namespace DjLive.CPService.Util
{
    public class ConfigurationValue
    {
        public static readonly string ApiAuthHeaderName = "Dj_Auth_Token";
        public static string TempRootPath
        {
            get
            {
                var path = ConfigurationManager.AppSettings["TempRootPath"];
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = @"D:/LiveTemp/";
                }
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }
        public static string TempLogoPath
        {
            get
            {
                var path = TempRootPath + "Logo/";
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = @"D:/LiveTemp/Logo/";
                }
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }
        public static string TempDvrPath
        {
            get
            {
                var path = TempRootPath + "Dvr/";
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = @"D:/LiveTemp/Dvr/";
                }
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }
        public static string TempFilePath
        {
            get
            {
                var path = TempRootPath + "File/";
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = @"D:/LiveTemp/File/";
                }
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }
        public static string DefaultCallbackDomain
        {
            get
            {
                var url = ConfigurationManager.AppSettings["DefaultCallbackDomain"];
                if (string.IsNullOrWhiteSpace(url))
                {
                    url = @"http://172.28.10.137/";
                }
                return url;
            }
        }

        public static string SecretKey {
            get
            {
                
                var key = ConfigurationManager.AppSettings["LiveSecretKey"];
                if (string.IsNullOrWhiteSpace(key))
                {
                    key = @"76eb7d3b40b64370939e02f04ad6b3a8";
                }
                return key;
            }
        }
    }
}