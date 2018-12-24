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

namespace DjLive.ControlPanel.Controllers.Api
{
    [RoutePrefix("api/Server")]
    public class ServerNodeController : BaseApiController
    {
        public IServerNodeServiceInterface ServerService { get; set; } = new ServerNodeServiceImpl();
        [HttpGet, Route("ServerNodes")]
        public async Task<IHttpActionResult> GetServerNodeModels([FromUri]ServerNodeModel delta)
        {
            try
            {
                int count = await ServerService.GetServerNodesCount(ThreadStaticObject.UserId, delta);
                List<ServerNodeModel> logos = await ServerService.GetSimpleServerNodes(ThreadStaticObject.UserId, delta);
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

        [HttpGet, Route("ServerNode")]
        [ResponseType(typeof(ServerNodeModel))]
        public async Task<IHttpActionResult> GetServerNodeModel([FromUri]string id)
        {
            try
            {
                ServerNodeModel serverNodeModel = await ServerService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (serverNodeModel == null)
                {
                    return NotFound();
                }

                return Json(new DjLiveResponse<dynamic>(serverNodeModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }

        }

        [HttpPut, Route("ServerNode")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutServerNodeModel([FromUri]string id, [FromBody]ServerNodeModel serverNodeModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != serverNodeModel.Id)
                {
                    return BadRequest("参数不匹配.");
                }
                try
                {
                    ServiceResultMessage result = await ServerService.UpdateServerNode(ThreadStaticObject.UserId, id, serverNodeModel);
                    if (result.code == ServiceResultCode.Success)
                    {
                        return Json(new DjLiveResponse<dynamic>(serverNodeModel));
                    }
                    return Json(new DjLiveResponse<dynamic>() { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}" });
                }
                catch (Exception dbE)
                {
                    if (!await ServerService.ServerNodeModelExists(ThreadStaticObject.UserId, id))
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

        [HttpPost, Route("ServerNode")]
        [ResponseType(typeof(ServerNodeModel))]
        public async Task<IHttpActionResult> PostServerNodeModel([FromBody]ServerNodeModel model)
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
                    ServiceResultMessage result = await ServerService.AddServerNode(ThreadStaticObject.UserId, id, model);
                    if (result.code == ServiceResultCode.Success)
                    {
                        model.Id = id;
                        return Json(new DjLiveResponse<dynamic>(model));
                    }
                    return Json(new DjLiveResponse<dynamic>() { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId}" });
                }
                catch (Exception)
                {
                    if (await ServerService.ServerNodeModelExists(ThreadStaticObject.UserId, model.Id))
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

        [HttpDelete, Route("ServerNode")]
        [ResponseType(typeof(ServerNodeModel))]
        public async Task<IHttpActionResult> DeleteServerNodeModel([FromUri]string id)
        {
            try
            {
                ServerNodeModel serverNodeModel = await ServerService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (serverNodeModel == null)
                {
                    return NotFound();
                }

                ServiceResultMessage result = await ServerService.RemoveAsync(ThreadStaticObject.UserId, id);
                return Json(new DjLiveResponse<dynamic>(serverNodeModel));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }



        }

        [HttpDelete, Route("ServerNodes")]
        [ResponseType(typeof(ServerNodeModel))]
        public async Task<IHttpActionResult> DeleteServerNodeModels([FromUri]List<string> ids)
        {
            throw new NotImplementedException();
            //try
            //{
            //    ServerNodeModel ServerNodeModel = await ServerService.FindByIdAsync(ThreadStaticObject.UserId, id);
            //    if (ServerNodeModel == null)
            //    {
            //        return NotFound();
            //    }

            //    ServiceResultMessage result = await ServerService.RemoveAsync(ThreadStaticObject.UserId, id);
            //    return Json(new DjLiveResponse<dynamic>()ServerNodeModel);
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