using System.Collections.Generic;

namespace DjLive.SdkModel.Model
{
    public class TranscodeTemplateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AppName { get; set; }
        public string Description { get; set; }
        public KeyNamePair LogoTemplatePair { get; set; } = new KeyNamePair("", "");
        public VideoOptionModel VideoOption { get; set; }
        public AudioOptionModel AudioOption { get; set; }
    }
}