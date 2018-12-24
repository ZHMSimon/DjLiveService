using System.Collections.Generic;

namespace DjLive.ControlPanel.WebUtil
{
    /// <summary>
    /// 消息实体
    /// </summary>
    public class SignalRMessage
    {
        public List<string> UserIds { get; set; }
        public MessageType MessageType { get; set; }

    }
}