using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.Sdk.ApiClient;
using DjLive.Sdk.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;

namespace DjLive.Sdk.ApiService
{
    public interface IVhostServiceInterface
    {
        [Api(HttpMethod = HttpMethod.Get, Url = "api/Domain/Domains")]
        DjLiveResponse<List<DomainModel>> GetDomains(int page, int countPerPage);

        [Api(HttpMethod = HttpMethod.Get, Url = "api/Domain/Domain")]
        DjLiveResponse<DomainModel> GetDomainById(string id);

        [Api(HttpMethod = HttpMethod.Post, Url = "api/Domain/Domain")]
        DjLiveResponse<DomainModel> CreateDomainModel(DomainModel template);

        [Api(HttpMethod = HttpMethod.Delete, Url = "api/Domain/Domain")]
        DjLiveResponse<DomainModel> DeleteDomainModel(string id);

        [Api(HttpMethod = HttpMethod.Put, Url = "api/Domain/Domain")]
        DjLiveResponse<DomainModel> UpdateDomainModel(string id, DomainModel template);
    }
}