using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;

namespace DjLive.CPService.Interface
{
    public interface ITranscodeTemplateServiceInterface
    {
        Task<int> GetTranscodeTemplatesCount(string userId, TranscodeTemplateModel delta);
        Task<List<TranscodeTemplateModel>> GetSimpleTranscodeTemplates(string userId, int page, int countPerPage, TranscodeTemplateModel delta);
        Task<TranscodeTemplateModel> FindByIdAsync(string userId, string id);
        Task<ServiceResultMessage> UpdateLogeTemplate(string userId, string id, TranscodeTemplateModel model);
        Task<bool> TranscodeTemplateModelExists(string userId, string id);
        Task<ServiceResultMessage> AddLogeTemplate(string userId, string id, TranscodeTemplateModel model);
        Task<ServiceResultMessage> RemoveAsync(string userId, string id);
    }
}