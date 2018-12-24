namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class HlsOption
    {
        public string enabled { get; set; } = "on";
        public int hls_fragment { get; set; } = 3;
        public int hls_window { get; set; } = 20;
        public double hls_td_ratio { get; set; } = 1.5;
        public double hls_aof_ratio { get; set; } = 2.0;
        /// <summary>
        /// ignore, disconnect, continue
        /// </summary>
        public string hls_on_error { get; set; } = "continue";
        public string hls_path { get; set; } = "./srs/trunk/objs/nginx/html";
        public string hls_m3u8_file { get; set; } = "[app]/[stream].m3u8";
        public string hls_ts_file { get; set; } = "[app]/[stream]-[seq].ts";
        public string hls_ts_floor { get; set; } = "off";
        public string hls_entry_prefix { get; set; } = "";
        /// <summary>
        /// aac, mp3, an
        /// </summary>
        public string hls_acodec { get; set; } = "aac";
        /// <summary>
        ///  h264, vn
        /// </summary>
        public string hls_vcodec { get; set; } = "h264";
        public string hls_cleanup { get; set; } = "on";
        public string hls_wait_keyframe { get; set; } = "on";
        public int hls_dispose { get; set; } = 0;
        public int hls_nb_notify { get; set; } = 64;
    }
}