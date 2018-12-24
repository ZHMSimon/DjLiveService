using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Util;

namespace DjLive.CPDao.Interface
{
    public interface ITranscodeTemplateDaoInterface
    {
        Task<DaoResultMessage> CreateTranscodeTemplateEntity(string id, TranscodeTemplateEntity entity);
        Task<DaoResultMessage> DeleteTranscodeTemplateEntity(string id);
        Task<DaoResultMessage> UpdateTranscodeTemplateEntity(string id, TranscodeTemplateEntity entity);
        Task<List<TranscodeTemplateEntity>> GetTranscodeTemplateEntitys(int page, int countPerPage,string userId="", TranscodeTemplateEntity delta = null);
        Task<TranscodeTemplateEntity> GetTranscodeTemplateEntitysById(string accountId, string id);
        Task<DaoResultMessage> Append2Account(string accountId, string id);
        Task<DaoResultMessage> Add2Account(string accountId, TranscodeTemplateEntity entity);
        Task<DaoResultMessage> Append2Domain(string accountId, string id);
        Task<int> GetTranscodeTemplatesCount(string userId, TranscodeTemplateEntity entityDelta);
    }
}