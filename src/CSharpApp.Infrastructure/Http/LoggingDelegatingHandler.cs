using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CSharpApp.Infrastructure.Http
{
    public class LoggingDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingDelegatingHandler> _logger;

        public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Outgoing HTTP {Method} {Url}",
                request.Method,
                request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);

            stopwatch.Stop();

            _logger.LogInformation("Completed HTTP {Method} {Url} in {ElapsedMs}ms with status {StatusCode}",
                request.Method,
                request.RequestUri,
                stopwatch.ElapsedMilliseconds,
                (int)response.StatusCode);

            return response;
        }
    }
}
