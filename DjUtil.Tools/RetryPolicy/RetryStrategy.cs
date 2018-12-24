using System;

namespace DjUtil.Tools.RetryPolicy
{
    public abstract class RetryStrategy
    {
        public static readonly int DefaultRetryCount = 2;
        public static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(10.0);
        public string name;

        protected RetryStrategy(string strategyName)
        {
            name = strategyName;
        }
        public abstract ShouldRetry GetShouldRetry();

    }
}