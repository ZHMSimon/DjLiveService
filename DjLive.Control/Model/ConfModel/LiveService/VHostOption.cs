using System.Collections.Generic;

namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class VHostOption
    {
        public VHostOption(string _host)
        {
            if (!string.IsNullOrWhiteSpace(_host))
            {
                host = _host;
            }
            else
            {
                host = "__defaultVhost__";
            }
        }

        public string host;
        /// <summary>
        /// local  remote
        /// </summary>
        public string mode { get; set; } = "local";
        public string origin { get; set; } = "";
        public string token_traverse { get; set; } = "off";
        public string forward { get; set; }
        public string gop_cache { get; set; } = "off";
        public string min_latency { get; set; } = "on";
        public int queue_length { get; set; } = 5;
        public int mw_latency { get; set; } = 100;
        public string tcp_nodelay { get; set; } = "on";
        public MergedReadOption mr { get; set; } = new MergedReadOption();

        public HlsOption hls { get; set; }

        public DvRecordOption dvr { get; set; }

        public RemuxOption http_remux { get; set; }

        public SecurityOption security { get; set; }

        public List<TranscodeOption> TranscodeOptions { get; set; }

        public HttpHookerOption http_hooks { get; set; }

    }
}