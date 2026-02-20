using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpApp.Infrastructure.Health
{
    public class ExternalApiHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;

        private readonly RestApiSettings _settings;

        public ExternalApiHealthCheck(IHttpClientFactory factory, IOptions<RestApiSettings> settings)
        {
            _httpClient = factory.CreateClient();
            _settings = settings.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                _httpClient.BaseAddress = new Uri(_settings.BaseUrl!);

                var response = await _httpClient.GetAsync(_settings.Products, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("External API is reachable.");
                }

                return HealthCheckResult.Unhealthy($"External API returned {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("External API unreachable.", ex);
            }
        }
    }
}
