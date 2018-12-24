namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class HdsOption
    {
        public string enabled { get; set; } = "on";
        public int hds_fragment { get; set; } = 10;
        public int hds_window { get; set; } = 60;
        public string hds_path { get; set; } = "./srs/trunk/objs/nginx/html";
    }
}