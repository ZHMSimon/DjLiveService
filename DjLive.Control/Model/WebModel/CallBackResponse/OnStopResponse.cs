namespace DjLive.Control.Model.WebModel.CallBackResponse
{
    /// <summary>
    /// 当客户端停止播放时。备注：停止播放可能不会关闭连接，还能再继续播放。
    /// </summary>
    public class OnStopResponse : CallbackResponseBase
    {
        public string stream { get; set; }
    }
}