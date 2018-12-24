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
    [RoutePrefix("api/Transcode")]
    public class TranscodeTemplateController : BaseApiController
    {
        public ITranscodeTemplateServiceInterface TranscodeService { get; set; } = new TranscodeTemplateServiceImpl();
        [HttpGet, Route("TranscodeTemplates")]
        public async Task<IHttpActionResult> GetTranscodeTemplateModels([FromUri]int page, [FromUri]int countPerPage, [FromUri]TranscodeTemplateModel delta)
        {
            try
            {
                int count = await TranscodeService.GetTranscodeTemplatesCount(ThreadStaticObject.UserId, delta);
                List<TranscodeTemplateModel> logos = await TranscodeService.GetSimpleTranscodeTemplates(ThreadStaticObject.UserId, page, countPerPage, delta);
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

        [HttpGet, Route("TranscodeTemplate")]
        [ResponseType(typeof(TranscodeTemplateModel))]
        public async Task<IHttpActionResult> GetTranscodeTemplateModel([FromUri]string id)
        {
            try
            {
                TranscodeTemplateModel TranscodeTemplateModel = await TranscodeService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (TranscodeTemplateModel == null)
                {
                    return NotFound();
                }

                return Json(new DjLiveResponse<dynamic>(TranscodeTemplateModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }

        }

        [HttpPut, Route("TranscodeTemplate")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTranscodeTemplateModel([FromUri]string id, [FromBody]TranscodeTemplateModel transcodeTemplateModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != transcodeTemplateModel.Id)
                {
                    return BadRequest("参数不匹配.");
                }
                try
                {
                    ServiceResultMessage result = await TranscodeService.UpdateLogeTemplate(ThreadStaticObject.UserId, id, transcodeTemplateModel);
                    if (result.code == ServiceResultCode.Success)
                    {
                        return Json(new DjLiveResponse<dynamic>(transcodeTemplateModel));
                    }
                    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}"});
                }
                catch (Exception dbE)
                {
                    if (!await TranscodeService.TranscodeTemplateModelExists(ThreadStaticObject.UserId, id))
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

        [HttpPost, Route("TranscodeTemplate")]
        [ResponseType(typeof(TranscodeTemplateModel))]
        public async Task<IHttpActionResult> PostTranscodeTemplateModel([FromBody]TranscodeTemplateModel model)
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
                    
                    ServiceResultMessage result = await TranscodeService.AddLogeTemplate(ThreadStaticObject.UserId, id, model);
                    if (result.code == ServiceResultCode.Success)
                    {
                        model.Id = id;
                        return Json(new DjLiveResponse<dynamic>(model));
                    }
                    return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}"});
                }
                catch (Exception)
                {
                    if (await TranscodeService.TranscodeTemplateModelExists(ThreadStaticObject.UserId, model.Id))
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

        [HttpDelete, Route("TranscodeTemplate")]
        [ResponseType(typeof(TranscodeTemplateModel))]
        public async Task<IHttpActionResult> DeleteTranscodeTemplateModel([FromUri]string id)
        {
            try
            {
                TranscodeTemplateModel transcodeTemplateModel = await TranscodeService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (transcodeTemplateModel == null)
                {
                    return NotFound();
                }

                ServiceResultMessage result = await TranscodeService.RemoveAsync(ThreadStaticObject.UserId, id);
                return Json(new DjLiveResponse<dynamic>(transcodeTemplateModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }



        }

        [HttpDelete, Route("TranscodeTemplates")]
        [ResponseType(typeof(TranscodeTemplateModel))]
        public async Task<IHttpActionResult> DeleteTranscodeTemplateModels([FromUri]List<string> ids)
        {
            throw new NotImplementedException();
            //try
            //{
            //    TranscodeTemplateModel TranscodeTemplateModel = await TranscodeService.FindByIdAsync(ThreadStaticObject.UserId, id);
            //    if (TranscodeTemplateModel == null)
            //    {
            //        return NotFound();
            //    }

            //    ServiceResultMessage result = await TranscodeService.RemoveAsync(ThreadStaticObject.UserId, id);
            //    return Json(new DjLiveResponse<dynamic>()TranscodeTemplateModel);
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
