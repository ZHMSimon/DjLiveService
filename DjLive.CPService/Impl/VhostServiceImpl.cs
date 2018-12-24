using System.Collections.Generic;
using DjLive.CPDao.Interface;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;

using DjLive.Control.Model.ConfModel.LiveService;
using DjLive.CPDao.Entity;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DjLive.Control.Service.Impl;
using DjLive.SdkModel.Enum;
using DjUtil.Tools;
using DjLive.Control.Service;
using DjLive.CPDao.Impl;
using DjLive.CPDao.Util;


namespace DjLive.CPService.Impl
{
    public class VhostServiceImpl:IVhostServiceInterface
    {
        public IBoardcastRoomDaoInterface BoardcastRoomDao { get; set; }= new BoardcastRoomDaoImpl();
        public ISecurePolicyDaoInterface SecurePolicyDao { get; set; } = new SecurePolicyDaoImpl();
        public IDomainDaoInterface DomainDao { get; set; } = new DomainDaoImpl();
        public ITranscodeTemplateDaoInterface TranscodeTemplateDao { get; set; } = new TranscodeTemplateDaoImpl();
        public ILogoTemplateDaoInterface LogoTemplateDao { get; set; } = new LogoTemplateDaoImpl();
        public IAppDaoInterface AppDao { get; set; } = new AppDaoImpl();
        public IStreamDaoInterface StreamDao { get; set; } = new StreamDaoImpl();
        public IAccountDaoInterface AccountDao { get; set; } = new AccountDaoImpl();
        public IServerDaoInterface ServerDao { get; set; } = new ServerDaoImpl();
        public IServerNodeServiceInterface ServerNodeService { get; set; } = new ServerNodeServiceImpl();

        public async Task<ServiceResultMessage> CreateVhost(string userId, DomainModel model)
        {
            ServiceResultMessage result = new ServiceResultMessage();
            var id = string.IsNullOrEmpty(model.Id) ? Guid.NewGuid().Str() : model.Id;
            try
            {
                var daomainResult = await DomainDao.Add2Account(userId,model.ServerPair.Key,new DomainEntity()
                {
                    Id = id,
                    SourceDomain = model.SourceDomain,
                    Description = model.Description,
                    SourceType = (int)model.SourceType,
                    StateType = 1,
                    RtmpPlayDomain = model.RtmpPlayDomain,
                    FlvPlayDomain = model.FlvPlayDomain,
                    HlsPlayDomain = model.HlsPlayDomain
                });
                if (daomainResult.Code == DaoResultCode.Success)
                {
                    var secureAppendtask = DomainDao.AppendSecure(id,model.SecurePolicyPair.Key);
                    var transcodeAppendtask = DomainDao.AppendTranscode(id,model.TranscodeIdPair?.Select(item => item.Key).ToList());
                    var resultsMessages = await Task.WhenAll(secureAppendtask, transcodeAppendtask);
                    if (resultsMessages.Count(item=>item.Code != DaoResultCode.Success)>0)
                    {
                        var rollbackResult = await DomainDao.DeleteDomainById(id);
                        result.code = ServiceResultCode.AppendItemError;
                    }
                    else
                    {
                        try
                        {
                            var updateConfResult = await ServerNodeService.UpdateSrsConf(userId, model.ServerPair.Key);
                            if (updateConfResult.code != ServiceResultCode.Success)
                            {
                                var rollbackResult = await DomainDao.DeleteDomainById(id);
                                result.code = ServiceResultCode.UpdateSrsError;
                                result.Message = updateConfResult.Message;
                                result.ErrorId = updateConfResult.ErrorId;
                            }
                        }
                        catch (Exception e)
                        {
                            var rollbackResult = await DomainDao.DeleteDomainById(id);
                            result.code = ServiceResultCode.UpdateSrsError;
                            result.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                            result.Message = "更新Srs服务器失败,请联系管理员,错误代码:" + result.ErrorId;
                        }
                    }
                }
                else
                {
                    result.code = ServiceResultCode.UnExceptError;
                }
            }
            catch (Exception e)
            {
                var rollbackResult = await DomainDao.DeleteDomainById(id);
                LogHelper.Error(e.Message, e);
                result.code = ServiceResultCode.UnExceptError;
                result.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                result.Message = "发生未知错误,请联系管理员,错误代码:"+ result.ErrorId;
            }
            return result;
        }

        public async Task<int> GetDomainsCount(string userId, DomainModel delta)
        {
            try
            {
                DomainEntity entityDelta = null;
                if (delta != null)
                {
                    entityDelta = new DomainEntity() {  };
                }
                return await DomainDao.GetDomainCount(userId, entityDelta);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<DomainModel>> GetSimpleDomains(string userId, int page, int countPerPage, DomainModel delta)
        {
            try
            {
                DomainEntity entityDelta = null;
                if (delta != null)
                {
                    entityDelta = new DomainEntity() {  };
                }
                var entities = await DomainDao.GetDomainEntities(page, countPerPage, userId, entityDelta);
                return entities?.Select(item => new DomainModel()
                {
                    Id = item.Id,
                    SourceDomain = item.SourceDomain,
                    Description = item.Description,
                    FlvPlayDomain = item.FlvPlayDomain,
                    HlsPlayDomain = item.HlsPlayDomain,
                    RtmpPlayDomain = item.RtmpPlayDomain,
                }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<DomainModel> FindByIdAsync(string userId, string id)
        {
            try
            {
                var entity = await DomainDao.GetDomainEntityById(id);
                if (entity != null)
                {
                    return new DomainModel()
                    {
                        Id = entity.Id,
                        ServerPair = new KeyNamePair(entity.Server?.Id,entity.Server?.Name),
                        SourceDomain = entity.SourceDomain,
                        Description = entity.Description,
                        SourceType = (SourceStreamType)entity.SourceType,
                        StateType = (StateType)entity.StateType,

                        RtmpPlayDomain = entity.RtmpPlayDomain,
                        FlvPlayDomain = entity.FlvPlayDomain,
                        HlsPlayDomain = entity.HlsPlayDomain,

                        SecurePolicyPair = new KeyNamePair(entity.SecurePolicy?.Id,entity.SecurePolicy?.Name),
                        RecordTemplatePair = new KeyNamePair(entity.RecordTemplate?.Id,entity.RecordTemplate?.Name),
                        TranscodeIdPair = entity.TranscodeList.Select(item =>
                        {
                           return new KeyNamePair(item.Id,item.Name);
                        }).ToList()
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
        //todo:UnFinish
        public async Task<ServiceResultMessage> UpdateDomain(string userId, string id, DomainModel model)
        {
            ServiceResultMessage result = new ServiceResultMessage();
            try
            {
                //todo:1.保存设置  3.重新生成配置文件 4.上传配置文件 5.重启SRS服务 
                var daoresult = await DomainDao.UpdataDomain(id, new DomainEntity()
                {
                    Id = model.Id,
                    SourceDomain = model.SourceDomain,
                    Description = model.Description,
                    SourceType = (int)model.SourceType,
                    StateType = (int)model.StateType,

                    RtmpPlayDomain = model.RtmpPlayDomain,
                    FlvPlayDomain = model.FlvPlayDomain,
                    HlsPlayDomain = model.HlsPlayDomain,


                });
                if (daoresult.Code == DaoResultCode.Success)
                {
                    var secureAppendtask = DomainDao.AppendSecure(id, model?.SecurePolicyPair.Key);
                    var transcodeAppendtask = DomainDao.AppendTranscode(id, model?.TranscodeIdPair?.Select(item => item.Key).ToList());
                    var resultsMessages = await Task.WhenAll( secureAppendtask, transcodeAppendtask);
                    if (resultsMessages.Count(item => item.Code != DaoResultCode.Success) > 0)
                    {
                        result.code = ServiceResultCode.AppendItemError;
                    }
                    else
                    {
                        try
                        {
                            var updateConfResult = await ServerNodeService.UpdateSrsConf(userId, model.ServerPair.Key);
                            if (updateConfResult.code != ServiceResultCode.Success)
                            {
                                var rollbackResult = await DomainDao.DeleteDomainById(id);
                                result.code = ServiceResultCode.UpdateSrsError;
                                result.Message = updateConfResult.Message;
                                result.ErrorId = updateConfResult.ErrorId;
                            }
                        }
                        catch (Exception e)
                        {
                            var rollbackResult = await DomainDao.DeleteDomainById(id);
                            result.code = ServiceResultCode.UpdateSrsError;
                            result.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                            result.Message = "更新Srs服务器失败,请联系管理员,错误代码:" + result.ErrorId;
                        }
                    }
                }
                else
                {
                    result.code = ServiceResultCode.UnExceptError;
                }
                return result;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                result.code = ServiceResultCode.UnExceptError;
                result.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                result.Message = "发生未知错误,请联系管理员,错误代码:" + result.ErrorId;
                return result;
            }
        }

        public async Task<bool> DomainModelExists(string userId, string id)
        {
            try
            {
                return await GetDomainsCount(userId, new DomainModel() { Id = id }) >= 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ServiceResultMessage> RemoveAsync(string userId, string id)
        {
            try
            {
                //todo:1.保存设置 2.删除水印图片 3.重新生成配置文件 4.上传配置文件 5.重启SRS服务 
                var daoresult = await DomainDao.DeleteDomainById(id);
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<ServiceResultMessage> CreateLiveRoom(string userId,BoardCastRoomModel model)
        {
            if (model ==null)return new ServiceResultMessage() {code = ServiceResultCode.UnDefineError,Message = "传入对象为空,请检查参数.."};
            try
            {
                // var domain = DomainDao.GetDomainCount(userId);//通过Domain 确定是否存在DomainNode

                var entity = new BoardCastRoomEntity()
                {
                    Id = model.Id,
                    Domain = model.Domain,
                    Name = model.Name,
                    AppName = string.IsNullOrEmpty(model.AppName) ? "live" : model.AppName,
                    ExpireTime = model.ExpireTime.Equals(new DateTime()) ? DateTime.Now.AddDays(2) : model.ExpireTime,
                    PlayEndNotifyUrl = model.PlayEndUrl,
                    PlayNotifyUrl = model.PlayNotifyUrl,
                    PublishEndNotifyUrl = model.PublishEndUrl,
                    PublishNotifyUrl = model.PublishNotifyUrl,
                    StreamName = model.StreamName.ToLower(),
                    State = 1,
                };
                DaoResultMessage daoresult = await BoardcastRoomDao.CreateRoom(entity);
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<BoardCastRoomModel> GetLiveRoomByStreamName(string streamName)
        {
            try
            {
                BoardCastRoomEntity entity = await BoardcastRoomDao.GetBoardCastRoomBySteamName(streamName.ToLower());
                if (entity == null)
                    return null;

                return new BoardCastRoomModel()
                {
                    Id = entity.Id,
                    Domain = entity.Domain,
                    Name = entity.Name,
                    AppName = entity.AppName,
                    ExpireTime = entity.ExpireTime,
                    PlayEndUrl  = entity.PlayEndNotifyUrl,
                    PlayNotifyUrl = entity.PlayNotifyUrl,
                    PublishEndUrl  = entity.PublishEndNotifyUrl,
                    PublishNotifyUrl = entity.PublishNotifyUrl,
                    StreamName = entity.StreamName,
                    State = entity.State,
                };
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
    }
}