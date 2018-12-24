using System.Collections.Generic;
using DjLive.SdkModel.Enum;

namespace DjLive.SdkModel.Model
{
    public class DomainModel
    {
        public string Id { get; set; }
        public string SourceDomain { get; set; }
        public string Description { get; set; }
        public SourceStreamType SourceType { get; set; }
        public StateType StateType { get; set; }

        public string RtmpPlayDomain { get; set; }
        public string FlvPlayDomain { get; set; }
        public string HlsPlayDomain { get; set; }

        public KeyNamePair ServerPair { get; set; } = new KeyNamePair("","");
        public List<KeyNamePair> TranscodeIdPair { get; set; }
        public KeyNamePair SecurePolicyPair { get; set; } = new KeyNamePair("", "");
        public KeyNamePair RecordTemplatePair { get; set; } = new KeyNamePair("", "");

    }
}