using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using DjLive.CPDao.Context;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Interface;
using DjLive.CPDao.Util;
using DjUtil.Tools;

namespace DjLive.CPDao.Impl
{
    public class ServerDaoImpl:IServerDaoInterface
    {
       public async Task<DaoResultMessage> CreateServerEntity(string id, ServerEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    entity.Id = id;
                    var item = context.Server.Add(entity);
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

       public async Task<DaoResultMessage> DeleteServerEntity(string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var entity = context.Server.FirstOrDefault(obj => obj.Id == id);
                    if (entity != null)
                    {
                        if (entity.Domains.Count >0)
                        {
                            message.Code = DaoResultCode.AlreadyInUse;
                            message.Message = "正在使用中..!";
                        }
                        else
                        {
                            context.Server.Remove(entity);
                            await context.SaveChangesAsync();
                            message.Code = DaoResultCode.Success;
                            message.Message = "删除成功!";
                            message.para = entity;
                        }
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

       public async Task<DaoResultMessage> UpdateServerEntity(string id, ServerEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Server.FirstOrDefault(obj => obj.Id == id);

                    if (dbentity != null && entity != null)
                    {
                        dbentity.Ip = string.IsNullOrEmpty(entity.Ip) ? dbentity.Ip : entity.Ip;
                        dbentity.Url = string.IsNullOrEmpty(entity.Url) ? dbentity.Url : entity.Url;
                        dbentity.UserName = string.IsNullOrEmpty(entity.UserName) ? dbentity.UserName : entity.UserName;
                        dbentity.Password = string.IsNullOrEmpty(entity.Password) ? dbentity.Password : entity.Password;
                        dbentity.Option = string.IsNullOrEmpty(entity.Option) ? dbentity.Option : entity.Option;

                        context.Server.Attach(dbentity);
                        context.Entry(dbentity).Property(x => x.Id).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Name).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Url).IsModified = !entity.Url.IsStringNull();
                        context.Entry(dbentity).Property(x => x.Ip).IsModified = !entity.Ip.IsStringNull();
                        context.Entry(dbentity).Property(x => x.UserName).IsModified = !entity.UserName.IsStringNull();
                        context.Entry(dbentity).Property(x => x.Password).IsModified = !entity.Password.IsStringNull();
                        context.Entry(dbentity).Property(x => x.Option).IsModified = !entity.Option.IsStringNull();
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

        public async Task<List<ServerEntity>> GetServerEntitys(string userId = "")
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT * FROM sys_servernode";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        sql = $@"SELECT * FROM sys_servernode as b 
                                join sys_alli_account_servernode as c 
                                on c.AccountId = {"{0}"} 
                                and b.Id = c.NodeId";
                        paraObjects = new object[] { userId };
                    }
                    var objs = context.Database.SqlQuery<ServerEntity>(sql, paraObjects);
                    return await objs.ToListAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
        public async Task<int> GetServerNodesCount(string userId, ServerEntity entityDelta)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT Count(0) FROM sys_servernode";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        sql = $@"SELECT Count(0) FROM sys_servernode as b 
                                join sys_alli_account_servernode as c 
                                on c.AccountId = {"{0}"} 
                                and b.Id = c.NodeId";
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

        public async Task<List<DomainEntity>> GetServerDomainsById(string id)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var serverEntity = await context.Server.Where(item => item.Id == id).FirstOrDefaultAsync();
                    if (serverEntity != null)
                    {
                        return serverEntity.Domains.Select(item =>
                        {
                            var secure = item.SecurePolicy;
                            var record = item.RecordTemplate;
                            var transcode = item.TranscodeList;
                            return item;
                        }).ToList();
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<ServerEntity> GetServerEntityById(string accountId,string id)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {

                    var sql = $@"SELECT * FROM sys_servernode as b 
                                join sys_alli_account_servernode as c 
                                on c.AccountId = {"{0}"} and c.NodeId={"{1}"}
                                and b.Id = c.NodeId ";
                    var paraObjects = new object[] { accountId, id };
                    var objs = context.Database.SqlQuery<ServerEntity>(sql, paraObjects);
                    return await objs.FirstOrDefaultAsync();
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
                    var server = context.Server.FirstOrDefault(obj => obj.Id == id);
                    if (dbentity != null && server != null)
                    {
                        server.AccountEntities.Add(dbentity);
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

       public async Task<DaoResultMessage> Add2Account(string accountId, ServerEntity entity)
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
                        context.Server.Add(entity);
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

       public async Task<DaoResultMessage> AppendDomain(string serverId, string domainId)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Server.FirstOrDefault(obj => obj.Id == serverId);
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