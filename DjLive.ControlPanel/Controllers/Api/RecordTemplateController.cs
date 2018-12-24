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
    [RoutePrefix("api/Record")]
    public class RecordTemplateController : BaseApiController
    {
        public IRecordTemplateServiceInterface RecordService { get; set; } = new RecordTemplateServiceImpl();
        [HttpGet, Route("RecordTemplates")]
        public async Task<IHttpActionResult> GetRecordTemplateModels([FromUri]int page, [FromUri]int countPerPage, [FromUri]RecordTemplateModel delta)
        {
            try
            {
                int count = await RecordService.GetRecordTemplatesCount(ThreadStaticObject.UserId, delta);
                List<RecordTemplateModel> recordTemplateModels = await RecordService.GetSimpleRecordTemplates(ThreadStaticObject.UserId, page, countPerPage, delta);
                return Json(new DjLiveResponse<List<RecordTemplateModel>>()
                {
                    ApiCode = ApiCode.Success,
                    Content = recordTemplateModels,
                    TotalCount = count,
                });
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>()
                {
                    ApiCode = ApiCode.UnExceptError,
                    Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"
                });
            }
        }

        [HttpGet, Route("RecordTemplate")]
        [ResponseType(typeof(RecordTemplateModel))]
        public async Task<IHttpActionResult> GetRecordTemplateModel([FromUri]string id)
        {
            try
            {
                RecordTemplateModel recordTemplateModel = await RecordService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (recordTemplateModel == null)
                {
                    return NotFound();
                }

                return Json(new DjLiveResponse<dynamic>(recordTemplateModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>()
                {
                    ApiCode = ApiCode.UnExceptError,
                    Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"
                });
            }

        }

        [HttpPut, Route("RecordTemplate")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRecordTemplateModel([FromUri]string id, [FromBody]RecordTemplateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != model.Id)
                {
                    return BadRequest("参数不匹配.");
                }
                try
                {
                    ServiceResultMessage result = await RecordService.UpdateLogeTemplate(ThreadStaticObject.UserId, id, model);
                    if (result.code == ServiceResultCode.Success)
                    {
                        return Json(new DjLiveResponse<RecordTemplateModel>(model));
                    }
                    return Json(new DjLiveResponse<dynamic>()
                    { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}" });
                }
                catch (Exception dbE)
                {
                    if (!await RecordService.RecordTemplateModelExists(ThreadStaticObject.UserId, id))
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
                return Json(new DjLiveResponse<dynamic>()
                {
                    ApiCode = ApiCode.UnExceptError,
                    Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"
                });
            }
        }

        [HttpPost, Route("RecordTemplate")]
        [ResponseType(typeof(RecordTemplateModel))]
        public async Task<IHttpActionResult> PostRecordTemplateModel([FromBody]RecordTemplateModel model)
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

                    ServiceResultMessage result = await RecordService.AddLogeTemplate(ThreadStaticObject.UserId, id, model);
                    if (result.code == ServiceResultCode.Success)
                    {
                        model.Id = id;
                        return Json(new DjLiveResponse<RecordTemplateModel>(model));
                    }
                    return   Json(new DjLiveResponse<dynamic>()
                    {
                        ApiCode = ApiCode.UnExceptError,
                        Message =  $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}"
                    });
                }
                catch (Exception)
                {
                    if (await RecordService.RecordTemplateModelExists(ThreadStaticObject.UserId, model.Id))
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
                return Json(new DjLiveResponse<dynamic>()
                {
                    ApiCode = ApiCode.UnExceptError,
                    Message = $"发生未知错误,请联系管理员,错误代码:{errorId}"
                });
            }

        }

        [HttpDelete, Route("RecordTemplate")]
        [ResponseType(typeof(RecordTemplateModel))]
        public async Task<IHttpActionResult> DeleteRecordTemplateModel([FromUri]string id)
        {
            try
            {
                RecordTemplateModel recordTemplateModel = await RecordService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (recordTemplateModel == null)
                {
                    return NotFound();
                }

                ServiceResultMessage result = await RecordService.RemoveAsync(ThreadStaticObject.UserId, id);
                return Json(new DjLiveResponse<RecordTemplateModel>(recordTemplateModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>()
                {
                    ApiCode = ApiCode.UnExceptError,
                    Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"
                });
            }



        }

        [HttpDelete, Route("RecordTemplates")]
        [ResponseType(typeof(RecordTemplateModel))]
        public async Task<IHttpActionResult> DeleteRecordTemplateModels([FromUri]List<string> ids)
        {
            throw new NotImplementedException();
            //try
            //{
            //    model model = await RecordService.FindByIdAsync(ThreadStaticObject.UserId, id);
            //    if (model == null)
            //    {
            //        return NotFound();
            //    }

            //    ServiceResultMessage result = await RecordService.RemoveAsync(ThreadStaticObject.UserId, id);
            //    return Json(model);
            //}
            //catch (Exception e)
            //{
            //    var errorId = Guid.NewGuid().Str();
            //    LogHelper.Error(errorId, e);
            //    return Json(new { message = $@"发生未知错误,请联系管理员,错误代码:{errorId}" });
            //}



        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}