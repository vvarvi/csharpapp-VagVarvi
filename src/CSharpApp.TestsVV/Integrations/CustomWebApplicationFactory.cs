using CSharpApp.Application.UseCases;
using CSharpApp.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CSharpApp.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove real services
                services.RemoveAll<IProductsService>();

                // Add fake service
                services.AddScoped<IProductsService, FakeProductsService>();
            });
        }
    }
}
