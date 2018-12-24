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
    public class RecordTemplateServiceImpl : IRecordTemplateServiceInterface
    {
        public IRecordTemplateDaoInterface RecordDao { get; set; } = new RecordTemplateDaoImpl();


        public async Task<int> GetRecordTemplatesCount(string userId, RecordTemplateModel delta)
        {
            try
            {
                RecordTemplateEntity entityDelta = null;

                if (delta != null)
                {
                    entityDelta = new RecordTemplateEntity() { Name = delta.Name };
                }
                return await RecordDao.GetRecordTemplatesCount(userId, entityDelta);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<RecordTemplateModel>> GetSimpleRecordTemplates(string userId, int page, int countPerPage, RecordTemplateModel delta)
        {
            try
            {
                RecordTemplateEntity entityDelta = null;
                if (delta != null)
                {
                    entityDelta = new RecordTemplateEntity() { Name = delta.Name };
                }
                var entities = await RecordDao.GetRecordTemplateEntitys(page, countPerPage, userId, entityDelta);
                return entities?.Select(item => new RecordTemplateModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NamePolicy = item.NamePolicy,
                    UploadUrl = item.UploadUrl,
                }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<RecordTemplateModel> FindByIdAsync(string userId, string id)
        {
            try
            {
                var entity = await RecordDao.GetRecordTemplateEntityById(userId, id);
                if (entity != null)
                {
                    return new RecordTemplateModel()
                    {
                       Id = entity.Id,
                       Name = entity.Name,
                       Description = entity.Description,
                        NamePolicy = entity.NamePolicy,
                        UploadUrl = entity.UploadUrl,
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
        public async Task<ServiceResultMessage> UpdateLogeTemplate(string userId, string id, RecordTemplateModel model)
        {
            try
            {
                //todo:1.保存设置  3.重新生成配置文件 4.上传配置文件 5.重启SRS服务 
                var daoresult = await RecordDao.UpdateRecordTemplateEntity(id, new RecordTemplateEntity()
                {
                    Name = model?.Name,
                    Description = model?.Description,
                    NamePolicy = model?.NamePolicy,
                    UploadUrl = model?.UploadUrl,
                });
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<bool> RecordTemplateModelExists(string userId, string id)
        {
            try
            {
                return await GetRecordTemplatesCount(userId, new RecordTemplateModel() {Id = id}) >= 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ServiceResultMessage> AddLogeTemplate(string userId, string id, RecordTemplateModel model)
        {
            try
            {
                var daoresult = await RecordDao.Add2Account(userId, new RecordTemplateEntity()
                {
                    Id = id,
                    Name = model?.Name,
                    Description = model?.Description,
                    NamePolicy = model?.NamePolicy,
                    UploadUrl = model?.UploadUrl,
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
                var daoresult = await RecordDao.DeleteRecordTemplateEntity(id);
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