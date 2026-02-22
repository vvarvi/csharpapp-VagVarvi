using Xunit;
using FluentAssertions;
using System.Net;

namespace CSharpApp.Tests.Integration
{
    public class ProductsEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductsEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_Should_Return_200()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/getproducts");

            // Assert
            response.StatusCode
                .Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);

            content.Should().Contain("IntegrationTest");
        }
    }
}
