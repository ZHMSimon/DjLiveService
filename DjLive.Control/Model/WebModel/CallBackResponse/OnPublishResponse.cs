namespace DjLive.Control.Model.WebModel.CallBackResponse
{
    /// <summary>
    /// 当客户端发布流时，譬如flash/FMLE方式推流到服务器
    /// </summary>
    public class OnPublishResponse : CallbackResponseBase
    {
        public string stream { get; set; }
    }
}