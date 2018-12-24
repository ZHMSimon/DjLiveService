using System;
using System.Collections.Generic;
using DjLive.Sdk.ApiClient;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;

namespace DjLive.Sdk.ApiService
{
    public interface ILogoTemplateServiceInterface
    {
        [Api(HttpMethod = HttpMethod.Get, Url = "api/Logo/LogoTemplates")]
        DjLiveResponse<List<LogoTemplateModel>> GetLogoTemplates(int page, int countPerPage);

        [Api(HttpMethod = HttpMethod.Get, Url = "api/Logo/LogoTemplate")]
        DjLiveResponse<LogoTemplateModel> GetLogoTemplateById(string id);

        [Api(HttpMethod = HttpMethod.Post, Url = "api/Logo/LogoTemplate")]
        DjLiveResponse<LogoTemplateModel> CreateLogoTemplateModel(LogoTemplateModel template);

        [Api(HttpMethod = HttpMethod.Delete, Url = "api/Logo/LogoTemplate")]
        DjLiveResponse<LogoTemplateModel> DeleteLogoTemplateModel(string id);

        [Api(HttpMethod = HttpMethod.Put, Url = "api/Logo/LogoTemplate")]
        DjLiveResponse<LogoTemplateModel> UpdateLogoTemplateModel(string id,LogoTemplateModel template);
    }
}