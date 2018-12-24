using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.Sdk.ApiClient;
using DjLive.Sdk.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;

namespace DjLive.Sdk.ApiService
{
    public interface ITranscodeTemplateServiceInterface
    {

        [Api(HttpMethod = HttpMethod.Get, Url = "api/Transcode/TranscodeTemplates")]
        DjLiveResponse<List<TranscodeTemplateModel>> GetTranscodeTemplates(int page, int countPerPage);

        [Api(HttpMethod = HttpMethod.Get, Url = "api/Transcode/TranscodeTemplate")]
        DjLiveResponse<TranscodeTemplateModel> GetTranscodeTemplateById(string id);

        [Api(HttpMethod = HttpMethod.Post, Url = "api/Transcode/TranscodeTemplate")]
        DjLiveResponse<TranscodeTemplateModel> CreateTranscodeTemplateModel(TranscodeTemplateModel template);

        [Api(HttpMethod = HttpMethod.Delete, Url = "api/Transcode/TranscodeTemplate")]
        DjLiveResponse<TranscodeTemplateModel> DeleteTranscodeTemplateModel(string id);

        [Api(HttpMethod = HttpMethod.Put, Url = "api/Transcode/TranscodeTemplate")]
        DjLiveResponse<TranscodeTemplateModel> UpdateTranscodeTemplateModel(string id, TranscodeTemplateModel template);
    }
}