using DjLive.SdkModel.Enum;

namespace DjLive.SdkModel.Model
{
    public class VideoOptionModel
    {
        public VideoCodeType Type { get; set; } = 0;
        //Kbps
        public int BitRate { get; set; } = 800;
        public int Fps { get; set; } = 20;
        public int Width { get; set; }
        public int Height { get; set; }
    }
}