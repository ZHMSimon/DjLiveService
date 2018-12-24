using DjLive.CPDao.Util;

namespace DjLive.CPService.Util
{
    public class ServiceResultBase
    {
        public ServiceResultCode code { get; set; }
        public string Message { get; set; }
        public object Para { get; set; }
        public string ErrorId { get; set; }
        public static ServiceResultMessage DaoResult2ServiceResult(DaoResultMessage message)
        {
            var serviceMessage = new ServiceResultMessage();
            serviceMessage.Message = message.Message;
            serviceMessage.Para = message.para;
            switch (message.Code)
            {
                case DaoResultCode.ItemNotExist:
                {
                    serviceMessage.code = ServiceResultCode.ItemNotExist;
                    break;
                }
                case DaoResultCode.Success:
                {
                    serviceMessage.code = ServiceResultCode.Success;
                    break;
                }
                case DaoResultCode.UnExpectError:
                {
                    serviceMessage.code = ServiceResultCode.UnExceptError;
                    break;
                }
                default:
                {
                    serviceMessage.code = ServiceResultCode.UnDefineError;
                    break;
                }
            }
            return serviceMessage;
        }
    }
}