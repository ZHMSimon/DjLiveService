using System.Collections.Generic;

namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class TranscodeOption
    {
        /// <summary>
        /// [app]/[stream]
        /// </summary>
        public string filter;
        public string enabled { get; set; } = "off";
        public string ffmpeg { get; set; } = "./srs/trunk/objs/ffmpeg/bin/ffmpeg";
        public List<EngineOption> EngineOptions { get; set; }
    }
}