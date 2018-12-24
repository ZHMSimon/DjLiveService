using System;

namespace DjUtil.Tools.RetryPolicy
{
    public interface IRetryDetectionStrategy
    {
        bool IsTransient(Exception ex);
    }
}