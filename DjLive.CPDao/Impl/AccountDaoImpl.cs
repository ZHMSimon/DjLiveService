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
    public class AccountDaoImpl: IAccountDaoInterface
    {
        public async Task<DaoResultMessage> CreateAccount(string id, AccountEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    entity.Id = id;
                    var item = context.Account.Add(entity);
                    await context.SaveChangesAsync();
                    message.Code = DaoResultCode.Success;
                    message.Message = "添加成功!";
                    message.para = item;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message,e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }
            
            return message;
        }

        public async Task<DaoResultMessage> RemoveAccountById(string id)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var entity = context.Account.FirstOrDefault(obj => obj.Id == id);
                    if (entity != null)
                    {
                        context.Account.Remove(entity);
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
                LogHelper.Error(e.Message,e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }

        public async Task<DaoResultMessage> UpdataAccount(string id, AccountEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == id);
                    
                    if (dbentity!=null && entity != null)
                    {
                        dbentity.Password = string.IsNullOrEmpty(entity.Password) ? dbentity.Password : entity.Password;
                        dbentity.Token = string.IsNullOrEmpty(entity.Token) ? dbentity.Token : entity.Token;
                        dbentity.RoleType = entity.RoleType == 0 ? dbentity.RoleType : entity.RoleType;
                        dbentity.StatType = entity.StatType == 0 ? dbentity.StatType : entity.StatType;

                        context.Account.Attach(dbentity);
                        context.Entry(dbentity).Property(x => x.Id).IsModified = false;
                        context.Entry(dbentity).Property(x => x.UserName).IsModified = false;
                        context.Entry(dbentity).Property(x => x.Token).IsModified = true;
                        context.Entry(dbentity).Property(x => x.Password).IsModified = true;
                        context.Entry(dbentity).Property(x => x.RoleType).IsModified = true;
                        context.Entry(dbentity).Property(x => x.StatType).IsModified = true;
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
                LogHelper.Error(e.Message,e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }

        public async Task<List<AccountEntity>> GetAccountEntities(int page, int countPerPage, int statType = 0, int roleTyoe = 0, string search = "")
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var query = context.Account.Where(item => true);
                    if (statType!= 0)
                    {
                        query = query.Where(item => item.StatType == statType);
                    }
                    if (roleTyoe != 0)
                    {
                        query = query.Where(item => item.RoleType == roleTyoe);
                    }
                    if (!string.IsNullOrEmpty(search))
                    {
                        query = query.Where(item => item.UserName.Contains(search));
                    }
                    query = query.OrderBy(item=>item.Id).Take(countPerPage).Skip((page - 1) * countPerPage);
                    return await query.ToListAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message,e);
                return null;
            }

        }

        public async Task<AccountEntity> GetAccountEntityById(string id)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var query = context.Account.Where(item => item.Id == id);
                    return await query.FirstOrDefaultAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<AccountEntity> GetAccountEntityByUserName(string userName)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var query = context.Account.Where(item => item.UserName == userName);
                    return await query.FirstOrDefaultAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<AccountEntity> GetAccountEntityByUserToken(string token)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var query = context.Account.Where(item => item.Token == token);
                    return await query.FirstOrDefaultAsync();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<AccountEntity> GetAccountEntityDetailById(string id)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var query = context.Account.Where(item => item.Id == id);
                    var entity = await query.FirstOrDefaultAsync();
                    return entity;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }


        public async Task<DaoResultMessage> AppendSecurePolicy(string accountId, string policyId)
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
                        dbentity.SecurePolicyEntities.Add(securepolicy);
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

        public async Task<DaoResultMessage> AddSecurePolicy(string accountId, SecurePolicyEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var securepolicy = context.SecurePolicy.FirstOrDefault(obj => obj.Id == entity.Id);
                    if (securepolicy != null)
                    {
                        message.Code = DaoResultCode.ItemAlreadyExist;
                        message.Message = "添加失败,对象已经存在!";
                    }
                    else if (dbentity != null)
                    {
                        dbentity.SecurePolicyEntities.Add(entity);
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

        public async Task<DaoResultMessage> AppendRecordTemplate(string accountId, string templateId)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var recordTemplateEntity = context.RecordTemplate.FirstOrDefault(obj => obj.Id == templateId);
                    if (dbentity != null && recordTemplateEntity != null)
                    {
                        dbentity.RecordTemplateEntities.Add(recordTemplateEntity);
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

        public async Task<DaoResultMessage> AppendRecordTemplate(string accountId, RecordTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var securepolicy = context.RecordTemplate.FirstOrDefault(obj => obj.Id == entity.Id);
                    if (securepolicy != null)
                    {
                        message.Code = DaoResultCode.ItemAlreadyExist;
                        message.Message = "添加失败,对象已经存在!";
                    }
                    else if (dbentity != null)
                    {
                        dbentity.RecordTemplateEntities.Add(entity);
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

        public async Task<DaoResultMessage> AppendDomain(string accountId, string domainId)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
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

        public async Task<DaoResultMessage> AddDomain(string accountId, DomainEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var domain = context.Domain.FirstOrDefault(obj => obj.Id == entity.Id);
                    if (domain != null)
                    {
                        message.Code = DaoResultCode.ItemAlreadyExist;
                        message.Message = "添加失败,对象已经存在!";
                    }
                    else if (dbentity != null)
                    {
                        dbentity.Domains.Add(entity);
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

        public async Task<DaoResultMessage> AppendTranscodeTemplate(string accountId, string templateId)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var transcode = context.TranscodeTemplate.FirstOrDefault(obj => obj.Id == templateId);
                    if (dbentity != null && transcode != null)
                    {
                        dbentity.TranscodeTemplateEntities.Add(transcode);
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

        public async Task<DaoResultMessage> AddTranscodeTemplate(string accountId, TranscodeTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var transcode = context.TranscodeTemplate.FirstOrDefault(obj => obj.Id == entity.Id);
                    if (transcode != null)
                    {
                        message.Code = DaoResultCode.ItemAlreadyExist;
                        message.Message = "添加失败,对象已经存在!";
                    }
                    else if (dbentity != null)
                    {
                        dbentity.TranscodeTemplateEntities.Add(entity);
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

        public async Task<DaoResultMessage> AppendLogoTemplate(string accountId, string templateId)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var logoTemplate = context.LogoTemplate.FirstOrDefault(obj => obj.Id == templateId);
                    if (dbentity != null && logoTemplate != null)
                    {
                        dbentity.LogoTemplateEntities.Add(logoTemplate);
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

        public async Task<DaoResultMessage> AddLogoTemplate(string accountId, LogoTemplateEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var logoTemplate = context.LogoTemplate.FirstOrDefault(obj => obj.Id == entity.Id);
                    if (logoTemplate != null)
                    {
                        message.Code = DaoResultCode.ItemAlreadyExist;
                        message.Message = "添加失败,对象已经存在!";
                    }
                    else if (dbentity != null)
                    {
                        dbentity.LogoTemplateEntities.Add(entity);
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

        public async Task<DaoResultMessage> AppendServer(string accountId, string serverId)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var serverEntity = context.Server.FirstOrDefault(obj => obj.Id == serverId);
                    if (dbentity != null && serverEntity != null)
                    {
                        dbentity.ServerEntities.Add(serverEntity);
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

        public async Task<DaoResultMessage> AddServer(string accountId, ServerEntity entity)
        {
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var dbentity = context.Account.FirstOrDefault(obj => obj.Id == accountId);
                    var serverEntity = context.Server.FirstOrDefault(obj => obj.Id == entity.Id);
                    if (serverEntity != null)
                    {
                        message.Code = DaoResultCode.ItemAlreadyExist;
                        message.Message = "添加失败,对象已经存在!";
                    }
                    else if (dbentity != null)
                    {
                        dbentity.ServerEntities.Add(entity);
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

        public async Task<List<SecurePolicyEntity>> GetSecurePolicyEntitys(string accountId,int page, int countPerPage, SecurePolicyEntity delta = null)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    var account = await context.Account.FirstOrDefaultAsync(item => item.Id == accountId);
                    if (account == null)
                    {
                        LogHelper.Error("accountId 不能为空.");
                        return null;
                    }
                    var query = account.SecurePolicyEntities.OrderBy(item => item.Id).Take(countPerPage).Skip((page - 1) * countPerPage);
                    return query.ToList();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
    }
}