namespace DjLive.Control.Service.Impl
{
    public class LiveServiceConfig
    {
        public string Host  { get; set; }
        public string HostUrl  { get; set; }
        public string UserName  { get; set; }
        public string Password  { get; set; }
        public int SshPort { get; set; } = 22;
        public int ApiPort { get; set; } = 1985;
        public int HttpPort { get; set; } = 8080;
        public int RtmpPort { get; set; } = 1935;
    }
}