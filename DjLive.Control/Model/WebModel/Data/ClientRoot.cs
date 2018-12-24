using System.Collections.Generic;

namespace DjLive.Control.Model.WebModel.Data
{
    public class ClientRoot
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
        public List<ClientsItem> clients { get; set; }
    }
}