using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DjLive.CPDao.Context;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Interface;
using DjLive.CPDao.Util;
using DjUtil.Tools;

namespace DjLive.CPDao.Impl
{
    public class DomainDaoImpl:IDomainDaoInterface
    {
        public async Task<DaoResultMessage> CreateDomain(string id, DomainEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    entity.Id = id;
                    var item = context.Domain.Add(entity);
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

        public async Task<DaoResultMessage> UpdataDomain(string id, DomainEntity entity)
        {
            //todo:修改 包含 各种模版
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Domain.FirstOrDefault(obj => obj.Id == id);

                    if (dbentity != null && entity != null)
                    {
                        dbentity.Description = string.IsNullOrEmpty(entity.Description) ? dbentity.Description : entity.Description;
                        dbentity.FlvPlayDomain = string.IsNullOrEmpty(entity.FlvPlayDomain) ? dbentity.FlvPlayDomain : entity.FlvPlayDomain;
                        dbentity.HlsPlayDomain = string.IsNullOrEmpty(entity.HlsPlayDomain) ? dbentity.HlsPlayDomain : entity.HlsPlayDomain;
                        dbentity.RtmpPlayDomain = string.IsNullOrEmpty(entity.RtmpPlayDomain) ? dbentity.RtmpPlayDomain : entity.RtmpPlayDomain;

                        context.Domain.Attach(dbentity);
                        context.Entry(dbentity).Property(x => x.Id).IsModified = false;
                        context.Entry(dbentity).Property(x => x.SourceDomain).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Description).IsModified = true;
                        context.Entry(dbentity).Property(x => x.FlvPlayDomain).IsModified = true;
                        context.Entry(dbentity).Property(x => x.HlsPlayDomain).IsModified = true;
                        context.Entry(dbentity).Property(x => x.RtmpPlayDomain).IsModified = true;
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

        public async Task<DaoResultMessage> DeleteDomainById(string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var entity = context.Domain.FirstOrDefault(obj => obj.Id == id);
                    if (entity != null)
                    {
                        context.Domain.Remove(entity);
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
        public async Task<List<DomainEntity>> GetDomainEntities(int page, int countPerPage,string accountId = "", DomainEntity delta = null)
        {
            //todo:增加筛选
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT * FROM sys_domain  
                                 limit {(page - 1) * countPerPage},{countPerPage}";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        sql = $@"SELECT * FROM sys_domain where
                                 AccountId = {"{0}"} limit {(page - 1) * countPerPage},{countPerPage}";
                        paraObjects = new object[] { accountId };
                    }
                    var objs = context.Database.SqlQuery<DomainEntity>(sql, paraObjects);
                    return await objs.ToListAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<DomainEntity> GetDomainEntityById(string id)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    return await context.Domain.Include("Server").Include("RecordTemplate").Include("SecurePolicy").Include("TranscodeList").FirstOrDefaultAsync(item => item.Id == id);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<DaoResultMessage> Append2Account(string accountId,string serverId, string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var entity = context.Domain.FirstOrDefault(obj => obj.Id == id);
                    var serverEntity = context.Server.FirstOrDefault(obj => obj.Id == serverId);
                    if (dbentity != null && entity != null&& serverEntity != null)
                    {
                        entity.Account = dbentity;
                        entity.Server = serverEntity;
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

        public async Task<DaoResultMessage> Add2Account(string accountId,string serverId, DomainEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    if ( await context.Domain.CountAsync(item=>item.SourceDomain == entity.SourceDomain)>=1)
                    {
                        message.Code = DaoResultCode.ItemAlreadyExist;
                        message.Message = "修改失败,对象已经存在!";
                    }
                    else
                    {
                        var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                        var serverEntity = context.Server.FirstOrDefault(obj => obj.Id == serverId);
                        if (dbentity != null && serverEntity != null)
                        {
                            entity.Account = dbentity;
                            entity.Server = serverEntity;
                            context.Domain.Add(entity);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            message.Code = DaoResultCode.ItemNotExist;
                            message.Message = "修改失败,对象不存在!";
                        }
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

        public async Task<DaoResultMessage> Append2Server(string serverId, string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Server.FirstOrDefault(obj => obj.Id == serverId);
                    var entity = context.Domain.FirstOrDefault(obj => obj.Id == id);
                    if (dbentity != null && entity != null)
                    {
                        entity.Server = dbentity;
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

        public async Task<DaoResultMessage> AppendTranscode(string domainId, List<string> templateIds)
        {
            if (templateIds == null||templateIds.Count<=0) return DaoResultMessage.OkWithParaNull;
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Domain.FirstOrDefault(obj => obj.Id == domainId);
                    foreach (var templateId in templateIds)
                    {
                        var entity = context.TranscodeTemplate.FirstOrDefault(obj => obj.Id == templateId);
                        if (dbentity != null && entity != null)
                        {
                            dbentity.TranscodeList.Add(entity);
                        }
                        else
                        {
                            return DaoResultMessage.ItemNotExist;
                        }
                    }
                    await context.SaveChangesAsync();

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

       
        public async Task<DaoResultMessage> AppendSecure(string domainId, string policyId)
        {
            if (string.IsNullOrEmpty(policyId)) return DaoResultMessage.OkWithParaNull;
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Domain.FirstOrDefault(obj => obj.Id == domainId);
                    var entity = context.SecurePolicy.FirstOrDefault(obj => obj.Id == policyId);
                    if (dbentity != null && entity != null)
                    {
                        dbentity.SecurePolicy = entity;
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

        public async Task<int> GetDomainCount(string accountId, DomainEntity delta)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT Count(0) FROM sys_domain ";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        sql = $@"SELECT Count(0) FROM sys_domain where AccountId = {"{0}"} ";
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
    }
}
