using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Util;

namespace DjLive.CPDao.Interface
{
    public interface ISecurePolicyDaoInterface
    {
        Task<DaoResultMessage> CreateSecurePolicyEntity(string id, SecurePolicyEntity entity);
        Task<DaoResultMessage> DeleteSecurePolicyEntity(string id);
        Task<DaoResultMessage> UpdateSecurePolicyEntity(string id, SecurePolicyEntity entity);
        Task<List<SecurePolicyEntity>> GetSecurePolicyEntitys(int page, int countPerPage, string userId, SecurePolicyEntity entityDelta);
        Task<int> GetSecurePolicysCount(string userId, SecurePolicyEntity entityDelta);

        Task<SecurePolicyEntity> GetSecurePolicyEntityById(string accountId, string id);
        Task<DaoResultMessage> Append2Account(string accountId, string id);
        Task<DaoResultMessage> Add2Account(string accountId, SecurePolicyEntity entity);
        Task<DaoResultMessage> Append2Domain(string domainId, string id);

    }
}