using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPService.Util;
using DjLive.SdkModel.Enum;
using DjLive.SdkModel.Model;

namespace DjLive.CPService.Interface
{
    public interface IAccountServiceInterface
    {
        Task<AccountValidateMessage> CheckAccountVilidate(string userName,string password, double expireTime = 7200);
        Task<AccountValidateMessage> CheckAccountTokenVilidate(string userName, string token);
        Task<ServiceResultMessage> CreateAccountModel(string userName, string password);
        Task<ServiceResultMessage> ModifyAccountPassword(string id,string password);
        Task<ServiceResultMessage> ModifyAccountRole(string id,AccountRoleType roleType);
        Task<ServiceResultMessage> ModifyAccountStat(string id,AccountStatType roleType);
        Task<List<AccountModel>> GetAccountModels();
    }
}