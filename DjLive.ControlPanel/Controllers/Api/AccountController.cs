using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DjLive.ControlPanel.WebUtil;
using DjLive.CPService.Impl;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;
using DjUtil.Tools;
using DjUtil.Tools.Cryptography;
using Newtonsoft.Json;

namespace DjLive.ControlPanel.Controllers.Api
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        public IAccountServiceInterface AccountService { get; set; }= new AccountServiceImpl();

        [AllowAnonymous,HttpPost,Route("Login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var message = await AccountService.CheckAccountVilidate(model.UserName, model.Password);
                if (message.ResulType == ValidateType.None)
                {
                    #if DEBUG
                    string header = JsonConvert.SerializeObject(message.Cookie);
#else
                    string header = AesEncryptHelper.EncryptAes(EncryptUtils.Base64Encrypt(JsonConvert.SerializeObject(message.Cookie)));
#endif

                    var result = new TokenModel() { AuthHeader = header, RespMessage = message.ResulType.ToString() };
                    return Json(result);
                }
                return BadRequest(message.Message);
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new { message = $@"发生未知错误,请联系管理员,错误代码:{errorId}" });
            }
        }
        
        [AllowAnonymous,HttpPost, Route("Register")]
        public async Task<IHttpActionResult> Register([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ServiceResultMessage resultMessage = await AccountService.CreateAccountModel(model.UserName, model.Password);
                if (resultMessage.code == ServiceResultCode.Success)
                {
                    var message = await AccountService.CheckAccountVilidate(model.UserName, model.Password);
                    if (message.ResulType == ValidateType.None)
                    {
#if DEBUG
                        string header = JsonConvert.SerializeObject(message.Cookie);
#else
                            string header = AesEncryptHelper.DecryptAes(JsonConvert.SerializeObject(message.Cookie));
#endif
                        var result = new {authHeader = header, respMessage = message.Message};
                        return Json(result);
                    }
                    return BadRequest(message.Message);
                }
                return BadRequest(resultMessage.Message);
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new {message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }

        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region 为 非对称加密 准备

        //[AllowAnonymous, HttpPost, Route("Login")]
        //public async Task<IHttpActionResult> Login([FromBody] string cryptJson)
        //{
        //    throw new NotImplementedException();
        //    try
        //    {
        //    }
        //    catch (Exception e)
        //    {
        //        var errorId = Guid.NewGuid().Str();
        //        LogHelper.Error(errorId, e);
        //        return Json(new { message = $@"发生未知错误,请联系管理员,错误代码:{errorId}" });
        //    }
        //}
        //[AllowAnonymous, HttpPost, Route("Register")]
        //public async Task<IHttpActionResult> Register([FromBody] string cryptJson)
        //{
        //    throw new NotImplementedException();
        //    try
        //    {
        //    }
        //    catch (Exception e)
        //    {
        //        var errorId = Guid.NewGuid().Str();
        //        LogHelper.Error(errorId, e);
        //        return Json(new { message = $@"发生未知错误,请联系管理员,错误代码:{errorId}" });
        //    }
        //}

        #endregion
    }
}
