namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class RemuxOption
    {
        public string enabled { get; set; } = "on";
        public int fast_cache { get; set; } = 30;
        public string mount { get; set; } = "[vhost]/[app]/[stream].flv";
        public string hstrs { get; set; } = "on";
    }
}