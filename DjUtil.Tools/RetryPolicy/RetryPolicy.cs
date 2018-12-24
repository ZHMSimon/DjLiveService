using System;
using System.Threading.Tasks;

namespace DjUtil.Tools.RetryPolicy
{
    public class RetryPolicy
    {
        public RetryStrategy DjRetryStrategy { get; private set; }
        public IRetryDetectionStrategy ErrorDetectionStrategy { get; private set; }

        public event EventHandler<RetryingEventArgs> Retrying;
        public RetryPolicy(IRetryDetectionStrategy errorDetectionStrategy, RetryStrategy retryStrategy)
        {
            if (errorDetectionStrategy != null && retryStrategy != null)
            {
                this.ErrorDetectionStrategy = errorDetectionStrategy;
                this.DjRetryStrategy = retryStrategy;
            }
        }
        /// <summary>
        /// 同步方式执行某无返回值操作，配以相应重试策略
        /// </summary>
        public virtual void ExecuteAction(Action action)
        {
            this.ExecuteAction<object>(delegate
            {
                action();
                return null;
            });
        }
        /// <summary>
        /// 同步方式执行某无返回值、带一个参数的操作，配以相应重试策略
        /// </summary>
        public virtual void ExecuteAction<T>(Action<T> action, T param)
        {
            this.ExecuteAction<object>(delegate
            {
                action(param);
                return null;
            });
        }

        /// <summary>
        /// 同步方式执行某有返回值操作，配以相应重试策略
        /// </summary>
        public virtual TResult ExecuteAction<TResult>(Func<TResult> func)
        {
            int num = 0;
            TimeSpan zero = TimeSpan.Zero;
            ShouldRetry shouldRetry = this.DjRetryStrategy.GetShouldRetry();
            TResult result;
            while (true)
            {
                Exception ex = null;
                try
                {
                    result = func();
                    break;
                }
                catch (Exception ex3)
                {
                    ex = ex3;
                    if (!this.ErrorDetectionStrategy.IsTransient(ex) || !shouldRetry(num++, ex, out zero))
                    {
                        throw;
                    }
                }
                if (zero.TotalMilliseconds < 0.0)
                {
                    zero = TimeSpan.Zero;
                }
                this.OnRetrying(num, ex, zero);
                if (num > 1)
                {
                    Task.Delay(zero).Wait();
                }
            }
            return result;
        }
        protected virtual void OnRetrying(int retryCount, Exception lastError, TimeSpan delay)
        {
            if (this.Retrying != null)
            {
                this.Retrying(this, new RetryingEventArgs(retryCount, delay, lastError));
            }
        }
    }
}