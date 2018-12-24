using System;
using DjLive.Sdk;
using DjLive.Sdk.Model;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SdkUnitTest
{
    [TestClass]
    public class SecurePolicyTest
    {
        private SecurePolicyModel DefaultModel = new SecurePolicyModel()
        {
            Name = "TestRecordTemplate",
            AuthConnectUrl = "http://www.520zhonghua.com/LiveRoom/ConnectVerify",
            AuthPublishUrl = "http://www.520zhonghua.com/LiveRoom/PushStreamVerify",
            AuthUnPublishUrl = "http://www.520zhonghua.com/LiveRoom/PushDone",
            AuthPlayUrl = "http://www.520zhonghua.com/LiveRoom/AudienceVerify",
            AuthStopUrl = "http://www.520zhonghua.com/LiveRoom/AudienceDone",
        };
        [TestMethod]
        public void GetSecurePolicyTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().SecurePolicyService.CreateSecurePolicyModel(DefaultModel);
            Assert.AreEqual(ApiCode.Success, add.ApiCode);
            var defaults = ApiManager.GetInstance().SecurePolicyService.GetSecurePolicyById(add?.Content?.Id);
            Assert.AreEqual(ApiCode.Success, defaults.ApiCode);
        }
        [TestMethod]
        public void AddSecurePolicyTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().SecurePolicyService.CreateSecurePolicyModel(DefaultModel);
            Assert.AreEqual(ApiCode.Success,add.ApiCode);
            Assert.IsNotNull(add.Content);
            Assert.AreEqual(DefaultModel?.Name, add.Content?.Name);
            ApiManager.GetInstance().SecurePolicyService.DeleteSecurePolicyModel(add.Content?.Id);
        }

        [TestMethod]
        public void DeleteSecurePolicyTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().SecurePolicyService.CreateSecurePolicyModel(DefaultModel);
            var delete = ApiManager.GetInstance().SecurePolicyService.DeleteSecurePolicyModel(add.Content?.Id);
            Assert.AreEqual(ApiCode.Success,delete.ApiCode);
            Assert.AreEqual(DefaultModel.Name, delete.Content?.Name);
        }

        [TestMethod]
        public void UpdateSecurePolicyTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().SecurePolicyService.CreateSecurePolicyModel(DefaultModel);
            add.Content.AuthCloseUrl = "1";
            var update = ApiManager.GetInstance().SecurePolicyService.UpdateSecurePolicyModel(add.Content?.Id, add.Content);
            Assert.AreEqual( ApiCode.Success,update.ApiCode);
            Assert.AreEqual(add.Content.AuthCloseUrl, update.Content?.AuthCloseUrl);

            var delete = ApiManager.GetInstance().SecurePolicyService.DeleteSecurePolicyModel(add.Content?.Id);
        }

    }
}
