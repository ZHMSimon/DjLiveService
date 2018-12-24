using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class TranscodeTemplateServiceImpl:ITranscodeTemplateServiceInterface
    {
        public ITranscodeTemplateDaoInterface TranscodeDao { get; set; } = new TranscodeTemplateDaoImpl();


        public async Task<int> GetTranscodeTemplatesCount(string userId, TranscodeTemplateModel delta)
        {
            try
            {
                TranscodeTemplateEntity entityDelta = null;

                if (delta != null)
                {
                    entityDelta = new TranscodeTemplateEntity() { Name = delta.Name };
                }
                return await TranscodeDao.GetTranscodeTemplatesCount(userId, entityDelta);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<TranscodeTemplateModel>> GetSimpleTranscodeTemplates(string userId, int page, int countPerPage, TranscodeTemplateModel delta)
        {
            try
            {
                TranscodeTemplateEntity entityDelta = null;
                if (delta != null)
                {
                    entityDelta = new TranscodeTemplateEntity() { Name = delta.Name };
                }
                var entities = await TranscodeDao.GetTranscodeTemplateEntitys(page, countPerPage, userId, entityDelta);
                return entities?.Select(item => new TranscodeTemplateModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    AppName = item.AppName,
                    Description = item.Name,
                }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<TranscodeTemplateModel> FindByIdAsync(string userId, string id)
        {
            try
            {
                var entity = await TranscodeDao.GetTranscodeTemplateEntitysById(userId, id);
                if (entity != null)
                {
                    return new TranscodeTemplateModel()
                    {
                       Id = entity.Id,
                       Name = entity.Name,
                       AppName = entity.AppName,
                       Description = entity.Name,
                       AudioOption = JsonConvert.DeserializeObject<AudioOptionModel>(entity.AudioOption),
                       VideoOption = JsonConvert.DeserializeObject<VideoOptionModel>(entity.AudioOption)
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
        public async Task<ServiceResultMessage> UpdateLogeTemplate(string userId, string id, TranscodeTemplateModel model)
        {
            try
            {
                //todo:1.保存设置  3.重新生成配置文件 4.上传配置文件 5.重启SRS服务 
                var daoresult = await TranscodeDao.UpdateTranscodeTemplateEntity(id, new TranscodeTemplateEntity()
                {
                    Name = model?.Name,
                    AppName = model?.AppName,
                    AudioOption = JsonConvert.SerializeObject(model?.AudioOption),
                    VideoOption = JsonConvert.SerializeObject(model?.AudioOption)
                });
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<bool> TranscodeTemplateModelExists(string userId, string id)
        {
            try
            {
                return await GetTranscodeTemplatesCount(userId, new TranscodeTemplateModel() {Id = id}) >= 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ServiceResultMessage> AddLogeTemplate(string userId, string id, TranscodeTemplateModel model)
        {
            try
            {
                var daoresult = await TranscodeDao.Add2Account(userId, new TranscodeTemplateEntity()
                {
                    Id = id,
                    Name = model?.Name,
                    AppName = model?.AppName,
                    AudioOption = JsonConvert.SerializeObject(model?.AudioOption),
                    VideoOption = JsonConvert.SerializeObject(model?.AudioOption)
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
                //todo:1.保存设置 3.重新生成配置文件 4.上传配置文件 5.重启SRS服务 
                var daoresult = await TranscodeDao.DeleteTranscodeTemplateEntity(id);
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
    }
}