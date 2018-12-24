namespace DjLive.Sdk.Util
{
    public class SdkErrorMessage
    {
        public static SdkErrorMessage OK
        {
            get
            {
                return new SdkErrorMessage()
                {
                    Code = SdkErrorType.Success,
                    Message = "消息处理成功.",
                };
            }
        }
        public SdkErrorType Code { get; set; }
        public string Message { get; set; }
    }
}