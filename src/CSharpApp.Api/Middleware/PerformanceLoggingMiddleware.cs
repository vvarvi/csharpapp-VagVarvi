using CSharpApp.Core.Settings;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CSharpApp.Api.Middleware
{
    public sealed class PerformanceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceLoggingMiddleware> _logger;

        private readonly PerformanceSettings _settings;

        public PerformanceLoggingMiddleware(RequestDelegate next, ILogger<PerformanceLoggingMiddleware> logger, IOptions<PerformanceSettings> settings)
        {
            _next = next;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);

                stopwatch.Stop();

                var elapsed = stopwatch.ElapsedMilliseconds;

                if (elapsed > _settings.SlowRequestThresholdMs)
                {
                    _logger.LogWarning(
                        "SLOW REQUEST: {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        elapsed);
                }
                else
                {
                    _logger.LogDebug(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    elapsed);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                 ex,
                 "FAILED REQUEST: {Method} {Path} after {ElapsedMilliseconds} ms",
                 context.Request.Method,
                 context.Request.Path,
                 stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}
