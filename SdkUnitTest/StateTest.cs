using System;
using DjLive.Sdk;
using DjLive.Sdk.Model;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SdkUnitTest
{
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public void GetVhostsTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var defaults = ApiManager.GetInstance().StateService.GetVhostsState();
            Assert.AreEqual(ApiCode.Success,defaults.ApiCode);

        }
        [TestMethod]
        public void GetStreamsTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var defaults = ApiManager.GetInstance().StateService.GetStreamsState();
            Assert.AreEqual(ApiCode.Success, defaults.ApiCode);

        }
        [TestMethod]
        public void GetClientsTest()
        {
            TokenManager.GetInstance().InitServicePoint(GlobalSetting.ServerPoint, HttpScheme.Http);
            var logResult = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;
            var defaults = ApiManager.GetInstance().StateService.GetClientsState();
            Assert.AreEqual(ApiCode.Success,defaults.ApiCode);

        }
    }
}
