using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DjLive.Control.Service.Impl;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Impl;
using DjLive.CPDao.Interface;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;
using DjUtil.Tools;
using Newtonsoft.Json;

namespace DjLive.CPService.Impl
{
    public class ServerNodeServiceImpl : IServerNodeServiceInterface
    {
        public IServerDaoInterface ServerDao { get; set; } = new ServerDaoImpl();

        public async Task<int> GetServerNodesCount(string userId, ServerNodeModel delta)
        {
            try
            {
                ServerEntity entityDelta = null;

                if (delta != null)
                {
                    entityDelta = new ServerEntity() { Name = delta.Name };
                }
                return await ServerDao.GetServerNodesCount(userId, entityDelta);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<ServerNodeModel>> GetSimpleServerNodes(string userId, ServerNodeModel delta)
        {
            try
            {
                ServerEntity entityDelta = null;
                if (delta != null)
                {
                    entityDelta = new ServerEntity() { Name = delta.Name };
                }
                var entities = await ServerDao.GetServerEntitys(userId);
                return entities?.Select(item => new ServerNodeModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ServerNodeModel> FindByIdAsync(string userId, string id)
        {
            try
            {
                var entity = await ServerDao.GetServerEntityById(userId, id);
                if (entity != null)
                {
                    return new ServerNodeModel()
                    {
                       Id = entity.Id,
                       Name = entity.Name,
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

        public async Task<ServiceResultMessage> UpdateServerNode(string userId, string id, ServerNodeModel model)
        {
            try
            {
                var Option = new LiveServiceConfig
                {
                    Host = model.Ip,
                    HostUrl = model.Url,
                    SshPort = model.SshPort,
                    UserName = model.UserName,
                    Password = model.Password,
                    ApiPort = model.ApiPort,
                    HttpPort = model.HttpPort,
                    RtmpPort = model.RtmpPort,
                };
                //todo:1.保存设置  3.重新生成配置文件 4.上传配置文件 5.重启SRS服务 
                var daoresult = await ServerDao.UpdateServerEntity(id, new ServerEntity()
                {
                    Name = model?.Name,
                    Ip = model?.Ip,
                    Url = model?.Url,
                    UserName = model?.UserName,
                    Password = model?.Password,
                    Option = JsonConvert.SerializeObject(Option),
                });
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<bool> ServerNodeModelExists(string userId, string id)
        {
            try
            {
                return await GetServerNodesCount(userId, new ServerNodeModel() {Id = id}) >= 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ServiceResultMessage> AddServerNode(string userId, string id, ServerNodeModel model)
        {
            try
            {
                var Option = new LiveServiceConfig
                {
                    Host = model.Ip,
                    HostUrl = model.Url,
                    SshPort = model.SshPort,
                    UserName = model.UserName,
                    Password = model.Password,
                    ApiPort = model.ApiPort,
                    HttpPort = model.HttpPort,
                    RtmpPort = model.RtmpPort,
                };
                var daoresult = await ServerDao.Add2Account(userId, new ServerEntity()
                {
                    Id = id,
                    Name = model?.Name,
                    Ip = model?.Ip,
                    Url = model?.Url,
                    UserName = model?.UserName,
                    Password = model?.Password,
                    Option = JsonConvert.SerializeObject(Option),
                });
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<ServiceResultMessage> RemoveAsync(string userId, string id)
        {
            try
            {
                var daoresult = await ServerDao.DeleteServerEntity(id);
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<ServiceResultMessage> UpdateSrsConf(string userId, string id ="")
        {
            ServiceResultMessage message = new ServiceResultMessage();
            try
            {
                List<ServerEntity> entitys;
                if (string.IsNullOrEmpty(id))
                {
                    entitys = await ServerDao.GetServerEntitys(userId);
                }
                else
                {
                    var entity = await ServerDao.GetServerEntityById(userId, id);
                    entitys = new List<ServerEntity>() {entity};
                }
                foreach (ServerEntity entity in entitys)
                {
                    try
                    {
                        var domains = await ServerDao.GetServerDomainsById(entity.Id);
                        var hostOption = entity.Parse2Conf();
                        hostOption.VHostOptions.AddRange(domains.Parse2Conf());
                        var config = JsonConvert.DeserializeObject<LiveServiceConfig>(entity.Option);
                        var liveService = new DjLiveService(config);
                        liveService.SetLiveServiceConf(hostOption, "test");
                    }
                    catch (Exception e)
                    {
                        message.code = ServiceResultCode.Warning;
                        message.Message += $"更新Server配置失败,ServerId:{entity.Id}";
                        message.ErrorId += "   "+LogHelper.ErrorWithId($"更新Server配置失败,ServerId:{entity.Id}", e);
                    }
                }
                return message;
            }
            catch (Exception e)
            {
                message.code = ServiceResultCode.UpdateSrsError;
                message.Message = e.Message;
                message.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                return message;
            }
        }
    }
}