using System;

namespace DjUtil.Tools.RetryPolicy
{
    public delegate bool ShouldRetry(int retryCount, Exception lastException, out TimeSpan delay);
}