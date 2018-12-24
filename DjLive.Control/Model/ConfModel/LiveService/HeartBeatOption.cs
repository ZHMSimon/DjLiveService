namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class HeartBeatOption
    {
        public string enabled { get; set; } = "on";
        public double interval { get; set; } = 9.3;
        public string url { get; set; }
        public string device_id { get; set; } = "my-srs-device";
        public string summaries { get; set; } = "off";
    }
}