using System;

namespace DjUtil.Tools.RetryPolicy
{
    public class CatchAllRetryDetectionStrategy : IRetryDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            return true;
        }
    }
}