using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DjLive.SdkModel.Model
{
    public class SecurePolicyModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        //推流验证地址
        public string AuthPublishUrl { get; set; }
        //播放验证地址
        public string AuthPlayUrl { get; set; }
        //连接验证地址
        public string AuthConnectUrl { get; set; }
        //断开连接地址
        public string AuthCloseUrl { get; set; }
        //取消推流地址
        public string AuthUnPublishUrl { get; set; }
        //开始录制提示地址
        public string AuthDvrUrl { get; set; }
        //结束录制提示地址
        public string AuthStopUrl { get; set; }
        //hls 观看提醒
        public string NotifyHlsUrl { get; set; }

    }
}
