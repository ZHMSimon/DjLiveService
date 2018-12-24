using System.Collections.Generic;

namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class EngineOption
    {
        public EngineOption(string _name)
        {
            name = _name;
        }
        public static EngineOption SuperEngineOption(string name = "ffsuper")
        {
            return new EngineOption(name)
            {
                enabled = "on",
                iformat = "flv",
                vfilter = VFilterOption.LogoOption(),
                vcodec = "libx264",
                vbitrate = 1500,
                vfps = 25,
                vwidth = 768,
                vheight = 320,
                vthreads = 12,
                vprofile = "main",
                vpreset = "medium",
                vparams = new Dictionary<string, string>() { { "t", "100" }, { "coder", "1" }, { "b_strategy", "2" }, { "bf", "3" }, { "refs", "10" } },
                acodec = "libfdk_aac",
                abitrate = 70,
                asample_rate = 44100,
                achannels = 2,
                aparams = new Dictionary<string, string>() { { "profile:a", "acc_low" } },
                oformat = "flv",
                output = "rtmp://127.0.0.1:[port]/[app]?vhost=[vhost]/[stream]_[engine]",
            };
        }

        public static EngineOption HdEngineOption(string name = "ffhd")
        {
            return new EngineOption(name)
            {
                enabled = "on",
                vcodec = "libx264",
                vbitrate = 1200,
                vfps = 25,
                vwidth = 1382,
                vheight = 576,
                vthreads = 6,
                vprofile = "main",
                vpreset = "medium",
                acodec = "libfdk_aac",
                abitrate = 70,
                asample_rate = 44100,
                achannels = 2,
                output = "rtmp://127.0.0.1:[port]/[app]?vhost=[vhost]/[stream]_[engine]",
            };
        }

        public static EngineOption SdEngineOption(string name = "ffsd")
        {
            return new EngineOption(name)
            {
                enabled = "on",
                vcodec = "libx264",
                vbitrate = 800,
                vfps = 25,
                vwidth = 1152,
                vheight = 480,
                vthreads = 4,
                vprofile = "main",
                vpreset = "fast",
                acodec = "libfdk_aac",
                abitrate = 60,
                asample_rate = 44100,
                achannels = 2,
                output = "rtmp://127.0.0.1:[port]/[app]?vhost=[vhost]/[stream]_[engine]",
            };
        }

        public static EngineOption FastEngineOption(string name = "fffast")
        {
            return new EngineOption(name)
            {
                enabled = "on",
                vcodec = "libx264",
                vbitrate = 300,
                vfps = 20,
                vwidth = 768,
                vheight = 320,
                vthreads = 2,
                vprofile = "baseline",
                vpreset = "superfast",
                acodec = "libfdk_aac",
                abitrate = 45,
                asample_rate = 44100,
                achannels = 2,
                output = "rtmp://127.0.0.1:[port]/[app]?vhost=[vhost]/[stream]_[engine]",
            };
        }

        public static EngineOption CopyEngineOption(string name = "ffcopy")
        {
            return new EngineOption(name)
            {
                enabled = "on",
                vcodec = "copy",
                acodec = "copy",
                output = "rtmp://127.0.0.1:[port]/[app]?vhost=[vhost]/[stream]_[engine]",
            };
        }

        public string name;
        public string enabled { get; set; } = "on";
        /// <summary>
        /// off,flv,other format, for example, mp4/aac whatever
        /// </summary>
        public string iformat { get; set; } = "";

        public VFilterOption vfilter { get; set; }

        /// <summary>
        /// libx264,copy,vn
        /// </summary>
        public string vcodec { get; set; } = "copy";
        /// <summary>
        /// kbs 比特率
        /// </summary>
        public int vbitrate { get; set; } = 0;
        /// <summary>
        /// fps
        /// </summary>
        public int vfps { get; set; } = 0;
        public int vwidth { get; set; } = 0;
        public int vheight { get; set; } = 0;
        /// <summary>
        /// ffmpeg 可用线程
        /// </summary>
        public int vthreads { get; set; } = 1;
        /// <summary>
        ///  high,main,baseline
        /// </summary>
        public string vprofile { get; set; } = "main";
        /// <summary>
        /// ultrafast,superfast,veryfast,faster,fast,medium,slow,slower,veryslow,placebo
        /// </summary>
        public string vpreset { get; set; } = "medium";
        /// <summary>
        /// http://ffmpeg.org/ffmpeg.html
        /// http://ffmpeg.org/ffmpeg-codecs.html#libx264
        /// </summary>
        public Dictionary<string, string> vparams { get; set; }
        //audio encoder name. can be:
        //  libfdk_aac: use aac(libfdk_aac) audio encoder.
        //  copy: donot encoder the audio stream, copy it.
        //  an: disable audio output.
        public string acodec { get; set; } = "copy";
        //audio bitrate, in kbps. [16, 72] for libfdk_aac.
        //@remark 0 to use source audio bitrate.
        //default: 0
        public int abitrate { get; set; } = 0;
        //audio sample rate. for flv/rtmp, it must be:
        //      44100,22050,11025,5512
        //@remark 0 to use source audio sample rate.
        //default: 0
        public int asample_rate { get; set; } = 0;
        //audio channel, 1 for mono, 2 for stereo.
        //@remark 0 to use source audio channels.
        //default: 0
        public int achannels { get; set; } = 0;
        public Dictionary<string, string> aparams { get; set; }
        //output format, can be:
        //      off, do not specifies the format, ffmpeg will guess it.
        //      flv, for flv or RTMP stream.
        //      other format, for example, mp4/aac whatever.
        //default: flv
        public string oformat { get; set; } = "";
        //output stream. variables:
        //      [vhost] the input stream vhost.
        //      [port] the intput stream port.
        //      [app] the input stream app.
        //      [stream] the input stream name.
        //      [engine] the tanscode engine name.
        public string output { get; set; } = "rtmp://127.0.0.1:[port]/[app]?vhost=[vhost]/[stream]_[engine]";
    }
}