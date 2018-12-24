using System;
using System.Threading.Tasks;
using DjLive.Control.Model.ConfModel.LiveService;
using DjLive.Control.Model.WebModel.Data;

namespace DjLive.Control.Service
{
    public interface IDjLiveInterface
    {
        /// <summary>
        /// 安装 直播服务
        /// </summary>
        /// <param name="resultCallback"></param>
        /// 
        void InstallLiveService(Action<int, string> resultCallback,bool repair = false);
        /// <summary>
        /// 保存设置配置.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="confName"></param>
        /// <returns>conf path</returns>
        string SetLiveServiceConf(HostOption option, string confName);
        /// <summary>
        /// 获取设置配置
        /// </summary>
        /// <param name="confName"></param>
        /// <returns>配置信息 string</returns>
        HostOption GetLiveServiceConf(string confName);
        /// <summary>
        /// 开启直播服务
        /// </summary>
        void StartLiveService(string confName, Action<int, string> resultCallback);
        /// <summary>
        /// 关闭直播服务
        /// </summary>
        void StopLiveService();
        /// <summary>
        /// 热更新直播配置
        /// </summary>
        /// <param name="confName"></param>
        void ReloadHotService(string confName, Action<int, string> resultCallback);

        #region API

        Task<VersionRoot> GetVersionInfo();
        Task<SummaryRoot> GetSummaryInfo();
        Task<ResourceUsedRoot> GetResourceUsedInfo();
        Task<SystemProcStatsRoot> GetSystemProcStatsInfo();
        Task<SelfProcessStatRoot> GetSelfProcessStatInfo();
        Task<MemRoot> GetMemInfo();
        Task<AuthorRoot> GetAuthorInfo();
        Task<FeatherRoot> GetFeatherInfo();
        Task<VhostRoot> GetVhostInfo();
        Task<StreamRoot> GetStreamInfo();
        Task<ClientRoot> GetClientInfo();

        Task DisConnectClientById(string id);
        Task DisConnectClientByApp(string app);
        Task DisConnectClientByStream(string app, string stream);

        #endregion
    }
}