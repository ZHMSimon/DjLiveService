namespace DjLive.SdkModel.Model
{
    public class LiveClientModel
    {
        public enum LiveClientType
        {
            未知,
            推流,
            播放,
        }
        public object SrsHostId { get; set; }
        public object SrsStreamId { get; set; }
        public int SrsServerId { get; set; }
        public string Name { get; set; }
        public int SrsClientId { get; set; }
        public string PageUrl { get; set; }
        public string SwfUrl { get; set; }
        public string TcUrl { get; set; }
        public string ClientIp { get; set; }
        public LiveClientType Type { get; set; }
        public string Url { get; set; }
        public bool IsPublish { get; set; }
        public string AliveTime { get; set; }
    }
}