using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DjLive.ControlPanel.WebUtil;
using DjLive.CPDao.Interface;
using DjLive.CPService.Impl;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;
using DjUtil.Tools;

namespace DjLive.ControlPanel.Controllers.Api
{
    [RoutePrefix("api/Logo")]
    public class LogoTemplateController : BaseApiController
    {
        public ILogoServiceInterface LogoService { get; set; } = new LogoTemplateServiceImpl();
        [HttpGet,Route("LogoTemplates")]
        public  async Task<IHttpActionResult> GetLogoTemplateModels([FromUri]int page,[FromUri]int countPerPage,[FromUri]LogoTemplateModel delta)
        {
            try
            {
                int count = await LogoService.GetLogoTemplatesCount(ThreadStaticObject.UserId, delta);
                List<LogoTemplateModel> logos = await LogoService.GetSimpleLogoTemplates(ThreadStaticObject.UserId, page,countPerPage,delta);
                var result = new {totalCount = count, models = logos};
                return Json(new DjLiveResponse<dynamic>(result));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }
        }

        [HttpGet, Route("LogoTemplate")]
        [ResponseType(typeof(LogoTemplateModel))]
        public async Task<IHttpActionResult> GetLogoTemplateModel([FromUri]string id)
        {
            try
            {
                LogoTemplateModel logoTemplateModel = await LogoService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (logoTemplateModel == null)
                {
                    return NotFound();
                }

                return Json(new DjLiveResponse<dynamic>(logoTemplateModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }
           
        }

        [HttpPut, Route("LogoTemplate")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLogoTemplateModel([FromUri]string id, [FromBody]LogoTemplateModel logoTemplateModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != logoTemplateModel.Id)
                {
                    return BadRequest("参数不匹配.");
                }
                try
                {
                    ServiceResultMessage result = await LogoService.UpdateLogeTemplate(ThreadStaticObject.UserId, id, logoTemplateModel);
                    if (result.code == ServiceResultCode.Success)
                    {
                        return Json(new DjLiveResponse<dynamic>(logoTemplateModel));
                    }
                    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}"});
                }
                catch (Exception dbE)
                {                    
                    if (!await LogoService.LogoTemplateModelExists(ThreadStaticObject.UserId, id))
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

        [HttpPost, Route("LogoTemplate")]
        [ResponseType(typeof(LogoTemplateModel))]
        public async Task<IHttpActionResult> PostLogoTemplateModel([FromBody]LogoTemplateModel model)
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
                    ServiceResultMessage result = await LogoService.AddLogeTemplate(ThreadStaticObject.UserId, id, model);
                    if (result.code == ServiceResultCode.Success)
                    {
                        model.Id = id;
                        return Json(new DjLiveResponse<dynamic>(model));
                    }
                    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}"});
                }
                catch (Exception)
                {
                    if (await LogoService.LogoTemplateModelExists(ThreadStaticObject.UserId, model.Id))
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

        [HttpDelete, Route("LogoTemplate")]
        [ResponseType(typeof(LogoTemplateModel))]
        public async Task<IHttpActionResult> DeleteLogoTemplateModel([FromUri]string id)
        {
            try
            {
                LogoTemplateModel logoTemplateModel = await LogoService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (logoTemplateModel == null)
                {
                    return NotFound();
                }

                ServiceResultMessage result = await LogoService.RemoveAsync(ThreadStaticObject.UserId,id);
                return Json(new DjLiveResponse<dynamic>(logoTemplateModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }


           
        }

        [HttpDelete, Route("LogoTemplates")]
        [ResponseType(typeof(LogoTemplateModel))]
        public async Task<IHttpActionResult> DeleteLogoTemplateModels([FromUri]List<string> ids)
        {
            throw new NotImplementedException();
            //try
            //{
            //    LogoTemplateModel logoTemplateModel = await LogoService.FindByIdAsync(ThreadStaticObject.UserId, id);
            //    if (logoTemplateModel == null)
            //    {
            //        return NotFound();
            //    }

            //    ServiceResultMessage result = await LogoService.RemoveAsync(ThreadStaticObject.UserId, id);
            //    return Json(new DjLiveResponse<dynamic>()logoTemplateModel);
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
