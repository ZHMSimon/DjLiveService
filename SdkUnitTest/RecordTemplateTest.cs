using System;
using DjLive.Sdk;
using DjLive.Sdk.Model;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SdkUnitTest
{
    [TestClass]
    public class RecordTemplateTest
    { 
        private RecordTemplateModel DefaultModel = new RecordTemplateModel()
        {
            Name = "TestRecordTemplate",
            Description = "TestRecordTemplate",
            UploadUrl = "TestUrl",
        };
        [TestMethod]
        public void GetRecordTemplateTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().RecordTemplateService.AddLogeTemplate(DefaultModel);
            Assert.AreEqual(ApiCode.Success, add.ApiCode);
            var defaults = ApiManager.GetInstance().RecordTemplateService.FindByIdAsync(add?.Content?.Id);
            Assert.AreEqual(ApiCode.Success, defaults.ApiCode);
        }
        [TestMethod]
        public void AddRecordTemplateTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().RecordTemplateService.AddLogeTemplate(DefaultModel);
            Assert.AreEqual(ApiCode.Success,add.ApiCode);
            Assert.IsNotNull(add.Content);
            Assert.AreEqual(DefaultModel?.Name, add.Content?.Name);
            ApiManager.GetInstance().RecordTemplateService.RemoveAsync(add.Content?.Id);
        }

        [TestMethod]
        public void DeleteRecordTemplateTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().RecordTemplateService.AddLogeTemplate(DefaultModel);
            var delete = ApiManager.GetInstance().RecordTemplateService.RemoveAsync(add.Content?.Id);
            Assert.AreEqual(ApiCode.Success,delete.ApiCode);
            Assert.AreEqual(DefaultModel.Name, delete.Content?.Name);
        }

        [TestMethod]
        public void UpdateRecordTemplateTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().RecordTemplateService.AddLogeTemplate(DefaultModel);
            add.Content.Description = "1";
            var update = ApiManager.GetInstance().RecordTemplateService.UpdateLogeTemplate(add.Content?.Id, add.Content);
            Assert.AreEqual( ApiCode.Success,update.ApiCode);
            Assert.AreEqual(add.Content.Description,update.Content?.Description);

            var delete = ApiManager.GetInstance().RecordTemplateService.RemoveAsync(add.Content?.Id);
        }

    }
}
