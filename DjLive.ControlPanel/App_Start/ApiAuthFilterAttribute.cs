using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using DjLive.ControlPanel.WebUtil;
using DjLive.CPService.Util;
using DjLive.SdkModel.Cookie;
using DjUtil.Tools.Cryptography;
using Newtonsoft.Json;

namespace DjLive.ControlPanel
{
    public class ApiAuthFilterAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //WebApi线程池会从空闲线程中取出一个来激活controller，造成TenantThreadLocalValue中有值，会导致tenant数据混乱
            //所以首先需要清空线程的tenant相关数据
            ClearTenantThreadLocalValue();
            if (SkipAuthorization(actionContext))
            {
                return;
            }
            HandleUserAuthorization(actionContext);
        }
        private bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
        private void ClearTenantThreadLocalValue()
        {
            ThreadStaticObject.Token = null;
            ThreadStaticObject.UserId = null;
            ThreadStaticObject.ClientIp = null;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            var content = "服务端拒绝访问：你没有权限，或者掉线了";
            response.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }


        private void HandleUserAuthorization(HttpActionContext actionContext)
        {
            var headerString = default(string);
            if (actionContext.Request.Headers.Contains(ConfigurationValue.ApiAuthHeaderName))
            {
                headerString = actionContext.Request.Headers.GetValues(ConfigurationValue.ApiAuthHeaderName).First();
            }
            if (string.Equals(headerString, "HEADER_NULL"))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            if (string.IsNullOrEmpty(headerString))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            AuthCookieModel header = null;
            try
            {

            #if DEBUG
                var result = headerString;// CspCrossPlatformExchangeWrapper.UnWrapKey(headerString);
            #else
                var result = EncryptUtils.Base64Decrypt(AesEncryptHelper.DecryptAes(headerString));

            #endif
                header = JsonConvert.DeserializeObject<AuthCookieModel>(result);
            }
            catch (Exception ex)
            {
                throw;
            }
            //todo:判断账号超时
            if (header == null || string.IsNullOrEmpty(header.UserName) || string.IsNullOrEmpty(header.Token)||DateTime.Now>header.Expired)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            ThreadStaticObject.UserId = header.Id;
            ThreadStaticObject.UserName = header.UserName;
            ThreadStaticObject.Token = header.Token;
        }

    }
}