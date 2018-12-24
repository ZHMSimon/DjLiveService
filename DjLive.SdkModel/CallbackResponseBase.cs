namespace DjLive.SdkModel
{
    public class CallbackResponseBase
    {
        public string action { get; set; }
        public int client_id { get; set; }
        public string ip { get; set; }
        public string vhost { get; set; }
        public string app { get; set; }
        public string stream { get; set; }
        public string pageUrl { get; set; }
        public string tcUrl { get; set; }
        public string cwd { get; set; }
        public string file { get; set; }
        public long send_bytes { get; set; }
        public long recv_bytes { get; set; }
    }
}