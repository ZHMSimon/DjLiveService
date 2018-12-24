namespace DjLive.Control.Model.WebModel.CallBackResponse
{
    /// <summary>
    /// 当客户端停止发布流时
    /// </summary>
    public class OnUnPublishResponse : CallbackResponseBase
    {
        public string stream { get; set; }
    }
}