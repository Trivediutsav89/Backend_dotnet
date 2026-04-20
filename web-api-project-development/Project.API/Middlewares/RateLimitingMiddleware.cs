using System.Collections.Concurrent;
using System.Net;

namespace Project.API.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxRequestsPerMinute;
        private static readonly ConcurrentDictionary<string, (DateTime WindowStart, int RequestCount)> _requestCounts = new();

        public RateLimitingMiddleware(RequestDelegate next, int maxRequestsPerMinute = 60)
        {
            _next = next;
            _maxRequestsPerMinute = maxRequestsPerMinute;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var currentTime = DateTime.UtcNow;

            var key = $"{clientIp}:{currentTime.ToString("yyyyMMddHHmm")}";

            var entry = _requestCounts.GetOrAdd(key, _ => (currentTime, 0));

            if (currentTime - entry.WindowStart > TimeSpan.FromMinutes(1))
            {
                entry = (currentTime, 0);
                _requestCounts[key] = entry;
            }

            if (entry.RequestCount >= _maxRequestsPerMinute)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                return;
            }

            _requestCounts[key] = (entry.WindowStart, entry.RequestCount + 1);

            await _next(context);
        }
    }
}
