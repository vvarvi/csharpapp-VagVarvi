using CSharpApp.Application.Abstractions;
using CSharpApp.Application.UseCases.Products;
using CSharpApp.Infrastructure.Auth;
using CSharpApp.Infrastructure.Http;
using CSharpApp.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HttpClientSettings>(configuration.GetSection(nameof(HttpClientSettings)));
        
        // Auth client (separate client)
        services.AddHttpClient<IAuthService, AuthService>((provider, client) =>
        {
            var settings = provider
                .GetRequiredService<IOptions<RestApiSettings>>()
                .Value;

            client.BaseAddress = new Uri(settings.BaseUrl!);
        });

        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddTransient<AuthDelegatingHandler>();

        // Products client with JWT handler
        services.AddHttpClient<IProductsService, ProductsService>((provider, client) =>
        {
            var settings = provider
                .GetRequiredService<IOptions<RestApiSettings>>()
                .Value;

            client.BaseAddress = new Uri(settings.BaseUrl!);
        })
        .AddHttpMessageHandler<LoggingDelegatingHandler>()
        .AddHttpMessageHandler<AuthDelegatingHandler>()
        .SetHandlerLifetime(GetHandlerLifetime(configuration))
        .AddPolicyHandler(GetRetryPolicy(configuration));

        services.AddHttpClient<ICategoriesService, CategoriesService>((provider, client) =>
        {
            var settings = provider
                .GetRequiredService<IOptions<RestApiSettings>>()
                .Value;

            client.BaseAddress = new Uri(settings.BaseUrl!);
        })
        .AddHttpMessageHandler<LoggingDelegatingHandler>()
        .AddHttpMessageHandler<AuthDelegatingHandler>()
        .SetHandlerLifetime(GetHandlerLifetime(configuration))
        .AddPolicyHandler(GetRetryPolicy(configuration));

        services.AddTransient<LoggingDelegatingHandler>();

        return services;
    }

    private static TimeSpan GetHandlerLifetime(IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(HttpClientSettings))
            .Get<HttpClientSettings>();

        return TimeSpan.FromMinutes(settings!.LifeTime);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(HttpClientSettings))
            .Get<HttpClientSettings>();

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                settings!.RetryCount,
                retryAttempt =>
                    TimeSpan.FromSeconds(settings.SleepDuration * retryAttempt));
    }
}