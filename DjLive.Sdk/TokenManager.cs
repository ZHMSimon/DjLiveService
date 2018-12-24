using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DjLive.Sdk.ApiClient;
using DjLive.Sdk.ApiService;
using DjLive.Sdk.Model;
using DjLive.Sdk.Util;
using DjLive.SdkModel.Cookie;
using DjLive.SdkModel.Model;

namespace DjLive.Sdk
{
    public class TokenManager: Singleton<TokenManager>
    {
        
        protected override void Init()
        {
            base.Init();
        }

        private ServicePoint _domain { get; set; }
        private string _userName = "";
        private string _password = "";
        private TokenModel _token = null;
        public TokenModel Token => _token;
        internal ServicePoint Domain => _domain;

        public void InitServicePoint(string url, HttpScheme scheme )
        {
            _domain = new ServicePoint()
            {
                ServiceUrl = url,
                Scheme = scheme,
            };
        }
        public async Task<SdkErrorMessage> UpdateToken(string userName,string password)
        {
            if (_domain == null) throw new NullReferenceException("未设置 直播服务器 信息.");
            //var apiProxy = new ApiProxy<ILogoTemplateServiceInterface>();
            //var loginService = ((ILogoTemplateServiceInterface)apiProxy.GetTransparentProxy());
            //var c = loginService.GetProductAnnounce("", "",0);

            _userName = userName;
            _password = password;

            var result = Validate();
            if (result.Code != SdkErrorType.Success) return result;

            await HttpUtil.PostAsync(_domain.ServiceUrl+"/api/Account/Login", $"{{UserName:\"{userName}\",Password:\"{password}\"}}", null).ContinueWith(
            item =>
            {
                var json = item.Result;
                if (json != null)
                {
                    _token = JsonConvert.DeserializeObject<TokenModel>(json);
                }
            });
            if (_token != null)
            {
                return SdkErrorMessage.OK;
            }
            return new SdkErrorMessage() { Code = SdkErrorType.GetMessageError };

        }
        public SdkErrorMessage RefreshToken()
        {
            if (_domain == null) throw new NullReferenceException("未设置 直播服务器 信息.");
            var result = Validate();
            if (result.Code != SdkErrorType.Success) return result;
            HttpUtil.PostAsync(_domain + "/api/Account/Login", $"{{{"UserName"}:\"{_userName}\",{"Password"}:\"{_password}\"}}", null).ContinueWith(
                item =>
                {
                    var json = item.Result;
                    if (json != null)
                    {
                        _token = JsonConvert.DeserializeObject<TokenModel>(json);
                    }
                });
            if (_token != null)
            {
                return SdkErrorMessage.OK;
            }
            return new SdkErrorMessage() { Code = SdkErrorType.GetMessageError };
        }
        private SdkErrorMessage Validate()
        {
            if (string.IsNullOrEmpty(_userName) || string.IsNullOrEmpty(_password))
            {
                return new SdkErrorMessage()
                {
                    Code = SdkErrorType.UserIsNull,
                    Message = "用户名或者密码不能为空."
                };
            }
            return SdkErrorMessage.OK;

        }
    }
}