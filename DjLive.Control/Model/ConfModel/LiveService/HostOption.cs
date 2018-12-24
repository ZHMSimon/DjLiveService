using System.Collections.Generic;

namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class HostOption
    {
        /// <summary>
        /// [ip:]port
        /// rtmp 服务监听端口号
        /// </summary>
        public int listen { get; set; } = 1935;
        /// <summary>
        /// 最大连接数 需要修改linux最大连接数 
        /// 修改: ulimit -HSn 8000+7 
        /// 查询: ulimit -n
        /// </summary>
        public int max_connections { get; set; } = 1000;
        /// <summary>
        /// max:65536 min:128
        /// 分块大小
        /// </summary>
        public int chunk_size { get; set; } = 30000;
        public string ff_log_dir { get; set; } = "./srs/trunk/objs";
        /// <summary>
        ///  pid文件路径
        /// 标识独立进程
        /// </summary>
        public string pid { get; set; } = "./srs/trunk/objs/srs.pid";

        /// <summary>
        /// file / console 
        /// 控制台
        /// </summary>
        public string srs_log_tank { get; set; } = "file";
        /// <summary>
        /// verbose, info, trace, warn, error
        /// 日志等级
        /// </summary>
        public string srs_log_level { get; set; } = "trace";
        /// <summary>
        /// 日志保存位置
        /// </summary>
        public string srs_log_file { get; set; } = "./srs/trunk/objs/srs.log";

        /// <summary>
        /// on / off 
        /// 守护进程启动
        /// </summary>
        public string daemon { get; set; } = "on";

        public ApiOption http_api { get; set; }
        public HeartBeatOption heartbeat { get; set; }
        public StatOption stats { get; set; }
        public HttpOption http_server { get; set; }
        public List<VHostOption> VHostOptions { get; set; } = new List<VHostOption>();
    }
}