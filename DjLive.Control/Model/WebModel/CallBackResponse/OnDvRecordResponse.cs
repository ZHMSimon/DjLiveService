namespace DjLive.Control.Model.WebModel.CallBackResponse
{
    /// <summary>
    /// 当DVR录制关闭一个flv文件时
    /// </summary>
    public class OnDvRecordResponse : CallbackResponseBase
    {
        public string stream { get; set; }
        public string cwd { get; set; }
        public string file { get; set; }
    }
}