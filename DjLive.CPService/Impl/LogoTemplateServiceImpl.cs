using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DjLive.Control.Service.Impl;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Impl;
using DjLive.CPDao.Interface;
using DjLive.CPDao.Util;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;
using DjUtil.Tools;
using DjUtil.Tools.Cryptography;
using Newtonsoft.Json;

namespace DjLive.CPService.Impl
{
    public class LogoTemplateServiceImpl:ILogoServiceInterface
    {
        public class LogoOption
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public ILogoTemplateDaoInterface LogoDao { get; set; } = new LogoTemplateDaoImpl();
        public IServerDaoInterface ServerDao { get; set; } = new ServerDaoImpl();
        public IServerNodeServiceInterface ServerNodeService { get; set; } = new ServerNodeServiceImpl();

        public async Task<int> GetLogoTemplatesCount(string userId, LogoTemplateModel delta)
        {
            try
            {
                LogoTemplateEntity entityDelta = null;

                if (delta != null)
                {
                    entityDelta = new LogoTemplateEntity() { Name = delta.Name, };
                }
                return await LogoDao.GetLogoTemplatesCount(userId, entityDelta);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<LogoTemplateModel>> GetSimpleLogoTemplates(string userId, int page, int countPerPage, LogoTemplateModel delta)
        {
            try
            {
                LogoTemplateEntity entityDelta = null;
                if (delta != null)
                {
                    entityDelta = new LogoTemplateEntity() { Name = delta.Name };
                }
                var entities = await LogoDao.GetLogoTemplates(page, countPerPage, userId, entityDelta);
                return entities?.Select(item => new LogoTemplateModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Base64String = item.Base64Vale,

                }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<LogoTemplateModel> FindByIdAsync(string userId, string id)
        {
            try
            {
                var entity = await LogoDao.GetLogoTemplateById(userId, id);
                if (entity != null)
                {
                    return new LogoTemplateModel()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Base64String = entity.Base64Vale,
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
        public async Task<ServiceResultMessage> UpdateLogeTemplate(string userId, string id, LogoTemplateModel logoTemplateModel)
        {
            ServiceResultMessage message = new ServiceResultMessage();

            try
            {
                string filePath =  string.Concat(ConfigurationValue.TempLogoPath, $"{id}");
                var options = JsonConvert.SerializeObject(new LogoOption()
                {
                    Height = logoTemplateModel.Height,
                    Width = logoTemplateModel.Width,
                });
                var path = EncryptUtils.Base64SaveImg(logoTemplateModel.Base64String, filePath);
                if (path == null)
                {
                    message.code = ServiceResultCode.ImageTranscodeError;
                    message.Message = "base64转 图片失败.";
                    return message;
                }
                var daoresult = await LogoDao.UpdateLogoTemplate(id, new LogoTemplateEntity()
                {
                    Id = id,
                    Name = logoTemplateModel.Name,
                    Base64Vale = logoTemplateModel.Base64String,
                    FilePath = path,
                    Option = options,
                });

                if (daoresult.Code == DaoResultCode.Success)
                {
                    var serverEntities = await ServerDao.GetServerEntitys(userId);
                    foreach (ServerEntity entity in serverEntities)
                    {
                        try
                        {
                            var config = JsonConvert.DeserializeObject<LiveServiceConfig>(entity.Option);
                            var liveService = new DjLiveService(config);
                            liveService.UploadLogoFile(id, path);
                        }
                        catch (Exception e)
                        {
                            message.ErrorId +=
                                ("  " + LogHelper.ErrorWithId($"上传服务器Logo文件失败,ServerId{entity.Id},LogoId:{id}", e));
                            message.Message += $"上传服务器Logo文件失败,ServerId{entity.Id},LogoId:{id}";
                        }
                    }
                    await ServerNodeService.UpdateSrsConf(userId);
                    return message;
                }
                else
                {
                    message = ServiceResultBase.DaoResult2ServiceResult(daoresult);
                }
                return message;
            }
            catch (Exception e)
            {
                message.code = ServiceResultCode.UnExceptError;
                message.Message = e.Message;
                message.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                return message;
            }
        }

        public async Task<bool> LogoTemplateModelExists(string userId, string id)
        {
            try
            {
                return await GetLogoTemplatesCount(userId, new LogoTemplateModel() {Id = id}) >= 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ServiceResultMessage> AddLogeTemplate(string userId, string id, LogoTemplateModel logoTemplateModel)
        {
            ServiceResultMessage message = new ServiceResultMessage();

            try
            {
                string filePath = string.Concat(ConfigurationValue.TempLogoPath, $"{id}");
                var options = JsonConvert.SerializeObject(new LogoOption()
                {
                    Height = logoTemplateModel.Height,
                    Width = logoTemplateModel.Width,
                });
                var path = EncryptUtils.Base64SaveImg(logoTemplateModel.Base64String, filePath);
                if (path == null)
                {
                    message.code = ServiceResultCode.ImageTranscodeError;
                    message.Message = "base64转 图片失败.";
                    return message;
                }
                var daoresult = await LogoDao.Add2Account(userId, new LogoTemplateEntity()
                {
                    Id = id,
                    Name = logoTemplateModel.Name,
                    Base64Vale = logoTemplateModel.Base64String,
                    FilePath = path,
                    Option = options,
                });

                if (daoresult.Code == DaoResultCode.Success)
                {
                    var serverEntities = await ServerDao.GetServerEntitys(userId);
                    foreach (ServerEntity entity in serverEntities)
                    {
                        try
                        {
                            var config = JsonConvert.DeserializeObject<LiveServiceConfig>(entity.Option);
                            var liveService = new DjLiveService(config);
                            liveService.UploadLogoFile(id, path);
                        }
                        catch (Exception e)
                        {
                            message.ErrorId +=
                                ("  " + LogHelper.ErrorWithId($"上传服务器Logo文件失败,ServerId{entity.Id},LogoId:{id}", e));
                            message.Message += $"上传服务器Logo文件失败,ServerId{entity.Id},LogoId:{id}";
                        }
                    }
                    await ServerNodeService.UpdateSrsConf(userId);
                    return message;
                }
                else
                {
                    message = ServiceResultBase.DaoResult2ServiceResult(daoresult);
                }
                return message;
            }
            catch (NullReferenceException e)
            {
                message.code = ServiceResultCode.SaveFileError;
                message.Message = e.Message;
                message.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                return message;
            }
            catch (Exception e)
            {
                message.code = ServiceResultCode.UnExceptError;
                message.Message = e.Message;
                message.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                return message;
            }
        }

        public async Task<ServiceResultMessage> RemoveAsync(string userId, string id)
        {
            ServiceResultMessage message = new ServiceResultMessage();
            try
            {
                var daoResult = await LogoDao.DeleteLogoTemplate(id);
                if (daoResult.Code == DaoResultCode.Success)
                {
                    var logo = daoResult.para as LogoTemplateEntity;
                    if (logo != null&& File.Exists(logo.FilePath))
                    {
                        try
                        {
                            File.Delete(logo.FilePath);
                        }
                        catch (Exception e)
                        {
                            LogHelper.Error($"删除Logo文件失败: 路径:{logo.FilePath}");
                        }
                    }
                    var serverEntities = await ServerDao.GetServerEntitys(userId);
                    foreach (ServerEntity entity in serverEntities)
                    {
                        try
                        {
                            var config = JsonConvert.DeserializeObject<LiveServiceConfig>(entity.Option);
                            var liveService = new DjLiveService(config);
                            liveService.RemoveLogoFile(id);
                        }
                        catch (Exception e)
                        {
                            message.ErrorId +=
                                ("  " + LogHelper.ErrorWithId($"删除服务器Logo文件失败,ServerId{entity.Id},LogoId:{id}", e));
                            message.Message += $"删除服务器Logo文件失败,ServerId{entity.Id},LogoId:{id}";
                        }
                    }
                    var updateResult = await ServerNodeService.UpdateSrsConf(userId);
                    message = updateResult;
                    if (updateResult.code == ServiceResultCode.Warning)
                    {
                        message.code = ServiceResultCode.Success;
                    }
                    await ServerNodeService.UpdateSrsConf(userId);
                }
                else
                {
                    message = ServiceResultBase.DaoResult2ServiceResult(daoResult);
                }
                return message;
            }
            catch (Exception e)
            {
                message.code = ServiceResultCode.UnExceptError;
                message.Message = e.Message;
                message.ErrorId = LogHelper.ErrorWithId(e.Message, e);
                return message;
            }
        }
    }
}