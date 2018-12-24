using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Util;

namespace DjLive.CPDao.Interface
{
    public interface ILogoTemplateDaoInterface
    {
        Task<DaoResultMessage> CreateLogoTemplate(string id,LogoTemplateEntity entity);
        Task<DaoResultMessage> DeleteLogoTemplate(string id);
        Task<DaoResultMessage> UpdateLogoTemplate(string id, LogoTemplateEntity entity);
        Task<List<LogoTemplateEntity>> GetLogoTemplates(int page,int countPerPage,string accountId="", LogoTemplateEntity delta = null);
        Task<int> GetLogoTemplatesCount(string accountId="", LogoTemplateEntity delta = null);

        Task<LogoTemplateEntity> GetLogoTemplateById(string accountId, string id);

        Task<DaoResultMessage> Append2Account(string accountId,string id);
        Task<DaoResultMessage> Add2Account(string accountId, LogoTemplateEntity entity);

        Task<DaoResultMessage> Append2Domain(string accountId, string id);
    }
}