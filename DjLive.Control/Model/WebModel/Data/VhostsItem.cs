namespace DjLive.Control.Model.WebModel.Data
{
    public class VhostsItem
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
        public string enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int clients { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int streams { get; set; }
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
        public VhostHls hls { get; set; }
    }
}