using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Util;

namespace DjLive.CPDao.Interface
{
    public interface IAccountDaoInterface
    {
        Task<DaoResultMessage> CreateAccount(string id, AccountEntity entity);
        Task<DaoResultMessage> RemoveAccountById(string id);
        Task<DaoResultMessage> UpdataAccount(string id, AccountEntity entity);
        Task<List<AccountEntity>> GetAccountEntities(int page, int countPerPage, int statType = 0, int roleTyoe = 0, string search = "");
        Task<AccountEntity> GetAccountEntityById(string id);
        Task<AccountEntity> GetAccountEntityByUserName(string userName);
        Task<AccountEntity> GetAccountEntityByUserToken(string token);
        Task<AccountEntity> GetAccountEntityDetailById(string id);
        Task<DaoResultMessage> AppendSecurePolicy(string accountId, string policyId);
        Task<DaoResultMessage> AddSecurePolicy(string accountId, SecurePolicyEntity entity);
        Task<DaoResultMessage> AppendRecordTemplate(string accountId, string templateId);
        Task<DaoResultMessage> AppendRecordTemplate(string accountId, RecordTemplateEntity entity);
        Task<DaoResultMessage> AppendDomain(string accountId, string domainId);
        Task<DaoResultMessage> AddDomain(string accountId, DomainEntity entity);
        Task<DaoResultMessage> AppendTranscodeTemplate(string accountId, string templateId);
        Task<DaoResultMessage> AddTranscodeTemplate(string accountId, TranscodeTemplateEntity entity);
        Task<DaoResultMessage> AppendLogoTemplate(string accountId, string templateId);
        Task<DaoResultMessage> AddLogoTemplate(string accountId, LogoTemplateEntity entity);
        Task<DaoResultMessage> AppendServer(string accountId, string serverId);
        Task<DaoResultMessage> AddServer(string accountId, ServerEntity entity);
        Task<List<SecurePolicyEntity>> GetSecurePolicyEntitys(string accountId, int page, int countPerPage, SecurePolicyEntity delta = null);

    }
}