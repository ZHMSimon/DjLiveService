using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DjLive.Control.Model.ConfModel.LiveService;
using DjLive.Control.Service.Impl;
using DjLive.CPDao.Entity;
using DjLive.SdkModel.Enum;
using DjLive.SdkModel.Model;
using DjUtil.Tools;
using Newtonsoft.Json;

namespace DjLive.CPService.Util
{
    public  static class ServiceExtention
    {
        public static TranscodeOption Parse2Conf(this List<TranscodeTemplateEntity> transcodeTemplate)
        {
            if (transcodeTemplate != null && transcodeTemplate.Count > 0)
            {
                List<EngineOption> engineList = new List<EngineOption>();
                foreach (TranscodeTemplateEntity entity in transcodeTemplate)
                {
                    LogoTemplateEntity logoTemplate = entity.LogoTemplate;
                    EngineOption engine = new EngineOption(entity.Name);
                    var videoOption = string.IsNullOrEmpty(entity.VideoOption) ? null : JsonConvert.DeserializeObject<VideoOptionModel>(entity.VideoOption);
                    var audioOption = string.IsNullOrEmpty(entity.AudioOption) ? null : JsonConvert.DeserializeObject<AudioOptionModel>(entity.AudioOption);

                    if (logoTemplate != null)
                    {
                        engine.vfilter = VFilterOption.LogoOption($"./etc/{logoTemplate.Id}.png");
                        if (videoOption == null)
                        {
                            videoOption = new VideoOptionModel()
                            {
                                BitRate = 800,
                                Type = VideoCodeType.Baseline,
                            };
                        }
                    }
                    engine.vcodec = videoOption == null ? "copy" : "libx264";
                    engine.vprofile = videoOption?.Type.ToString() ?? "";
                    engine.vbitrate = videoOption?.BitRate ?? 0;
                    engine.vfps = videoOption?.Fps ?? 0;
                    engine.vwidth = videoOption?.Width ?? 0;
                    engine.vheight = videoOption?.Height ?? 0;

                    engine.acodec = audioOption == null ? "copy" : "libfdk_aac";
                    engine.abitrate = audioOption?.BitRate ?? 0;
                    engine.asample_rate = audioOption?.SampleRate ?? 0;
                    engineList.Add(engine);
                }
                TranscodeOption transcode = new TranscodeOption()
                {
                    enabled = "on",
                    EngineOptions = engineList,
                };
                return transcode;
            }
            else
            {
                return null;
            }
        }
        public static SecurityOption Parse2Conf(this SecurePolicyEntity secureEntity)
        {
            SecurityOption option = new SecurityOption();
            option.enabled = "off";
            option.settings = new List<SecuritySetting>();
            return option;
        }
        public static DvRecordOption Parse2Conf(this RecordTemplateEntity secureEntity)
        {
            DvRecordOption option = new DvRecordOption();
            if (secureEntity != null)
            {
                option.enabled = "on";
            }
            else
            {
                option.enabled = "off";
            }
            return option;

        }
        public static HttpHookerOption Parse2HookConf(this SecurePolicyEntity secureEntity)
        {
            HttpHookerOption option = new HttpHookerOption();
            option.enabled = "on";

            option.on_connect = secureEntity.AuthConnectUrl;
            option.on_close = secureEntity.AuthCloseUrl;
            option.on_dvr = secureEntity.AuthDvrUrl;
            option.on_stop = secureEntity.AuthStopUrl;
            option.on_unpublish = secureEntity.AuthUnPublishUrl;
            option.on_hls_notify = secureEntity.NotifyHlsUrl;
            option.on_play = secureEntity.AuthPlayUrl;
            option.on_publish = secureEntity.AuthPublishUrl;
            return option;
        }
        public static VHostOption Parse2Conf(this DomainEntity domain)
        {
            TranscodeOption transcodeOption = domain.TranscodeList?.Parse2Conf();
            SecurityOption securityOption = domain.SecurePolicy?.Parse2Conf();
            VHostOption vHostOption = new VHostOption(domain.SourceDomain);
            vHostOption.TranscodeOptions = new List<TranscodeOption>();
            if (transcodeOption != null)
            {
                vHostOption.TranscodeOptions.Add(transcodeOption);
            }
            vHostOption.security = securityOption;
            vHostOption.http_hooks = domain.SecurePolicy?.Parse2HookConf();
            if (domain.RecordTemplate != null)
            {
                if (vHostOption.http_hooks  == null) vHostOption.http_hooks = new HttpHookerOption();
                vHostOption.dvr = domain.RecordTemplate?.Parse2Conf();
                vHostOption.http_hooks.on_dvr += $" {ConfigurationValue.DefaultCallbackDomain}api/State/DvrCallback";
            }
            //todo:在这增加拉流设置拉流功能.

            return vHostOption;
        }
        public static List<VHostOption> Parse2Conf(this List<DomainEntity> domains)
        {
            List < VHostOption > vhostList = new List<VHostOption>();
            foreach (var domain in domains)
            {
                vhostList.Add(domain.Parse2Conf());
            }
            return vhostList;
        }
        public static HostOption Parse2Conf(this ServerEntity server)
        {
            var serviceConfig = JsonConvert.DeserializeObject<LiveServiceConfig>(server.Option);
           
            HostOption hostOption = new HostOption();
            if (serviceConfig != null)
            {
                hostOption.listen = serviceConfig.RtmpPort != 0 ? serviceConfig.RtmpPort : hostOption.listen;
                hostOption.http_api = new ApiOption("")
                {
                    listen = serviceConfig.ApiPort != 0 ? serviceConfig.ApiPort : 1985,
                };
                hostOption.http_server = new HttpOption("")
                {
                    listen = serviceConfig.HttpPort != 0 ? serviceConfig.HttpPort : 8080,
                };
            }
            if (server.Domains != null)
            {
                foreach (DomainEntity serverDomain in server.Domains)
                {
                    hostOption.VHostOptions.Add(serverDomain.Parse2Conf());
                }   
            }
            return hostOption;

        }
    }
}
