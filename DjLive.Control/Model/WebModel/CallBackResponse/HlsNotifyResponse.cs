namespace DjLive.Control.Model.WebModel.CallBackResponse
{
    /// <summary>
    /// # when srs reap a ts file of hls, call this hook,
    ///# used to push file to cdn network, by get the ts file from cdn network.
    ///# so we use HTTP GET and use the variable following:
    ///#       [app], replace with the app.
    ///#       [stream], replace with the stream.
    ///#       [ts_url], replace with the ts url.
    ///# ignore any return data of server.
    ///# @remark random select a url to report, not report all.
    /// </summary>
    public class HlsNotifyResponse : CallbackResponseBase
    {
        public string stream { get; set; }
        public string tsUrl { get; set; }

    }
}