namespace DjLive.Control.Model.WebModel.CallBackResponse
{
    /// <summary>
    /// 当客户端开始播放流时
    /// </summary>
    public class OnPlayResponse : CallbackResponseBase
    {
        public string stream { get; set; }
        public string pageUrl { get; set; }

    }
}