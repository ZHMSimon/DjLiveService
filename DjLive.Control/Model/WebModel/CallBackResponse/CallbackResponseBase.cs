namespace DjLive.Control.Model.WebModel.CallBackResponse
{
    /// <summary>
    /// 事件：发生该事件时，即回调指定的HTTP地址。
    ///HTTP地址：可以支持多个，以空格分隔，SRS会依次回调这些接口。
    ///数据：SRS将数据POST到HTTP接口。
    ///返回值：SRS要求HTTP服务器返回HTTP200并且response内容为整数错误码（0表示成功），其他错误码会断开客户端连接。
    /// </summary>
    public class CallbackResponseBase
    {
        public string action { get; set; }
        public int client_id { get; set; }
        public string ip { get; set; }
        public string vhost { get; set; }
        public string app { get; set; }

    }
}