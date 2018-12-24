using System;

namespace DjLive.SdkModel.Model
{
    public class BoardCastRoomModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string AppName { get; set; }
        public string StreamName { get; set; }
        public int State { get; set; }
        public DateTime ExpireTime { get; set; }
        public string PublishNotifyUrl { get; set; }
        public string PublishEndUrl { get; set; }
        public string PlayNotifyUrl { get; set; }
        public string PlayEndUrl { get; set; }
    }
}