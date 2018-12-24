using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Impl;
using DjLive.CPDao.Interface;
using DjLive.CPService.Interface;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;
using DjUtil.Tools;
using Newtonsoft.Json;

namespace DjLive.CPService.Impl
{
    public class SecurePolicyServiceImpl:ISecurePolicyServiceInterface
    {
        public ISecurePolicyDaoInterface SecureDao { get; set; } = new SecurePolicyDaoImpl();


        public async Task<int> GetSecurePolicysCount(string userId, SecurePolicyModel delta)
        {
            try
            {
                SecurePolicyEntity entityDelta = null;

                if (delta != null)
                {
                    entityDelta = new SecurePolicyEntity() { Name = delta.Name };
                }
                return await SecureDao.GetSecurePolicysCount(userId, entityDelta);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<SecurePolicyModel>> GetSimpleSecurePolicys(string userId, int page, int countPerPage, SecurePolicyModel delta)
        {
            try
            {
                SecurePolicyEntity entityDelta = null;
                if (delta != null)
                {
                    entityDelta = new SecurePolicyEntity() { Name = delta.Name };
                }
                var entities = await SecureDao.GetSecurePolicyEntitys(page, countPerPage, userId, entityDelta);
                return entities?.Select(item => new SecurePolicyModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<SecurePolicyModel> FindByIdAsync(string userId, string id)
        {
            try
            {
                var entity = await SecureDao.GetSecurePolicyEntityById(userId, id);
                if (entity != null)
                {
                    return new SecurePolicyModel()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        AuthPlayUrl = entity.AuthPlayUrl,
                        AuthPublishUrl = entity.AuthPublishUrl,
                        AuthCloseUrl = entity.AuthCloseUrl,
                        AuthUnPublishUrl = entity.AuthUnPublishUrl,
                        AuthStopUrl = entity.AuthStopUrl,
                        AuthConnectUrl = entity.AuthConnectUrl,
                        AuthDvrUrl = entity.AuthDvrUrl,
                        NotifyHlsUrl = entity.NotifyHlsUrl
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
        //todo:UnFinish
        public async Task<ServiceResultMessage> UpdateSecurePolicy(string userId, string id, SecurePolicyModel model)
        {
            try
            {
                //todo:1.保存设置 2.上传水印图片 3.重新生成配置文件 4.上传配置文件 5.重启SRS服务 
                var daoresult = await SecureDao.UpdateSecurePolicyEntity(id, new SecurePolicyEntity()
                {
                    Name = model?.Name,
                    AuthPlayUrl = model?.AuthPlayUrl,
                    AuthPublishUrl = model?.AuthPublishUrl,
                    AuthCloseUrl = model.AuthCloseUrl,
                    AuthUnPublishUrl = model.AuthUnPublishUrl,
                    AuthStopUrl = model.AuthStopUrl,
                    AuthConnectUrl = model.AuthConnectUrl,
                    AuthDvrUrl = model.AuthDvrUrl,
                    NotifyHlsUrl = model.NotifyHlsUrl

                });
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<bool> SecurePolicyModelExists(string userId, string id)
        {
            try
            {
                return await GetSecurePolicysCount(userId, new SecurePolicyModel() {Id = id}) >= 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ServiceResultMessage> AddSecurePolicy(string userId, string id, SecurePolicyModel model)
        {
            try
            {
                var daoresult = await SecureDao.Add2Account(userId, new SecurePolicyEntity()
                {
                    Id = id,
                    Name = model.Name,
                    AuthPlayUrl = model.AuthPlayUrl,
                    AuthPublishUrl = model.AuthPublishUrl,
                    AuthCloseUrl = model.AuthCloseUrl,
                    AuthUnPublishUrl = model.AuthUnPublishUrl,
                    AuthStopUrl = model.AuthStopUrl,
                    AuthConnectUrl = model.AuthConnectUrl,
                    AuthDvrUrl = model.AuthDvrUrl,
                    NotifyHlsUrl = model.NotifyHlsUrl
                });
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }

        public async Task<ServiceResultMessage> RemoveAsync(string userId, string id)
        {
            try
            {
                //todo:1.保存设置 2.删除水印图片 3.重新生成配置文件 4.上传配置文件 5.重启SRS服务 
                var daoresult = await SecureDao.DeleteSecurePolicyEntity(id);
                return (ServiceResultMessage)ServiceResultBase.DaoResult2ServiceResult(daoresult);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
    }
}