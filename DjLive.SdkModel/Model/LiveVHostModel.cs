namespace DjLive.SdkModel.Model
{
    public class LiveVHostModel
    {
        public int SrsHostId { get; set; }
        public int SrsServerId { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
        public int ClientCount { get; set; }
        public int StreamCount { get; set; }
        public long SendFlowKB { get; set; }
        public long RecvFlowKB { get; set; }
        public long SendFlowRateKBs { get; set; }
        public long RecvFlowRateKBs { get; set; }
        public bool IsHaveHls { get; set; }
    }

    
}