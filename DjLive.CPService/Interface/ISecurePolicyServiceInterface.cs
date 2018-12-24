using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;

namespace DjLive.CPService.Interface
{
    public interface ISecurePolicyServiceInterface
    {
        Task<int> GetSecurePolicysCount(string userId, SecurePolicyModel delta);
        Task<List<SecurePolicyModel>> GetSimpleSecurePolicys(string userId, int page, int countPerPage, SecurePolicyModel delta);
        Task<SecurePolicyModel> FindByIdAsync(string userId, string id);
        Task<ServiceResultMessage> UpdateSecurePolicy(string userId, string id, SecurePolicyModel model);
        Task<bool> SecurePolicyModelExists(string userId, string id);
        Task<ServiceResultMessage> AddSecurePolicy(string userId, string id, SecurePolicyModel model);
        Task<ServiceResultMessage> RemoveAsync(string userId, string id);
    }
}