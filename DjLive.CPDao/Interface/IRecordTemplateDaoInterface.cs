using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Util;

namespace DjLive.CPDao.Interface
{
    public interface IRecordTemplateDaoInterface
    {
        Task<DaoResultMessage> CreateRecordTemplateEntity(string id, RecordTemplateEntity entity);
        Task<DaoResultMessage> DeleteRecordTemplateEntity(string id);
        Task<DaoResultMessage> UpdateRecordTemplateEntity(string id, RecordTemplateEntity entity);
        Task<List<RecordTemplateEntity>> GetRecordTemplateEntitys(int page, int countPerPage, string userId, RecordTemplateEntity entityDelta);
        Task<int> GetRecordTemplatesCount(string userId, RecordTemplateEntity entityDelta);

        Task<RecordTemplateEntity> GetRecordTemplateEntityById(string accountId, string id);
        Task<DaoResultMessage> Append2Account(string accountId, string id);
        Task<DaoResultMessage> Add2Account(string accountId, RecordTemplateEntity entity);
        Task<DaoResultMessage> Append2Domain(string accountId, string id);

    }
}