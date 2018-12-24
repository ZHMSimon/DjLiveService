using System;
using DjLive.Sdk;
using DjLive.Sdk.Model;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SdkUnitTest
{
    [TestClass]
    public class TranscodeTemplateTest
    {
        private TranscodeTemplateModel DefaultModel = new TranscodeTemplateModel()
        {
            AppName = "xxx",
            AudioOption = new AudioOptionModel(),
            Description = "test",
            Name = "Unit_Test",
            VideoOption = new VideoOptionModel()
        };
        [TestMethod]
        public void GetTranscodeTemplateTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().TranscodeTemplateService.CreateTranscodeTemplateModel(DefaultModel);
            var defaults = ApiManager.GetInstance().TranscodeTemplateService.GetTranscodeTemplateById(add.Content?.Id);
            ApiManager.GetInstance().TranscodeTemplateService.DeleteTranscodeTemplateModel(add.Content?.Id);
            Assert.AreEqual(ApiCode.Success, defaults.ApiCode);
        }
        [TestMethod]
        public void AddTranscodeTemplateTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().TranscodeTemplateService.CreateTranscodeTemplateModel(DefaultModel);
            Assert.AreEqual(ApiCode.Success,add.ApiCode);
            Assert.IsNotNull(add.Content);
            Assert.AreEqual(add.Content?.Name, add.Content?.Name);
            ApiManager.GetInstance().TranscodeTemplateService.DeleteTranscodeTemplateModel(add.Content?.Id);
        }

        [TestMethod]
        public void DeleteTranscodeTemplateTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().TranscodeTemplateService.CreateTranscodeTemplateModel(DefaultModel);
            var delete = ApiManager.GetInstance().TranscodeTemplateService.DeleteTranscodeTemplateModel(add.Content?.Id);
            Assert.AreEqual(ApiCode.Success,delete.ApiCode);
            Assert.AreEqual(add.Content?.Name, delete.Content?.Name);
        }

        [TestMethod]
        public void UpdateTranscodeTemplateTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var add = ApiManager.GetInstance().TranscodeTemplateService.CreateTranscodeTemplateModel(DefaultModel);
            add.Content.Description = "1";
            var update = ApiManager.GetInstance().TranscodeTemplateService.UpdateTranscodeTemplateModel(add.Content?.Id, add.Content);
            Assert.AreEqual( ApiCode.Success,update.ApiCode);
            Assert.AreEqual(add.Content.Description,update.Content?.Description);

            var delete = ApiManager.GetInstance().TranscodeTemplateService.DeleteTranscodeTemplateModel(add.Content?.Id);
        }

    }
}
