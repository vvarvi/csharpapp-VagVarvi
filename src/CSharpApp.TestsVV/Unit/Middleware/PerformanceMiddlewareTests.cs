using CSharpApp.Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using CSharpApp.Api.Middleware;

namespace CSharpApp.Tests.Unit.Middleware
{
    public class PerformanceMiddlewareTests
    {
        [Fact]
        public async Task Middleware_Should_Invoke_Next()
        {
            // Arrange
            var context = new DefaultHttpContext();

            var loggerMock = new Mock<ILogger<PerformanceLoggingMiddleware>>();

            var options = Options.Create(new PerformanceSettings
            {
                SlowRequestThresholdMs = 1
            });

            var middleware = new PerformanceLoggingMiddleware(
                async (ctx) =>
                {
                    await Task.Delay(10);
                    ctx.Response.StatusCode = 200;
                },
                loggerMock.Object,
                options);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }
    }
}
