using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Util;

namespace DjLive.CPDao.Interface
{
    public interface IServerDaoInterface
    {
        Task<DaoResultMessage> CreateServerEntity(string id, ServerEntity entity);
        Task<DaoResultMessage> DeleteServerEntity(string id);
        Task<DaoResultMessage> UpdateServerEntity(string id, ServerEntity entity);
        Task<List<ServerEntity>> GetServerEntitys(string userId = "");
        Task<List<DomainEntity>> GetServerDomainsById(string id);
        Task<ServerEntity> GetServerEntityById(string accountId, string id);

        Task<DaoResultMessage> Append2Account(string accountId, string id);
        Task<DaoResultMessage> Add2Account(string accountId, ServerEntity entity);
        Task<DaoResultMessage> AppendDomain(string serverId, string domainId);

        Task<int> GetServerNodesCount(string userId, ServerEntity entityDelta);
    }
}