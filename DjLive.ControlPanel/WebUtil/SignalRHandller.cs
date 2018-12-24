using System;
using System.Collections.Generic;

namespace DjLive.ControlPanel.WebUtil
{
    /// <summary>
    /// SignalR 消息处理 全局方法
    /// </summary>
    public static class SignalRHandller
    {
        private static event Action<List<string>, string, string> SignalRMessageEvent;
        /// <summary>
        /// 添加 消息处理器
        /// </summary>
        /// <param name="handler"></param>
        public static void AppentMessageEventHandler(Action<List<string>, string, string> handler)
        {           
            SignalRMessageEvent += handler;
        }
        /// <summary>
        /// 移除消息处理器
        /// </summary>
        /// <param name="handler"></param>
        public static void RemoveMessageEventHandler(Action<List<string>, string, string> handler)
        {
            SignalRMessageEvent -= handler;
        }
        /// <summary>
        /// 清空消息处理器
        /// </summary>
        public static void ClearMessageEventHandler()
        {
            if (SignalRMessageEvent != null)
            {
                foreach (Delegate d in SignalRMessageEvent.GetInvocationList())
                {
                    var action = d as Action<List<string>, string, string>;
                    if (action != null)
                    {
                        SignalRMessageEvent -= action;
                    }
                }
            }

        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="userIdList"></param>
        /// <param name="message"></param>
        /// <param name="url"></param>
        public static void SendSignalRNotification2User(List<string> userIdList, string message, string url)
        {
            if (userIdList == null)return;
            SignalRMessageEvent?.Invoke(userIdList, message, url);
        }

        public static void SendSignalRNotification2All(string message, string url)
        {
            SignalRMessageEvent?.Invoke(null,message,url);
        }
    }
}