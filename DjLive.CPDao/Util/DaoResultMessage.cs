namespace DjLive.CPDao.Util
{
    public class DaoResultMessage
    {
        public static DaoResultMessage OkWithParaNull
        {
            get { return new DaoResultMessage()
            {
                Code = DaoResultCode.Success,
                Message = "允许参数为空.",
            };}
        }
        public static DaoResultMessage ItemNotExist
        {
            get
            {
                return new DaoResultMessage()
                {
                    Code = DaoResultCode.ItemNotExist,
                    Message = "对象不存在..",
                };
            }
        }

        public DaoResultCode Code { get; set; }
        public string Message { get; set; }
        public object para { get; set; }
    }
}