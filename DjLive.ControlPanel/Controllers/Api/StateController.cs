using System;
using System.Collections.Generic;
using System.IO;
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
using DjLive.SdkModel.Model;
using DjUtil.Tools;
using DjUtil.Tools.Cryptography;
using Newtonsoft.Json;

namespace DjLive.ControlPanel.Controllers.Api
{
    [RoutePrefix("api/State")]
    public class StateController : ApiController
    {
        public IStateServiceInterface StateService { get; set; } = new StateServiceImpl();
        public IServerNodeServiceInterface ServerNodeService { get; set; } = new ServerNodeServiceImpl();
        public IVhostServiceInterface VhostService { get; set; } = new VhostServiceImpl();
        [HttpGet,Route("ReloadConf")]
        public async Task<IHttpActionResult> ReloadSrsConf(string id)
        {
            try
            {
                var node = ServerNodeService.FindByIdAsync(ThreadStaticObject.UserId, id);
                if (node != null)
                {
                    var models = await ServerNodeService.UpdateSrsConf(ThreadStaticObject.UserId, id);
                    return Json(new DjLiveResponse<dynamic>(models));
                }
                else
                {
                    return Json(new DjLiveResponse<dynamic>()
                    {
                        ApiCode = ApiCode.NotFound,
                        Message = "服务器对象不存在."
                    });

                }
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>() { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}" });
            }
        }
        [HttpGet, Route("Vhosts")]
        public async Task<IHttpActionResult> GetVhostModels()
        {
            try
            {
                var models = await StateService.GetVhostsState();
                return Json(new DjLiveResponse<dynamic>(models));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }
        }
        [HttpGet, Route("Streams")]
        public async Task<IHttpActionResult> GetStreamModels()
        {
            try
            {
                var models = await StateService.GetStreamsState();
                return Json(new DjLiveResponse<dynamic>(models));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }
        }
        [HttpGet, Route("Clients")]
        public async Task<IHttpActionResult> GetClientModels()
        {
            try
            {
                var models = await StateService.GetClientsState();
                return Json(new DjLiveResponse<dynamic>(models));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>(){ApiCode = ApiCode.UnExceptError,Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}"});
            }
        }
        [HttpGet, Route("Dvrs")]
        public async Task<IHttpActionResult> GetDvrModels([FromUri] string domainId,[FromUri]string streamName, [FromUri]int page, [FromUri]int countPerPage)
        {
            try
            {
                List<VodItemModel> models = await StateService.GetVodItems(domainId,streamName,page,countPerPage);
                return Json(new DjLiveResponse<dynamic>(models));
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Json(new DjLiveResponse<dynamic>() { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}" });
            }
        }
        [HttpPost, Route("Domain")]
        public async Task<IHttpActionResult> CreateDefaultDomain([FromBody]DomainModel model)
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
                    model.SecurePolicyPair = new KeyNamePair("76eb7d3b40b64370939e02f04ad6b3a8", "");
                    model.ServerPair = new KeyNamePair("fdd5659abd28416d91b7711581cfa6a0", "");
                    if (result.code == ServiceResultCode.Success)
                    {
                        return Json(new DjLiveResponse<DomainModel>(model));
                    }
                    return Json(new DjLiveResponse<dynamic>() { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId} {result.Message}" });
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
                return Json(new DjLiveResponse<dynamic>() { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}" });
            }
        }
        [HttpPost, Route("Room")]
        public async Task<IHttpActionResult> CreateRoom([FromBody]BoardCastRoomModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (model.ExpireTime.Equals(new DateTime()))
                {
                    model.ExpireTime = DateTime.Now.AddDays(2);
                }
                var id = Guid.NewGuid().Str();
                try
                {
                    model.Id = id;
                    ServiceResultMessage result = await VhostService.CreateLiveRoom(ThreadStaticObject.UserId, model);
                    if (result.code == ServiceResultCode.Success)
                    {  
                        string publishUrl = $"rtmp://{model.Domain}/live/?{CreatePublishTokenAuth(ThreadStaticObject.UserId, model.StreamName,model.ExpireTime.Ticks)}/{model.StreamName}";
                        string audienceUrl = $"rtmp://{model.Domain}/live/?{CreateAudinceTokenAuth(ThreadStaticObject.UserId, model.StreamName, model.ExpireTime.Ticks)}/{model.StreamName}";
                        return Json(new DjLiveResponse<dynamic>(new
                        {
                            PublishUrl = publishUrl,
                            PlayUrl = audienceUrl,
                        }));
                    }
                    return Json(new DjLiveResponse<dynamic>() { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{result.ErrorId} {result.Message}" });

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
                return Json(new DjLiveResponse<dynamic>() { ApiCode = ApiCode.UnExceptError, Message = $@"发生未知错误,请联系管理员,错误代码:{errorId}" });
            }
        }

        private string CreateAudinceTokenAuth(string userId, string roomName,long expireTime)
        {
            var tokenInfo = $"room={roomName}&audienceId={userId}&expireTime={expireTime}";
            var token = EncryptUtils.MD5Encrypt(tokenInfo + $"|{ConfigurationValue.SecretKey}");
            tokenInfo += $"&token={token}";
            return tokenInfo;
        }
        private string CreatePublishTokenAuth(string userId, string roomName, long expireTime)
        {
            var tokenInfo = $"room={roomName}&publisher={userId}&expireTime={expireTime}";
            var token = EncryptUtils.MD5Encrypt(tokenInfo + $"|{ConfigurationValue.SecretKey}");
            tokenInfo += $"&token={token}";
            return tokenInfo;
        }
        private bool TryVertifyTokenAuth(string authString, out string roomNum, out string userId, out int action)
        {
            roomNum = string.Empty;
            userId = string.Empty;
            action = 0;
            var expireTick = DateTime.MinValue.Ticks;
            var token = string.Empty;
            var pairs = authString.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                var values = pair.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 2)
                {
                    switch (values[0])
                    {
                        case "room":
                            roomNum = values[1];
                            break;
                        case "audienceId":
                            userId = values[1];
                            action = 2;
                            break;
                        case "publisher":
                            userId = values[1];
                            action = 1;
                            break;
                        case "expireTime":
                            long.TryParse(values[1], out expireTick);
                            break;
                        case "token":
                            token = values[1];
                            break;
                    }
                }
                else
                {
                    return false;
                }
            }
            if (new DateTime(expireTick) < DateTime.Now) return false;
            string tokenInfo;
            switch (action)
            {
                case 1:
                    tokenInfo = $"room={roomNum}&publisher={userId}&expireTime={expireTick}";
                    break;
                case 2:
                    tokenInfo = $"room={roomNum}&audienceId={userId}&expireTime={expireTick}";
                    break;
                default:
                    return false;
            }
            var compareToken = EncryptUtils.MD5Encrypt(tokenInfo + $"|{ConfigurationValue.SecretKey}");
            return string.CompareOrdinal(token, compareToken) == 0;
        }


        [AllowAnonymous,HttpPost, Route("DvrCallback")]
        public async Task<IHttpActionResult> DvrCallback()
        {
            var stream = await Request.Content.ReadAsStreamAsync();
            stream.Position = 0;
            CallbackResponseBase obj = null;
            using (var sr = new StreamReader(stream))
            {
                var json = await sr.ReadToEndAsync();
                obj = JsonConvert.DeserializeObject<CallbackResponseBase>(json);
                LogHelper.Error($"获取Dvr回调 文件{obj.cwd} /{obj.file}");
            }
            stream.Close();
            StateService.SaveVodItem(obj);
            return Content(HttpStatusCode.OK,0);
        }
        [AllowAnonymous, HttpPost, Route("PublishVertify")]
        public async Task<IHttpActionResult> DefaultPublishVertify()
        {
            var stream = await Request.Content.ReadAsStreamAsync();
            stream.Position = 0;
            CallbackResponseBase obj = null;
            using (var sr = new StreamReader(stream))
            {
                var json = await sr.ReadToEndAsync();
                LogHelper.Error("推流鉴权: " + json);
                obj = JsonConvert.DeserializeObject<CallbackResponseBase>(json);

            }
            stream.Close();
            if (obj == null)
            {
                LogHelper.Error("推流鉴权失败: ");
                return Content(HttpStatusCode.NoContent, 500);
            }

            LogHelper.Error("推流鉴权成功: " + obj);
            BoardCastRoomModel liveRoomModel = await VhostService.GetLiveRoomByStreamName(obj.stream);
            if (liveRoomModel != null)
            {
                Scheduler.GetInstance().ProcessPostTask(liveRoomModel.PublishNotifyUrl, JsonConvert.SerializeObject(new
                {
                    Action = 1,
                    AppName = "live",
                    StreamName = obj.stream,
                }));
                LogHelper.Error("推送信息: " + obj);
            }

            return Content(HttpStatusCode.OK, 0);

        }
        [AllowAnonymous, HttpPost, Route("PublishDone")]
        public async Task<IHttpActionResult> DefaultPublishDone()
        {
            var stream = await Request.Content.ReadAsStreamAsync();
            stream.Position = 0;
            CallbackResponseBase obj = null;
            using (var sr = new StreamReader(stream))
            {
                var json = await sr.ReadToEndAsync();
                obj = JsonConvert.DeserializeObject<CallbackResponseBase>(json);
            }
            stream.Close();
            BoardCastRoomModel liveRoomModel = await VhostService.GetLiveRoomByStreamName(obj.stream);
            if (liveRoomModel != null)
            {
                Scheduler.GetInstance().ProcessPostTask(liveRoomModel.PublishEndUrl, JsonConvert.SerializeObject(new
                {
                    Action = 2,
                    AppName = "live",
                    StreamName = obj.stream,
                }));
                LogHelper.Error("推送信息: " + obj);
            }
            return Content(HttpStatusCode.OK, 0);
        }
        [AllowAnonymous, HttpPost, Route("PlayVertify")]
        public async Task<IHttpActionResult> DefaultPlayVertify()
        {
            var stream = await Request.Content.ReadAsStreamAsync();
            stream.Position = 0;
            CallbackResponseBase obj = null;

            using (var sr = new StreamReader(stream))
            {
                var json = await sr.ReadToEndAsync();
                 obj = JsonConvert.DeserializeObject<CallbackResponseBase>(json);
            }
            stream.Close();
            if (obj == null)
            {
                return Content(HttpStatusCode.NoContent, 500);
            }
            BoardCastRoomModel liveRoomModel = await VhostService.GetLiveRoomByStreamName(obj.stream);
            if (liveRoomModel != null)
            {
                Scheduler.GetInstance().ProcessPostTask(liveRoomModel.PlayNotifyUrl, JsonConvert.SerializeObject(new
                {
                    Action = 3,
                    AppName = "live",
                    StreamName = obj.stream,
                }));
                LogHelper.Error("推送信息: " + obj);
            }
            return Content(HttpStatusCode.OK, 0);
        }
        [AllowAnonymous, HttpPost, Route("PlayDone")]
        public async Task<IHttpActionResult> DefaultPlayDone()
        {
            var stream = await Request.Content.ReadAsStreamAsync();
            stream.Position = 0;
            CallbackResponseBase obj = null;
            using (var sr = new StreamReader(stream))
            {
                var json = await sr.ReadToEndAsync();
                 obj = JsonConvert.DeserializeObject<CallbackResponseBase>(json);
            }
            stream.Close();
            BoardCastRoomModel liveRoomModel = await VhostService.GetLiveRoomByStreamName(obj.stream);
            if (liveRoomModel != null)
            {
                Scheduler.GetInstance().ProcessPostTask(liveRoomModel.PlayEndUrl, JsonConvert.SerializeObject(new
                {
                    Action = 4,
                    AppName = "live",
                    StreamName = obj.stream,
                }));
                LogHelper.Error("推送信息: " + obj);
            }
            return Content(HttpStatusCode.OK, 0);
        }

        [AllowAnonymous, HttpPost, Route("ConnectVertify")]
        public async Task<IHttpActionResult> DefaultConnectVertify()
        {
            try
            {

                var stream = await Request.Content.ReadAsStreamAsync();
                stream.Position = 0;
                CallbackResponseBase callbackInfo = null;
                using (var sr = new StreamReader(stream))
                {
                    var json = await sr.ReadToEndAsync();
                    callbackInfo = JsonConvert.DeserializeObject<CallbackResponseBase>(json);
                }
                stream.Close();
                if (!string.IsNullOrWhiteSpace(callbackInfo.tcUrl))
                {
                    var pairs = callbackInfo.tcUrl.Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (pairs.Length == 2)
                    {
                        string roomNum;
                        string userId;
                        int action = 0; //1: publisher 2:player
                        if (TryVertifyTokenAuth(pairs[1], out roomNum, out userId, out action))
                        {
                            LogHelper.Error("连接鉴权成功: " + callbackInfo.tcUrl);
                            return Content(HttpStatusCode.OK, 0);
                        }
                        else
                        {
                            LogHelper.Error("连接鉴权失败: " + callbackInfo.tcUrl);
                            return Content(HttpStatusCode.Unauthorized, 401);
                        }
                    }
                    else
                    {
                        LogHelper.Error("连接鉴权错误: " + callbackInfo.tcUrl);
                        return Content(HttpStatusCode.Forbidden, 403);
                    }
                }
                else
                {
                    LogHelper.Error("连接鉴权错误: " + callbackInfo.tcUrl);
                    return Content(HttpStatusCode.Forbidden, 403);
                }
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid().Str();
                LogHelper.Error(errorId, e);
                return Content(HttpStatusCode.InternalServerError, 500);
            }
        }

        [AllowAnonymous, HttpPost, Route("DisConnectVertify")]
        public async Task<IHttpActionResult> DefaultDisConnectVertify()
        {
            var stream = await Request.Content.ReadAsStreamAsync();
            stream.Position = 0;
            CallbackResponseBase callbackInfo = null;
            using (var sr = new StreamReader(stream))
            {
                var json = await sr.ReadToEndAsync();
                //callbackInfo = JsonConvert.DeserializeObject<CallbackResponseBase>(json);
            }
            stream.Close();
            return Content(HttpStatusCode.OK, 0);
        }

    }
}
