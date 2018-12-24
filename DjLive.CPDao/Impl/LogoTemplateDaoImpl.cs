using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DjLive.CPDao.Context;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Interface;
using DjLive.CPDao.Util;
using DjUtil.Tools;

namespace DjLive.CPDao.Impl
{
    public class LogoTemplateDaoImpl:ILogoTemplateDaoInterface
    {
        public async Task<DaoResultMessage> CreateLogoTemplate(string id, LogoTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    entity.Id = id;
                    var item = context.LogoTemplate.Add(entity);
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

        public async Task<DaoResultMessage> DeleteLogoTemplate(string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var entity = context.LogoTemplate.FirstOrDefault(obj => obj.Id == id);
                    if (entity != null)
                    {
                        context.LogoTemplate.Remove(entity);
                        await context.SaveChangesAsync();
                        message.Code = DaoResultCode.Success;
                        message.Message = "删除成功!";
                        message.para = entity;
                    }
                    else
                    {
                        message.Code = DaoResultCode.ItemNotExist;
                        message.Message = "删除失败,对象不存在!";
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

        public async Task<DaoResultMessage> UpdateLogoTemplate(string id, LogoTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.LogoTemplate.FirstOrDefault(obj => obj.Id == id);

                    if (dbentity != null && entity != null)
                    {
                        dbentity.Option =  entity.Option;
                        dbentity.Base64Vale = entity.Base64Vale;
                        dbentity.FilePath =  entity.FilePath;

                        context.LogoTemplate.Attach(dbentity);
                        context.Entry(dbentity).Property(x => x.Id).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Name).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Option).IsModified = !entity.Option.IsStringNull();
                        context.Entry(dbentity).Property(x => x.Base64Vale).IsModified = !entity.Base64Vale.IsStringNull();
                        context.Entry(dbentity).Property(x => x.FilePath).IsModified = !entity.FilePath.IsStringNull();
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

        public async Task<List<LogoTemplateEntity>> GetLogoTemplates(int page, int countPerPage, string accountId = "", LogoTemplateEntity delta = null)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT * FROM sys_logotemplate  
                                 limit {(page - 1) * countPerPage},{countPerPage}";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        sql = $@"SELECT * FROM sys_logotemplate where
                                 AccountId = {"{0}"} limit {(page - 1) * countPerPage},{countPerPage}";
                        paraObjects = new object[] { accountId };
                    }
                    var objs = context.Database.SqlQuery<LogoTemplateEntity>(sql, paraObjects);
                    return await objs.ToListAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }

        }

        public async Task<int> GetLogoTemplatesCount(string accountId = "", LogoTemplateEntity delta = null)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var paraObjects = new object[] { };
                    var sql = $@"SELECT Count(0) FROM sys_logotemplate ";
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        sql = $@"SELECT Count(0) FROM sys_logotemplate where
                                 AccountId = {"{0}"} ";
                        paraObjects = new object[] { accountId };
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



        public async Task<LogoTemplateEntity> GetLogoTemplateById(string accountId, string id)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    return await context.LogoTemplate.FirstOrDefaultAsync(item => item.AccountEntity.Id == accountId && item.Id == id);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                throw;
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
                    var enity = context.LogoTemplate.FirstOrDefault(obj => obj.Id == id);
                    if (dbentity != null && enity != null)
                    {
                        enity.AccountEntity = dbentity;
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

        public async Task<DaoResultMessage> Add2Account(string accountId, LogoTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = await context.Account.FirstOrDefaultAsync(obj => obj.Id == accountId);
                    if (dbentity != null)
                    {
                        entity.AccountEntity = dbentity;
                        context.LogoTemplate.Add(entity);
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