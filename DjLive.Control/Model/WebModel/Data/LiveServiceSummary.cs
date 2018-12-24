namespace DjLive.Control.Model.WebModel.Data
{
    public class LiveServiceSummary
    {
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ppid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string argv { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cwd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int mem_kbyte { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double mem_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double cpu_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double srs_uptime { get; set; }
    }
}