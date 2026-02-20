using CSharpApp.Application.UseCases.Products.Queries;
using CSharpApp.Core.Entities;
using CSharpApp.Core.Interfaces;
using Xunit;
using Moq;
using FluentAssertions;

namespace CSharpApp.Tests.Unit.Handlers
{
    public class GetProductsHandlerTests
    {
        private readonly Mock<IProductsService> _productsServiceMock;
        private readonly GetProductsHandler _handler;

        public GetProductsHandlerTests()
        {
            _productsServiceMock = new Mock<IProductsService>();
            _handler = new GetProductsHandler(_productsServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_List_Of_Products()
        {
            CancellationToken cancellationToken = new CancellationToken();

            // Arrange
            var products = new List<Product>
        {
            new Product { Id = 1, Title = "Test" }
        }.AsReadOnly();

            //_productsServiceMock
            //   .Setup(x => x.GetProducts(cancellationToken))
            //    .ReturnsAsync(products);

            var query = new GetProductsQuery();

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            result.Should().BeEquivalentTo(products);
            _productsServiceMock.Verify(x => x.GetProducts(cancellationToken), Times.Once);
        }
    }
}
