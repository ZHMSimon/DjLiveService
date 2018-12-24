using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DjLive.ControlPanel.WebUtil;
using DjLive.CPService.Impl;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;
using DjUtil.Tools;
using Newtonsoft.Json;

namespace DjLive.ControlPanel.Controllers.Api
{
    [RoutePrefix("api/Secure")]
    public class SecurePolicyController : BaseApiController
    {
        public ISecurePolicyServiceInterface SecureService { get; set; } = new SecurePolicyServiceImpl();
        [HttpGet, Route("SecurePolicys")]
        public async Task<IHttpActionResult> GetSecurePolicyModels([FromUri]int page, [FromUri]int countPerPage = 10, [FromUri]SecurePolicyModel delta = null)
        {
            try
            {
                int count = await SecureService.GetSecurePolicysCount(ThreadStaticObject.UserId, delta);
                List<SecurePolicyModel> logos = await SecureService.GetSimpleSecurePolicys(ThreadStaticObject.UserId, page, countPerPage, delta);
                var result = new { totalCount = count, models = logos };
                return Json(new DjLiveResponse<dynamic>(result));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }
        }

        [HttpGet, Route("SecurePolicy")]
        [ResponseType(typeof(SecurePolicyModel))]
        public async Task<IHttpActionResult> GetSecurePolicyModel([FromUri]string id)
        {
            try
            {
                SecurePolicyModel securePolicyModel = await SecureService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (securePolicyModel == null)
                {
                    return NotFound();
                }

                return Json(new DjLiveResponse<dynamic>(securePolicyModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }

        }

        [HttpPut, Route("SecurePolicy")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSecurePolicyModel([FromUri]string id, [FromBody]SecurePolicyModel securePolicyModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != securePolicyModel.Id)
                {
                    return BadRequest("参数不匹配.");
                }
                try
                {
                    ServiceResultMessage result = await SecureService.UpdateSecurePolicy(ThreadStaticObject.UserId, id, securePolicyModel);
                    if (result.code == ServiceResultCode.Success)
                    {
                        return Json(new DjLiveResponse<dynamic>(securePolicyModel));
                    }
                    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}"});
                }
                catch (Exception dbE)
                {
                    if (!await SecureService.SecurePolicyModelExists(ThreadStaticObject.UserId, id))
                    {
                        return NotFound();
                    }
                    throw;
                }

            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }
        }

        [HttpPost, Route("SecurePolicy")]
        [ResponseType(typeof(SecurePolicyModel))]
        public async Task<IHttpActionResult> PostSecurePolicyModel([FromBody]SecurePolicyModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var id = Guid.NewGuid().Str();
                try
                {
                    
                    ServiceResultMessage result = await SecureService.AddSecurePolicy(ThreadStaticObject.UserId, id, model);
                    if (result.code == ServiceResultCode.Success)
                    {
                        model.Id = id;
                        return Json(new DjLiveResponse<dynamic>(model));
                    }
                    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}"});
                }
                catch (Exception)
                {
                    if (await SecureService.SecurePolicyModelExists(ThreadStaticObject.UserId, model.Id))
                    {
                        return Conflict();
                    }
                    throw;
                }
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }

        }

        [HttpDelete, Route("SecurePolicy")]
        [ResponseType(typeof(SecurePolicyModel))]
        public async Task<IHttpActionResult> DeleteSecurePolicyModel([FromUri]string id)
        {
            try
            {
                SecurePolicyModel securePolicyModel = await SecureService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (securePolicyModel == null)
                {
                    return NotFound();
                }

                ServiceResultMessage result = await SecureService.RemoveAsync(ThreadStaticObject.UserId, id);
                return Json(new DjLiveResponse<dynamic>(securePolicyModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }



        }

        [HttpDelete, Route("SecurePolicys")]
        [ResponseType(typeof(SecurePolicyModel))]
        public async Task<IHttpActionResult> DeleteSecurePolicyModels([FromUri]List<string> ids)
        {
            throw new NotImplementedException();
            //try
            //{
            //    SecurePolicyModel SecurePolicyModel = await SecureService.FindByIdAsync(ThreadStaticObject.UserId, id);
            //    if (SecurePolicyModel == null)
            //    {
            //        return NotFound();
            //    }

            //    ServiceResultMessage result = await SecureService.RemoveAsync(ThreadStaticObject.UserId, id);
            //    return Json(new DjLiveResponse<dynamic>()SecurePolicyModel);
            //}
            //catch (Exception e)
            //{
            //    var errorId = Guid.NewGuid().Str();
            //    LogHelper.Error(errorId, e);
            //    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            //}



        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
