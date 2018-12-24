namespace DjLive.SdkModel.Model
{
    public class ServerNodeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string  Url { get; set; }
        public string  Ip { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int SshPort { get; set; }
        public int RtmpPort { get; set; }
        public int ApiPort { get; set; }
        public int HttpPort { get; set; }
    }
}