namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class ApiOption
    {
        public ApiOption(string _host)
        {
            host = _host;
        }
        public string host;
        public string enabled { get; set; } = "on";
        public int listen { get; set; } = 1985;
        public string crossdomain { get; set; } = "on";
    }
}