namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RestApiSettings>(configuration.GetSection(nameof(RestApiSettings)));

        services.Configure<HttpClientSettings>(configuration.GetSection(nameof(HttpClientSettings)));

        services.Configure<PerformanceSettings>(configuration.GetSection(nameof(PerformanceSettings)));

        //Validate PerformanceSettings -> SlowRequestThresholdMs
        services.AddOptions<PerformanceSettings>()
                .Bind(configuration.GetSection(nameof(PerformanceSettings)))
                .Validate(s => s.SlowRequestThresholdMs > 0, "SlowRequestThresholdMs must be greater than 0")
                .ValidateOnStart();

        return services;
    }
}