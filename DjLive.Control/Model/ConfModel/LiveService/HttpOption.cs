namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class HttpOption
    {
        public HttpOption(string _host)
        {
            host = _host;
        }
        public string host;
        public string enabled { get; set; } = "on";
        public int listen { get; set; } = 8080;
        public string dir { get; set; } = "./srs/trunk/objs/nginx/html";
    }
}