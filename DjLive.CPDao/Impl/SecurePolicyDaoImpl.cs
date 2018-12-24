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
    public class SecurePolicyDaoImpl:ISecurePolicyDaoInterface
    {
        public async Task<DaoResultMessage> CreateSecurePolicyEntity(string id, SecurePolicyEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    entity.Id = id;
                    var item = context.SecurePolicy.Add(entity);
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

        public async Task<DaoResultMessage> DeleteSecurePolicyEntity(string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var securepolicy = context.SecurePolicy.FirstOrDefault(obj => obj.Id == id);
                    if (securepolicy != null )
                    {
                        context.SecurePolicy.Remove(securepolicy);
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

        public async Task<DaoResultMessage> UpdateSecurePolicyEntity(string id, SecurePolicyEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.SecurePolicy.FirstOrDefault(obj => obj.Id == id);

                    if (dbentity != null && entity != null)
                    {
                        dbentity.AuthPlayUrl =  entity.AuthPlayUrl;
                        dbentity.AuthPublishUrl = entity.AuthPublishUrl;
                        dbentity.AuthCloseUrl =  entity.AuthCloseUrl;
                        dbentity.AuthConnectUrl =  entity.AuthConnectUrl;
                        dbentity.AuthDvrUrl =  entity.AuthDvrUrl;
                        dbentity.AuthStopUrl =  entity.AuthStopUrl;
                        dbentity.AuthUnPublishUrl =  entity.AuthUnPublishUrl;
                        dbentity.NotifyHlsUrl =  entity.NotifyHlsUrl;

                        context.SecurePolicy.Attach(dbentity);
                        context.Entry(dbentity).Property(x => x.Id).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Name).IsModified = false;
                        context.Entry(dbentity).Property(x => x.AuthPlayUrl).IsModified = !entity.AuthPlayUrl.IsStringNull();
                        context.Entry(dbentity).Property(x => x.AuthPublishUrl).IsModified = !entity.AuthPublishUrl.IsStringNull();
                        context.Entry(dbentity).Property(x => x.AuthCloseUrl).IsModified = !entity.AuthCloseUrl.IsStringNull();
                        context.Entry(dbentity).Property(x => x.AuthConnectUrl).IsModified = !entity.AuthConnectUrl.IsStringNull();
                        context.Entry(dbentity).Property(x => x.AuthDvrUrl).IsModified = !entity.AuthDvrUrl.IsStringNull();
                        context.Entry(dbentity).Property(x => x.AuthStopUrl).IsModified = !entity.AuthStopUrl.IsStringNull();
                        context.Entry(dbentity).Property(x => x.AuthUnPublishUrl).IsModified = !entity.AuthUnPublishUrl.IsStringNull();
                        context.Entry(dbentity).Property(x => x.NotifyHlsUrl).IsModified = !entity.NotifyHlsUrl.IsStringNull();

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

        public async Task<List<SecurePolicyEntity>> GetSecurePolicyEntitys(int page, int countPerPage, string userId = "", SecurePolicyEntity delta = null)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT * FROM sys_securepolicy  
                                 limit {(page - 1) * countPerPage},{countPerPage}";
                    var paraObjects = new object[] {  };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        sql = $@"SELECT * FROM sys_securepolicy as b 
                                join sys_alli_account_securepolicy as c 
                                on c.AccountId = {"{0}"} 
                                and b.Id = c.PolicyId limit {(page - 1) * countPerPage},{countPerPage}";
                        paraObjects  = new object[] { userId };
                    }
                    var objs = context.Database.SqlQuery<SecurePolicyEntity>(sql, paraObjects);
                    return await objs.ToListAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }

        }

        public async Task<int> GetSecurePolicysCount(string userId, SecurePolicyEntity entityDelta)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var sql = $@"SELECT Count(0) FROM sys_securepolicy";
                    var paraObjects = new object[] { };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        sql = $@"SELECT Count(0) FROM sys_securepolicy as b 
                                join sys_alli_account_securepolicy as c 
                                on c.AccountId = {"{0}"}
                                and b.Id = c.PolicyId";
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

        public async Task<SecurePolicyEntity> GetSecurePolicyEntityById(string accountId, string id)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    return  await context.SecurePolicy.FirstOrDefaultAsync(item => item.Id == id);;
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
                    var securepolicy = context.SecurePolicy.FirstOrDefault(obj => obj.Id == policyId);
                    if (dbentity != null && securepolicy != null)
                    {
                        securepolicy.AccountEntities.Add(dbentity);
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

        public async Task<DaoResultMessage> Add2Account(string accountId, SecurePolicyEntity entity)
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
                        context.SecurePolicy.Add(entity);
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