using DjLive.Sdk.ApiClient;
using DjLive.Sdk.ApiService;
using DjLive.Sdk.Util;

namespace DjLive.Sdk
{
    public class ApiManager:Singleton<ApiManager>
    {
        internal static ApiProxy<IRecordTemplateServiceInterface> RecordApiProxy { get; set; } = new ApiProxy<IRecordTemplateServiceInterface>();
        internal static ApiProxy<IVhostServiceInterface> VhostApiProxy { get; set; } = new ApiProxy<IVhostServiceInterface>();
        internal static ApiProxy<ILogoTemplateServiceInterface> LogoApiProxy { get; set; } = new ApiProxy<ILogoTemplateServiceInterface>();
        internal static ApiProxy<ISecurePolicyServiceInterface> SecureApiProxy { get; set; } = new ApiProxy<ISecurePolicyServiceInterface>();
        internal static ApiProxy<IStateServiceInterface> StateApiProxy { get; set; } = new ApiProxy<IStateServiceInterface>();
        internal static ApiProxy<ITranscodeTemplateServiceInterface> TranscodeApiProxy { get; set; } = new ApiProxy<ITranscodeTemplateServiceInterface>();

        public IRecordTemplateServiceInterface RecordTemplateService { get; set; } = ((IRecordTemplateServiceInterface)ApiManager.RecordApiProxy.GetTransparentProxy());
        public IVhostServiceInterface VhostService { get; set; } = ((IVhostServiceInterface)ApiManager.VhostApiProxy.GetTransparentProxy());
        public ILogoTemplateServiceInterface LogoTemplateService { get; set; } = ((ILogoTemplateServiceInterface)ApiManager.LogoApiProxy.GetTransparentProxy());
        public ISecurePolicyServiceInterface SecurePolicyService { get; set; } = ((ISecurePolicyServiceInterface)ApiManager.SecureApiProxy.GetTransparentProxy());
        public IStateServiceInterface StateService { get; set; } = ((IStateServiceInterface)ApiManager.StateApiProxy.GetTransparentProxy());
        public ITranscodeTemplateServiceInterface TranscodeTemplateService { get; set; } = ((ITranscodeTemplateServiceInterface)ApiManager.TranscodeApiProxy.GetTransparentProxy());
    }
}