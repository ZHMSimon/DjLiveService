namespace DjLive.Control.Model.WebModel.Data
{
    public class LiveHostSystemSummary
    {
        /// <summary>
        /// 
        /// </summary>
        public double cpu_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int disk_read_KBps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int disk_write_KBps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int disk_busy_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int mem_ram_kbyte { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double mem_ram_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int mem_swap_kbyte { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int mem_swap_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cpus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cpus_online { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double uptime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double ilde_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double load_1m { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double load_5m { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double load_15m { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int net_sample_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int net_recv_bytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int net_send_bytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int net_recvi_bytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int net_sendi_bytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int srs_sample_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int srs_recv_bytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int srs_send_bytes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int conn_sys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int conn_sys_et { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int conn_sys_tw { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int conn_sys_udp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int conn_srs { get; set; }
    }
}