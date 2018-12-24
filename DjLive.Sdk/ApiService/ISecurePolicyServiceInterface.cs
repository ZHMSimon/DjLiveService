using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.Sdk.ApiClient;
using DjLive.Sdk.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;

namespace DjLive.Sdk.ApiService
{
    public interface ISecurePolicyServiceInterface
    {
        [Api(HttpMethod = HttpMethod.Get, Url = "api/Secure/SecurePolicys")]
        DjLiveResponse<List<SecurePolicyModel>> GetSecurePolicys(int page, int countPerPage);

        [Api(HttpMethod = HttpMethod.Get, Url = "api/Secure/SecurePolicy")]
        DjLiveResponse<SecurePolicyModel> GetSecurePolicyById(string id);

        [Api(HttpMethod = HttpMethod.Post, Url = "api/Secure/SecurePolicy")]
        DjLiveResponse<SecurePolicyModel> CreateSecurePolicyModel(SecurePolicyModel template);

        [Api(HttpMethod = HttpMethod.Delete, Url = "api/Secure/SecurePolicy")]
        DjLiveResponse<SecurePolicyModel> DeleteSecurePolicyModel(string id);

        [Api(HttpMethod = HttpMethod.Put, Url = "api/Secure/SecurePolicy")]
        DjLiveResponse<SecurePolicyModel> UpdateSecurePolicyModel(string id, SecurePolicyModel template);
    }
}