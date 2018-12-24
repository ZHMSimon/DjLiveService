using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Util;

namespace DjLive.CPDao.Interface
{
    public interface IDomainDaoInterface
    {
        Task<DaoResultMessage> CreateDomain(string id, DomainEntity entity);
        Task<DaoResultMessage> DeleteDomainById(string id);
        Task<DaoResultMessage> UpdataDomain(string id, DomainEntity entity);
        Task<List<DomainEntity>> GetDomainEntities(int page, int countPerPage,string accountId ="", DomainEntity entity = null);
        Task<DomainEntity> GetDomainEntityById(string id);

        Task<DaoResultMessage> Append2Account(string accountId, string serverId, string id);
        Task<DaoResultMessage> Add2Account(string accountId,string serverId, DomainEntity entity);

        Task<DaoResultMessage> Append2Server(string serverId, string id);

        Task<DaoResultMessage> AppendTranscode(string domainId, List<string> templateId);
        Task<DaoResultMessage> AppendSecure(string domainId, string policyId);
        Task<int> GetDomainCount(string userId, DomainEntity delta);
    }
}