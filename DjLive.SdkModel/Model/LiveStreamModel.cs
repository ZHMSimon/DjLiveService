namespace DjLive.SdkModel.Model
{
    public class LiveStreamModel
    {
        public object SrsHostId { get; set; }
        public object SrsStreamId { get; set; }
        public int SrsServerId { get; set; }
        public string Name { get; set; }
        public int ClientCount { get; set; }
        public long SendFlowKB { get; set; }
        public long RecvFlowKB { get; set; }
        public long SendFlowRateKBs { get; set; }
        public long RecvFlowRateKBs { get; set; }
        public string App { get; set; }
        public bool Active { get; set; }
        public string VideoCode { get; set; }
        public string VideoProfile { get; set; }
        public string VideoLevel { get; set; }
        public int? AudioSampleRate { get; set; }
        public string AudioCode { get; set; }
        public string AudioProfile { get; set; }
        public int? AudioChannel { get; set; }
    }
}