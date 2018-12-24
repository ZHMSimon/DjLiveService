using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DjLive.SdkModel.Cookie
{
    public class AuthCookieModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime Expired { get; set; }
    }
}
