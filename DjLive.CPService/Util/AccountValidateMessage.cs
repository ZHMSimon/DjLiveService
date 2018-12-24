using DjLive.SdkModel.Cookie;

namespace DjLive.CPService.Util
{
    public class AccountValidateMessage
    {
        public ValidateType ResulType { get; set; }
        public string Message { get; set; }
        public AuthCookieModel Cookie { get; set; }
    }
}