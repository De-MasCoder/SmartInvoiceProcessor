using Polly;
using Polly.Retry;

namespace InvoiceProcessor.Infrastructure.Resilience
{
    public static class RetryPolicies
    {
        public static AsyncRetryPolicy GetRetryPolicy()
        {
            return Policy
                .Handle<TimeoutException>()
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(
                    retryCount: 3, // only retry 3 times
                    sleepDurationProvider: attempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, attempt)), // retry after 2s, 4s, 8s
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine(
                            $"Retry {retryCount} after {timeSpan.TotalSeconds}s due to {exception.Message}");
                    });
        }
    }
}
