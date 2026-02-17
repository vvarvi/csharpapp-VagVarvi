using CSharpApp.Infrastructure.Security.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<CacheTTLOptions>(configuration.GetSection("CacheTtlPolicy"));

            services.AddMemoryCache();

            //services.AddScoped<ICacheTtlPolicy, ConfigurableCacheTtlPolicy>();

            //services.AddHttpClients();

            //services.AddExternalApiClients();

            //services.AddSingleton<IMetricsLogger, MetricsLogger>();

            //services.AddOpenTelemetryTracing();

            //services.AddCorrelationLogging();

            services.AddJwtSecurity(configuration);

            //services.AddSingleton<IApiPerformanceTracker, InMemoryApiPerformanceTracker>();

            //services.AddSingleton<PerformanceAnalyzerHostedService>();

            //services.AddHostedService<PerformanceLoggingHostedService>();

            return services;
        }
    }
}
