using System;
using System.Threading;

namespace WhenAnyWithCancellationToken
{
    public static class AsyncHelperExt
    {
        private static double DefaultTimeoutInSeconds = 20.0d;

        public static CancellationToken WithTimout(this CancellationToken token, TimeSpan timeout)
        {
            if(timeout == default)
                timeout = TimeSpan.FromSeconds(DefaultTimeoutInSeconds);

            var childCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(token);
            childCancellationToken.CancelAfter(timeout);

            return childCancellationToken.Token;
        }
    }
}