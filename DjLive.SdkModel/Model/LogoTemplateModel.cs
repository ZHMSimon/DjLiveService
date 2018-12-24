namespace DjLive.SdkModel.Model
{
    public class LogoTemplateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Base64String { get; set; }
        //todo:位置,偏移,缩放方式
    }
}