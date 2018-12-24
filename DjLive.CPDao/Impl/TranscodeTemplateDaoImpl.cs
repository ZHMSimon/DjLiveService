using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DjLive.CPDao.Context;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Interface;
using DjLive.CPDao.Util;
using DjUtil.Tools;

namespace DjLive.CPDao.Impl
{
    public class TranscodeTemplateDaoImpl:ITranscodeTemplateDaoInterface
    {
        public async Task<DaoResultMessage> CreateTranscodeTemplateEntity(string id, TranscodeTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    entity.Id = id;
                    var item = context.TranscodeTemplate.Add(entity);
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

        public async Task<DaoResultMessage> DeleteTranscodeTemplateEntity(string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var template = context.TranscodeTemplate.FirstOrDefault(obj => obj.Id == id);
                    if (template != null)
                    {
                        context.TranscodeTemplate.Remove(template);
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

        public async Task<DaoResultMessage> UpdateTranscodeTemplateEntity(string id, TranscodeTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.TranscodeTemplate.FirstOrDefault(obj => obj.Id == id);

                    if (dbentity != null && entity != null)
                    {
                        dbentity.AudioOption = string.IsNullOrEmpty(entity.AudioOption) ? dbentity.AudioOption : entity.AudioOption;
                        dbentity.VideoOption = string.IsNullOrEmpty(entity.VideoOption) ? dbentity.VideoOption : entity.VideoOption;
                        dbentity.Name = string.IsNullOrEmpty(entity.Name) ? dbentity.Name : entity.Name;
                        context.TranscodeTemplate.Attach(dbentity);
                        context.Entry(dbentity).Property(x => x.Id).IsModified = false;
                        context.Entry(dbentity).Property(x => x.AudioOption).IsModified = true;
                        context.Entry(dbentity).Property(x => x.VideoOption).IsModified = true;
                        context.Entry(dbentity).Property(x => x.Name).IsModified = true;
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

        public async Task<List<TranscodeTemplateEntity>> GetTranscodeTemplateEntitys(int page, int countPerPage, string userId = "", TranscodeTemplateEntity delta = null)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT * FROM sys_transcodetemplate  
                                 limit {(page - 1) * countPerPage},{countPerPage}";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        sql = $@"SELECT * FROM sys_transcodetemplate as b 
                                join sys_alli_account_transcodetemplate as c 
                                on c.AccountId = {"{0}"} 
                                and b.Id = c.TemplateId limit {(page - 1) * countPerPage},{countPerPage}";
                        paraObjects = new object[] { userId };
                    }
                    var objs = context.Database.SqlQuery<TranscodeTemplateEntity>(sql, paraObjects);
                    return await objs.ToListAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
        public async Task<int> GetTranscodeTemplatesCount(string userId, TranscodeTemplateEntity entityDelta)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT Count(0) FROM sys_transcodetemplate";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        sql = $@"SELECT Count(0) FROM sys_transcodetemplate as b 
                                join sys_alli_account_transcodetemplate as c 
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

        public async Task<TranscodeTemplateEntity> GetTranscodeTemplateEntitysById(string accountId, string id)
        {
            try
            {
                if (id == null) return null;
                using (var context = new DjLiveCpContext())
                {
                    var paraObjects = new List<object>();
                    paraObjects.Add(accountId);
                    paraObjects.Add(id);
                     var sql = @"SELECT * FROM sys_transcodetemplate as b 
                                join sys_alli_account_transcodetemplate as c 
                                on c.AccountId = {0} and c.TemplateId = {1}
                                and b.Id = c.TemplateId ";
                    var objs = context.Database.SqlQuery<TranscodeTemplateEntity>(sql, paraObjects.ToArray());
                    var obj = await objs.FirstOrDefaultAsync();
                    if (obj != null)
                    {
                        obj.LogoTemplate = await context.LogoTemplate.FirstOrDefaultAsync(item => item.Id == obj.Id);
                    }
                    return obj;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<DaoResultMessage> Append2Account(string accountId, string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var template = context.TranscodeTemplate.FirstOrDefault(obj => obj.Id == id);
                    if (dbentity != null && template != null)
                    {
                        template.AccountEntities.Add(dbentity);
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

        public async Task<DaoResultMessage> Add2Account(string accountId, TranscodeTemplateEntity entity)
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
                        context.TranscodeTemplate.Add(entity);
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

         public async Task<DaoResultMessage> AppendLogo(string transcodeId, string logoId)
        {
            if (string.IsNullOrEmpty(logoId)) return DaoResultMessage.OkWithParaNull;
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.TranscodeTemplate.FirstOrDefault(obj => obj.Id == transcodeId);
                    var entity = context.LogoTemplate.FirstOrDefault(obj => obj.Id == logoId);
                    if (dbentity != null && entity != null)
                    {
                        dbentity.LogoTemplate = entity;
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