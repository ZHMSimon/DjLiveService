using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Impl;
using DjLive.CPDao.Interface;
using DjLive.CPDao.Util;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel.Cookie;
using DjLive.SdkModel.Enum;
using DjLive.SdkModel.Model;
using DjUtil.Tools;

namespace DjLive.CPService.Impl
{
    public class AccountServiceImpl:IAccountServiceInterface
    {
        private IAccountDaoInterface AccountDao { get; set; } = new AccountDaoImpl();

        private AccountValidateMessage CheckAccountState(AccountEntity entity)
        {
            AccountValidateMessage result = null;
            if (entity == null)
            {
                result = new AccountValidateMessage()
                {
                    ResulType = ValidateType.NotExist,
                    Message = "登陆失败,用户名或密码错误.",
                };
            }
            if (entity != null && entity.StatType != (int) AccountStatType.Normal)
            {
                result = new AccountValidateMessage();
                switch (entity.StatType)
                {
                    case (int) AccountStatType.Disable:
                    {
                        result.ResulType = ValidateType.Disabled;
                        result.Message = "帐号已停用!";
                        break;
                    }
                    case (int) AccountStatType.FreezeReason1:
                    {
                        result.ResulType = ValidateType.Freezeed;
                        result.Message = "帐号已由于原因1冻结!";
                        break;
                    }
                    case (int) AccountStatType.FreezeReason2:
                    {
                        result.ResulType = ValidateType.Freezeed;
                        result.Message = "帐号已由于原因2冻结!";

                        break;
                    }
                    case (int) AccountStatType.Verify:
                    {
                        result.ResulType = ValidateType.Verify;
                        result.Message = "帐号需要输入验证!";
                        break;
                    }
                    default:
                    {
                        result.ResulType = ValidateType.UnExcept;
                        result.Message = "未知错误!";

                        break;
                    }

                }
            }
            return result;
        }

        public async Task<AccountValidateMessage> CheckAccountVilidate(string userName,string password,double expireTime = 7200)
        {
            var entity = await AccountDao.GetAccountEntityByUserName(userName);
            AccountValidateMessage result = CheckAccountState(entity);
            if (result == null)
            {
                result = new AccountValidateMessage();

                if (entity.Password == password)
                {
                    try
                    {
                        var expired = DateTime.Now.AddSeconds(expireTime);
                        var token = Guid.NewGuid().Str() + $@"|{expired.Unix10TimeStamp()}";
                        await AccountDao.UpdataAccount(entity.Id, new AccountEntity() {Token = token});
                        result.ResulType = ValidateType.None;
                        result.Cookie = new AuthCookieModel()
                        {
                            Id = entity.Id,
                            UserName = entity.UserName,
                            Token = token,
                            Expired = expired,
                        };
                        result.Message = "登陆成功!";
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e.Message, e);
                        result.ResulType = ValidateType.UnExcept;
                        result.Message = "登陆失败,未知错误.";
                    }
                }
                else
                {
                    result.ResulType = ValidateType.PasswordError;
                    result.Message = "登陆失败,用户名或密码错误.";
                }
            }
            return result;
        }

         public async Task<AccountValidateMessage> CheckAccountTokenVilidate(string userName,string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new AccountValidateMessage()
                {
                    ResulType = ValidateType.AuthCookieEmpty,
                    Message = "Token 验证失败.",
                };
            }
            var entity = await AccountDao.GetAccountEntityByUserName(userName);
            AccountValidateMessage result = CheckAccountState(entity);
            if (result == null)
            {
                result = new AccountValidateMessage();
                if (entity.Token == token)
                {
                    try
                    {
                        var expiredStr = token.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                        var expire = long.Parse(expiredStr);
                        if (DateTime.Now.Unix10TimeStamp()> expire)
                        {
                            result.ResulType = ValidateType.AuthCookieExpired;
                            result.Message = "Token已经过期,请重新登陆!";
                        }
                        else
                        {
                            result.ResulType = ValidateType.None;
                            result.Message = "登陆成功!";
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e.Message, e);
                        result.ResulType = ValidateType.UnExcept;
                        result.Message = "登陆失败,未知错误.";
                    }
                }
                else
                {
                    result.ResulType = ValidateType.AuthCookieError;
                    result.Message = "Token 验证失败.";
                }
            }
            return result;
        }

        public async Task<ServiceResultMessage> CreateAccountModel(string userName, string password)
        {
            ServiceResultMessage result = new ServiceResultMessage();
            try
            {
                var id = Guid.NewGuid().Str();
                var dbitem = await AccountDao.GetAccountEntityByUserName(userName);
                if (dbitem != null)
                {
                    result.code = ServiceResultCode.ItemAlreadyExist;
                    result.Message = "帐号已经存在..";
                    result.Para = new AccountModel()
                    {
                        UserName = userName,
                        Id = id,
                        RoleType = "普通用户",
                        StatType = "正常",
                    };
                }
                else
                {
                    var daoresult = await AccountDao.CreateAccount(id, new AccountEntity()
                    {
                        UserName = userName,
                        Password = password,
                        Id = id,
                        RoleType = (int)AccountRoleType.User,
                        StatType = (int)AccountStatType.Normal,
                        Token = "",
                    });

                    if (daoresult.Code == DaoResultCode.Success)
                    {
                        result.code = ServiceResultCode.Success;
                        result.Message = "帐号创建成功.";
                        result.Para = new AccountModel()
                        {
                            UserName = userName,
                            Id = id,
                            RoleType = "普通用户",
                            StatType = "正常",
                        };
                    }
                    else
                    {
                        result.code = ServiceResultCode.UnExceptError;
                        result.Message = "创建失败,未知错误.";
                    }
                }
                
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message,e);
                result.code = ServiceResultCode.UnExceptError;
                result.Message = "创建失败,未知错误.";
            }
            return result;
        }

        public async Task<ServiceResultMessage> ModifyAccountPassword(string id, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResultMessage> ModifyAccountRole(string id, AccountRoleType roleType)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResultMessage> ModifyAccountStat(string id, AccountStatType roleType)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<AccountModel>> GetAccountModels()
        {
            throw new System.NotImplementedException();
        }
    }
}