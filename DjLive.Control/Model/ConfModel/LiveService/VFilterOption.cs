namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class VFilterOption
    {
        public static VFilterOption MirrorOption { get; } = new VFilterOption()
        {
            vf = @"split [main][tmp]; [tmp] crop=iw:ih/2:0:0, vflip [flip]; [main][flip] overlay=0:H/2",
        };

        public static VFilterOption LogoOption(string logoPath = "./etc/ffmpeg-logo.png")
        {
            return new VFilterOption()
            {
                i = logoPath,
                filter_complex = "overlay=10:10",
            };
        }

        /// <summary>
        /// logo file
        /// </summary>
        public string i { get; set; }
        /// <summary>
        /// ffmpeg filter
        /// </summary>
        public string filter_complex { get; set; }

        public string vf { get; set; }
    }
}