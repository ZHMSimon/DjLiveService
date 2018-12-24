using System.Collections.Generic;

namespace DjLive.Control.Model.WebModel.Data
{
    public class VhostRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int server { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<VhostsItem> vhosts { get; set; }
    }
}