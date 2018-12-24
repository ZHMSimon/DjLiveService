namespace DjLive.SdkModel
{
    public class ApiMessage<T>
    {
        public ApiCode Code { get; set; }
        public string Message { get; set; }
        public T ReturnValue { get; set; }
    }

}