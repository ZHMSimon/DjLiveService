using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.Sdk.ApiClient;
using DjLive.Sdk.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;

namespace DjLive.Sdk.ApiService
{
    public interface IRecordTemplateServiceInterface
    {
        [Api(HttpMethod = HttpMethod.Get, Url = "api/Record/RecordTemplates")]
        DjLiveResponse<List<RecordTemplateModel>> GetSimpleRecordTemplates(int page, int countPerPage);
        [Api(HttpMethod = HttpMethod.Get, Url = "api/Record/RecordTemplate")]
        DjLiveResponse<RecordTemplateModel> FindByIdAsync(string id);
        [Api(HttpMethod = HttpMethod.Put, Url = "api/Record/RecordTemplate")]
        DjLiveResponse<RecordTemplateModel> UpdateLogeTemplate(string id, RecordTemplateModel model);
        [Api(HttpMethod = HttpMethod.Post, Url = "api/Record/RecordTemplate")]
        DjLiveResponse<RecordTemplateModel> AddLogeTemplate(RecordTemplateModel model);
        [Api(HttpMethod = HttpMethod.Delete, Url = "api/Record/RecordTemplate")]
        DjLiveResponse<RecordTemplateModel> RemoveAsync(string id);
    }
}