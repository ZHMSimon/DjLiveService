namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class DvRecordOption
    {
        public string enabled { get; set; } = "off";
        /// <summary>
        /// session  segment  append
        /// </summary>
        public string dvr_plan { get; set; } = "session";
        /// <summary>
        ///  [vhost], the vhost of stream.
        ///  [app], the app of stream.
        ///  [stream], the stream name of stream.
        ///  [2006], replace this const to current year.
        ///  [01], replace this const to current month.
        ///  [02], replace this const to current date.
        ///  [15], replace this const to current hour.
        ///  [04], repleace this const to current minute.
        ///  [05], repleace this const to current second.
        ///  [999], repleace this const to current millisecond.
        ///  [timestamp],replace this const to current UNIX timestamp in ms.
        /// </summary>
        public string dvr_path { get; set; } = "./srs/trunk/objs/nginx/html/[app]/[stream].[timestamp].flv";
        /// <summary>
        /// 分段长度
        ///  segment apply it.
        ///  session,append ignore.
        /// </summary>
        public int dvr_duration { get; set; } = 30;
        public string dvr_wait_keyframe { get; set; } = "on";
        public string time_jitter { get; set; } = "full";

    }
}