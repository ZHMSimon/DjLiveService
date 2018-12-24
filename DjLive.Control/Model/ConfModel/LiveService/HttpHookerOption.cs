namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class HttpHookerOption
    {
        public string enabled { get; set; } = "on";
        //可多个回调
        public string on_connect { get; set; }
        public string on_close { get; set; }
        public string on_publish { get; set; }
        public string on_unpublish { get; set; }
        public string on_play { get; set; }
        public string on_stop { get; set; }
        public string on_dvr { get; set; }
        //hls 回调 忽略任何返回信息.
        public string on_hls_notify { get; set; }
    }
}