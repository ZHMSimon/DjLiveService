using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Web;
using DjLive.Sdk.Util;
using DjLive.SdkModel;

namespace DjLive.Sdk.ApiClient
{

    internal class ApiProxy<TInterface> : RealProxy
    {

        public event EventHandler<EventArgs> PreInvoke;

        #region Constructor

        public ApiProxy()
            : base(typeof(TInterface))
        { }

        protected virtual void OnPreInvoke(EventArgs e)
        {
            if (TokenManager.GetInstance().Domain == null) throw new NullReferenceException("未设置 直播服务器 信息.");
            PreInvoke?.Invoke(this, e);
        }

        #endregion Constructor       
        public override IMessage Invoke(IMessage msg)
        {
            this.OnPreInvoke(EventArgs.Empty);

            var methodCallMessage = msg as IMethodCallMessage;
            var paramMap = new Dictionary<String, Object>();
            for (int i = 0; i < methodCallMessage?.ArgCount; i++)
            {
                paramMap.Add(methodCallMessage.GetInArgName(i), methodCallMessage.GetArg((i)));
            }
            var apiAttribute = methodCallMessage?.MethodBase.GetCustomAttributes(typeof(ApiAttribute), false)[0] as ApiAttribute;
            if (apiAttribute == null)
            {
                //todo:未来所有返回值 都修改为ApiMessage
                return new ReturnMessage(new ApiMessage<string>() { Code = ApiCode.UnExceptError, Message = "API文件异常,请联系管理员..." }, null, 0, null, null);
            }
            var requestUrl = TokenManager.GetInstance().Domain.ServiceUrl + apiAttribute?.Url;
            var headerDic = new Dictionary<string,string>()
            {
                {ConfigValue.ApiAuthHeaderName,TokenManager.GetInstance().Token?.AuthHeader}, 
                {ConfigValue.ApiMessageName,TokenManager.GetInstance().Token?.RespMessage}, 
            };
            object responseValue = null;
            if (apiAttribute?.HttpMethod == HttpMethod.Get && paramMap.Count == 0)
            {
                HttpUtil.GetAsync(requestUrl, headerDic).ContinueWith(item =>
                {
                    responseValue = JsonConvert.DeserializeObject(item.Result, (methodCallMessage.MethodBase as MethodInfo)?.ReturnType);
                }).Wait();

            }
            if (apiAttribute?.HttpMethod == HttpMethod.Get && paramMap.Count > 0)
            {
                var paramArr = paramMap.Select(param =>
                    $"{param.Key}={HttpUtility.UrlEncode(param.Value.ToString(), Encoding.UTF8)}");
                requestUrl += ("?" + string.Join("&", paramArr.ToArray()));
                HttpUtil.GetAsync(requestUrl, headerDic).ContinueWith(item =>
                {
                    responseValue = JsonConvert.DeserializeObject(item.Result,(methodCallMessage.MethodBase as MethodInfo)?.ReturnType);
                }).Wait();

            }
            if (apiAttribute?.HttpMethod == HttpMethod.Delete && paramMap.Count > 0)
            {
                var paramArr = paramMap.Select(param => $"{param.Key}={HttpUtility.UrlEncode(param.Value.ToString(), Encoding.UTF8)}");
                requestUrl += ("?" + string.Join("&", paramArr.ToArray()));
                HttpUtil.DeleteAsync(requestUrl, headerDic).ContinueWith(item =>
                {
                    responseValue = JsonConvert.DeserializeObject(item.Result, (methodCallMessage.MethodBase as MethodInfo)?.ReturnType);
                }).Wait();

            }
            if (apiAttribute?.HttpMethod == HttpMethod.Put && paramMap.Count > 0)
            {
                var paramArr = paramMap.Where(item => item.Value.GetType().IsValueType || item.Value is string).Select(param =>
                    $"{param.Key}={HttpUtility.UrlEncode(param.Value.ToString(), Encoding.UTF8)}");
                requestUrl += ("?" + string.Join("&", paramArr.ToArray()));
                var json = JsonConvert.SerializeObject(paramMap.FirstOrDefault(item => !item.Value.GetType().IsValueType && !(item.Value is string) ).Value);
                HttpUtil.PutAsync(requestUrl, json, headerDic).ContinueWith(item =>
                {
                    responseValue = JsonConvert.DeserializeObject(item.Result, (methodCallMessage.MethodBase as MethodInfo)?.ReturnType);
                }).Wait();
            }
            else if (apiAttribute?.HttpMethod == HttpMethod.Post && paramMap.Count > 0)
            {
                var paramArr = paramMap.Where(item => item.Value.GetType().IsValueType || item.Value is string).Select(param =>
                    $"{param.Key}={HttpUtility.UrlEncode(param.Value.ToString(), Encoding.UTF8)}");
                requestUrl += ("?" + string.Join("&", paramArr.ToArray()));
                var json = JsonConvert.SerializeObject(paramMap.FirstOrDefault(item => !item.Value.GetType().IsValueType && !(item.Value is string)).Value);
                HttpUtil.PostAsync(requestUrl, json, headerDic).ContinueWith(item =>
                {
                    responseValue = JsonConvert.DeserializeObject(item.Result, (methodCallMessage.MethodBase as MethodInfo)?.ReturnType);
                }).Wait();
            }
            return new ReturnMessage(responseValue, null, 0, null, null); 
        }

 
    }
}