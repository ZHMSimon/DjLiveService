using System;
using System.Collections.Generic;
using DjLive.Sdk;
using DjLive.Sdk.Model;
using DjLive.SdkModel;
using DjLive.SdkModel.Enum;
using DjLive.SdkModel.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SdkUnitTest
{
    [TestClass]
    public class DomainTest
    {
        private const string DefaultServerId = "fdd5659abd28416d91b7711581cfa6a0";
        private DomainModel DefaultModel = new DomainModel()
        {
            SourceDomain = "172.28.10.135",
            Description = "test Domain",
            SourceType = SourceStreamType.Rtmp_Push,
            StateType = StateType.Open,
            ServerPair = new KeyNamePair("fdd5659abd28416d91b7711581cfa6a0","TestName"),
        };
        [TestMethod]
        public void GetDomainTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().VhostService.CreateDomainModel(DefaultModel);
            Assert.AreEqual(ApiCode.Success, add.ApiCode);
            var defaults = ApiManager.GetInstance().VhostService.GetDomainById(add?.Content?.Id);
            Assert.AreEqual(ApiCode.Success, defaults.ApiCode);
            var delete = ApiManager.GetInstance().VhostService.DeleteDomainModel(add.Content?.Id);
        }
        [TestMethod]
        public void AddDomainTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().VhostService.CreateDomainModel(DefaultModel);
            Assert.AreEqual(ApiCode.Success,add.ApiCode);
            Assert.IsNotNull(add.Content);
            Assert.AreEqual(DefaultModel?.Description, add.Content?.Description);
            var delete = ApiManager.GetInstance().VhostService.DeleteDomainModel(add.Content?.Id);
        }

        [TestMethod]
        public void DeleteDomainTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().VhostService.CreateDomainModel(DefaultModel);
            var delete = ApiManager.GetInstance().VhostService.DeleteDomainModel(add.Content?.Id);
            Assert.AreEqual(ApiCode.Success,delete.ApiCode);
            Assert.AreEqual(DefaultModel.Description, delete.Content?.Description);
        }

        [TestMethod]
        public void UpdateDomainTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().VhostService.CreateDomainModel(DefaultModel);
            add.Content.Description = "1";
            var update = ApiManager.GetInstance().VhostService.UpdateDomainModel(add.Content?.Id, add.Content);
            Assert.AreEqual(ApiCode.Success, update.ApiCode);
            Assert.AreEqual(add.Content.Description, update.Content?.Description);
            var delete = ApiManager.GetInstance().VhostService.DeleteDomainModel(add.Content?.Id);
        }

    }
}
