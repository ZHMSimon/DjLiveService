namespace DjLive.SdkModel
{
    public class DjLiveResponse<T>
    {
        public ApiCode ApiCode { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }
        public int TotalCount { get; set; }

        public DjLiveResponse()
        {
            
        }
        public DjLiveResponse(T obj)
        {
            ApiCode = ApiCode.Success;
            Message = "成功";
            Content = obj;
        }
    }
}