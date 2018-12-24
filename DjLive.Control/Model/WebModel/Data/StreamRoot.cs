using System.Collections.Generic;

namespace DjLive.Control.Model.WebModel.Data
{
    public class StreamRoot
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
        public List<StreamsItem> streams { get; set; }
    }
}