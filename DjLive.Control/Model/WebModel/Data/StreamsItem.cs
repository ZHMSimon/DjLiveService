namespace DjLive.Control.Model.WebModel.Data
{
    public class StreamsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int vhost { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string app { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long live_ms { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int clients { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int frames { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long send_bytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long recv_bytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Kbps kbps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PublishState publish { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public VideoInfo video { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AudioInfo audio { get; set; }
    }
}