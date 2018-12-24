using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;

namespace DjLive.CPService.Interface
{
    public interface ILogoServiceInterface
    {
        Task<int> GetLogoTemplatesCount(string userId, LogoTemplateModel delta);
        Task<List<LogoTemplateModel>> GetSimpleLogoTemplates(string userId, int page, int countPerPage, LogoTemplateModel delta);
        Task<LogoTemplateModel> FindByIdAsync(string userId, string id);
        Task<ServiceResultMessage> UpdateLogeTemplate(string userId, string id, LogoTemplateModel model);
        Task<bool> LogoTemplateModelExists(string userId, string id);
        Task<ServiceResultMessage> AddLogeTemplate(string userId, string id, LogoTemplateModel model);
        Task<ServiceResultMessage> RemoveAsync(string userId, string id);
    }
}