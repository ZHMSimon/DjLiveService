namespace DjLive.Control.Model.WebModel.Data
{
    public class SystemProcStatsData
    {
        /// <summary>
        /// 
        /// </summary>
        public string ok { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sample_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int nice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int idle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int iowait { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int irq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int softirq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int steal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int guest { get; set; }
    }
}