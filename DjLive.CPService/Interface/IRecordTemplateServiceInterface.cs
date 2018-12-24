using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;

namespace DjLive.CPService.Interface
{
    public interface IRecordTemplateServiceInterface
    {
        Task<int> GetRecordTemplatesCount(string userId, RecordTemplateModel delta);
        Task<List<RecordTemplateModel>> GetSimpleRecordTemplates(string userId, int page, int countPerPage, RecordTemplateModel delta);
        Task<RecordTemplateModel> FindByIdAsync(string userId, string id);
        Task<ServiceResultMessage> UpdateLogeTemplate(string userId, string id, RecordTemplateModel model);
        Task<bool> RecordTemplateModelExists(string userId, string id);
        Task<ServiceResultMessage> AddLogeTemplate(string userId, string id, RecordTemplateModel model);
        Task<ServiceResultMessage> RemoveAsync(string userId, string id);
    }
}