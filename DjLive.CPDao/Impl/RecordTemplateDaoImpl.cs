using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DjLive.CPDao.Context;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Interface;
using DjLive.CPDao.Util;
using DjUtil.Tools;

namespace DjLive.CPDao.Impl
{
    public class RecordTemplateDaoImpl:IRecordTemplateDaoInterface
    {
        public async Task<DaoResultMessage> CreateRecordTemplateEntity(string id, RecordTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    entity.Id = id;
                    var item = context.RecordTemplate.Add(entity);
                    await context.SaveChangesAsync();
                    message.Code = DaoResultCode.Success;
                    message.Message = "添加成功!";
                    message.para = item;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }

        public async Task<DaoResultMessage> DeleteRecordTemplateEntity(string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var recordTemplate = context.RecordTemplate.FirstOrDefault(obj => obj.Id == id);
                    if (recordTemplate != null)
                    {
                        context.RecordTemplate.Remove(recordTemplate);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        message.Code = DaoResultCode.ItemNotExist;
                        message.Message = "修改失败,对象不存在!";
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }

        public async Task<DaoResultMessage> UpdateRecordTemplateEntity(string id, RecordTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.RecordTemplate.FirstOrDefault(obj => obj.Id == id);

                    if (dbentity != null && entity != null)
                    {
                        dbentity.Description = entity.Description;
                        dbentity.NamePolicy = entity.NamePolicy;
                        dbentity.UploadUrl = entity.UploadUrl;

                        context.RecordTemplate.Attach(dbentity);
                        context.Entry(dbentity).Property(x => x.Id).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Name).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Description).IsModified = !entity.Description.IsStringNull();
                        context.Entry(dbentity).Property(x => x.NamePolicy).IsModified = !entity.NamePolicy.IsStringNull();
                        context.Entry(dbentity).Property(x => x.UploadUrl).IsModified = !entity.UploadUrl.IsStringNull();
                        await context.SaveChangesAsync();
                        message.Code = DaoResultCode.Success;
                        message.Message = "修改成功!";
                        message.para = entity;
                    }
                    else
                    {
                        message.Code = DaoResultCode.ItemNotExist;
                        message.Message = "修改失败,对象不存在!";
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }

        public async Task<List<RecordTemplateEntity>> GetRecordTemplateEntitys(int page, int countPerPage, string userId = "", RecordTemplateEntity delta = null)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT * FROM sys_recordtemplate  
                                 limit {(page - 1) * countPerPage},{countPerPage}";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        sql = $@"SELECT * FROM sys_recordtemplate as b 
                                join sys_alli_account_recordtemplate as c 
                                on c.AccountId = {"{0}"}  
                                and b.Id = c.TemplateId limit {(page - 1) * countPerPage},{countPerPage}";
                        paraObjects = new object[] { userId };
                    }
                    var objs = context.Database.SqlQuery<RecordTemplateEntity>(sql, paraObjects);
                    return await objs.ToListAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }

        }

        public async Task<int> GetRecordTemplatesCount(string userId, RecordTemplateEntity entityDelta)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT Count(0) FROM sys_recordtemplate";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        sql = $@"SELECT Count(0) FROM sys_recordtemplate as b 
                                join sys_alli_account_recordtemplate as c 
                                on c.AccountId = {"{0}"} 
                                and b.Id = c.TemplateId";
                        paraObjects = new object[] { userId };
                    }
                    var objs = context.Database.SqlQuery<int>(sql, paraObjects);
                    return await objs.FirstOrDefaultAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return 0;
            }
        }

        public async Task<RecordTemplateEntity> GetRecordTemplateEntityById(string accountId, string id)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    return await context.RecordTemplate.FirstOrDefaultAsync(item => item.Id == id);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<DaoResultMessage> Append2Account(string accountId, string policyId)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var recordTemplate = context.RecordTemplate.FirstOrDefault(obj => obj.Id == policyId);
                    if (dbentity != null && recordTemplate != null)
                    {
                        recordTemplate.AccountEntities.Add(dbentity);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        message.Code = DaoResultCode.ItemNotExist;
                        message.Message = "修改失败,对象不存在!";
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }

        public async Task<DaoResultMessage> Add2Account(string accountId, RecordTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    if (dbentity != null)
                    {
                        entity.AccountEntities.Add(dbentity);
                        context.RecordTemplate.Add(entity);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        message.Code = DaoResultCode.ItemNotExist;
                        message.Message = "修改失败,对象不存在!";
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }

        public async Task<DaoResultMessage> Append2Domain(string domainId, string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.TranscodeTemplate.FirstOrDefault(obj => obj.Id == id);
                    var domain = context.Domain.FirstOrDefault(obj => obj.Id == domainId);
                    if (dbentity != null && domain != null)
                    {
                        dbentity.Domains.Add(domain);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        message.Code = DaoResultCode.ItemNotExist;
                        message.Message = "修改失败,对象不存在!";
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }
    }
}