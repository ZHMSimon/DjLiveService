namespace DjLive.Control.Model.WebModel.CallBackResponse
{
    /// <summary>
    /// 当客户端连接到指定的vhost和app时
    /// </summary>
    public class OnConnectResponse : CallbackResponseBase
    {
        public string tcUrl { get; set; }
        public string pageUrl { get; set; }
    }
}