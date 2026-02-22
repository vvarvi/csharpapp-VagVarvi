using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using FluentAssertions;
using Microsoft.Extensions.Options;
using CSharpApp.Infrastructure.Auth;
using CSharpApp.Core.Settings;
using CSharpApp.Core.Common;

namespace CSharpApp.Tests.Unit.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task GetAccessTokenAsync_Should_Return_Token()
        {
            // Arrange
            var tokenResponse = new AuthTokenResponse
            {
                AccessToken = "fake-token"
            };

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(tokenResponse)
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://api.escuelajs.co/api/v1/")
            };

            var settings = Options.Create(new RestApiSettings
            {
                Auth = "auth/login",
                Username = "john@mail.com",
                Password = "changeme"
            });

            var service = new AuthService(httpClient, settings);

            // Act
            Result<AuthTokenResponse> result = await service.LoginAsync();

            if (result.Value == null || !result.IsSuccess)
            {
                throw new Exception("Result is null");
            }

            // Assert
            result.Value.AccessToken.Should().Be("fake-token");
        }
    }
}
