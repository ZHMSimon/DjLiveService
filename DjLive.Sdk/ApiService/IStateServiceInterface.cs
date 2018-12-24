using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.Sdk.ApiClient;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;

namespace DjLive.Sdk.ApiService
{
    public interface IStateServiceInterface
    {
        [Api(HttpMethod = HttpMethod.Get, Url = "api/State/ReloadConf")]
        DjLiveResponse<dynamic> ReloadSrsConf(string id);
        [Api(HttpMethod = HttpMethod.Get, Url = "api/State/Vhosts")]
        DjLiveResponse<List<LiveVHostModel>> GetVhostsState();
        [Api(HttpMethod = HttpMethod.Get, Url = "api/State/Streams")]
        DjLiveResponse<List<LiveStreamModel>> GetStreamsState();
        [Api(HttpMethod = HttpMethod.Get, Url = "api/State/Clients")]
        DjLiveResponse<List<LiveClientModel>> GetClientsState();
        [Api(HttpMethod = HttpMethod.Post, Url = "api/State/Domain")]
        DjLiveResponse<DomainModel> CreateDefaultDomain(DomainModel domainModel);
    }
}