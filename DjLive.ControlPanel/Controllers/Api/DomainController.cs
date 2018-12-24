using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DjLive.ControlPanel.WebUtil;
using DjLive.CPService.Impl;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Enum;
using DjLive.SdkModel.Model;
using DjUtil.Tools;

namespace DjLive.ControlPanel.Controllers.Api
{
    [RoutePrefix("api/Domain")]
    public class DomainController : BaseApiController
    {
        public IVhostServiceInterface VhostService { get; set; } = new VhostServiceImpl();

        [HttpGet, Route("TranscodeTemplates")]
        public async Task<IHttpActionResult> GetDomainModels([FromUri]int page, [FromUri]int countPerPage = 10, [FromUri]DomainModel delta = null)
        {
            try
            {
                int count = await VhostService.GetDomainsCount(ThreadStaticObject.UserId, delta);
                List<DomainModel> logos = await VhostService.GetSimpleDomains(ThreadStaticObject.UserId, page, countPerPage, delta);
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

        [HttpGet, Route("Domain")]
        [ResponseType(typeof(DomainModel))]
        public async Task<IHttpActionResult> GetDomainModel([FromUri]string id)
        {
            try
            {
                DomainModel domainModel = await VhostService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (domainModel == null)
                {
                    return NotFound();
                }

                return Json(new DjLiveResponse<dynamic>(domainModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }

        }

        [HttpPut, Route("Domain")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDomainModel([FromUri]string id, [FromBody]DomainModel domainModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != domainModel.Id)
                {
                    return BadRequest("参数不匹配.");
                }
                try
                {
                    ServiceResultMessage result = await VhostService.UpdateDomain(ThreadStaticObject.UserId, id, domainModel);
                    if (result.code == ServiceResultCode.Success)
                    {
                        return Json(new DjLiveResponse<dynamic>(domainModel));
                    }
                    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}"});
                }
                catch (Exception dbE)
                {
                    if (!await VhostService.DomainModelExists(ThreadStaticObject.UserId, id))
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
        [HttpDelete, Route("Domain")]
        [ResponseType(typeof(DomainModel))]
        public async Task<IHttpActionResult> DeleteDomainModel([FromUri]string id)
        {
            try
            {
                DomainModel DomainModel = await VhostService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (DomainModel == null)
                {
                    return NotFound();
                }

                ServiceResultMessage result = await VhostService.RemoveAsync(ThreadStaticObject.UserId, id);
                return Json(new DjLiveResponse<dynamic>(DomainModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }



        }

        [HttpPost, Route("Domain")]
        [ResponseType(typeof(DomainModel))]
        public async Task<IHttpActionResult> CreateDomain([FromBody]DomainModel model)
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
                    model.Id = id;
                    ServiceResultMessage result = await VhostService.CreateVhost(ThreadStaticObject.UserId, model);
                    if (result.code == ServiceResultCode.Success)
                    {
                        return Json(new DjLiveResponse<DomainModel>(model));
                    }
                    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId} {result.Message}"});
                }
                catch (Exception e)
                {
                    //todo:判断异常原因
                    return Json(new DjLiveResponse<dynamic>(model)
                    {
                        ApiCode = ApiCode.Conflict,
                        Message = "数据库中添加失败.",
                    });
                }
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }
        }       
    }
}
