using System;

namespace DjUtil.Tools.RetryPolicy
{
    public class DefaultRetryStrategy : RetryStrategy
    {
        private readonly int m_retryCount;
        private readonly TimeSpan m_retryInterval;

        public DefaultRetryStrategy() : this("DefaultRetryStrategy", RetryStrategy.DefaultRetryCount, RetryStrategy.DefaultRetryInterval)
        {

        }
        public DefaultRetryStrategy(int retryCount, TimeSpan retryInterval) : this("DefaultRetryStrategy", retryCount, retryInterval)
        {

        }
        public DefaultRetryStrategy(string name, int retryCount, TimeSpan retryInterval) : base(name)
        {
            m_retryCount = retryCount;
            m_retryInterval = retryInterval;
        }

        public override ShouldRetry GetShouldRetry()
        {
            if (this.m_retryCount == 0)
            {
                return delegate (int currentRetryCount, Exception lastException, out TimeSpan interval)
                {
                    interval = TimeSpan.Zero;
                    return false;
                };
            }
            return delegate (int currentRetryCount, Exception lastException, out TimeSpan interval)
            {
                if (currentRetryCount < this.m_retryCount)
                {
                    interval = this.m_retryInterval;
                    return true;
                }
                interval = TimeSpan.Zero;
                return false;
            };
        }
    }
}