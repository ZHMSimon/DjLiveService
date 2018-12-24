using System;

namespace DjUtil.Tools.RetryPolicy
{
    public class RetryingEventArgs : EventArgs
    {
        public int CurrentRetryCount
        {
            get;
            private set;
        }
        public TimeSpan Delay
        {
            get;
            private set;
        }
        public Exception LastException
        {
            get;
            private set;
        }
        public RetryingEventArgs(int currentRetryCount, TimeSpan delay, Exception lastException)
        {
            this.CurrentRetryCount = currentRetryCount;
            this.Delay = delay;
            this.LastException = lastException;
        }
    }
}