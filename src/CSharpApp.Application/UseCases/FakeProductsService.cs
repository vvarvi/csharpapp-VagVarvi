using CSharpApp.Core.Commands.DTO;
using CSharpApp.Core.Common;
using CSharpApp.Core.Entities;

namespace CSharpApp.Application.UseCases
{
    public class FakeProductsService : IProductsService
    {
        public Task<Result<IReadOnlyCollection<Product>>> GetProducts(CancellationToken cancellationToken)
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Title = "IntegrationTest" }
            };

            return Task.FromResult(Result<IReadOnlyCollection<Product>>.Success(products.AsReadOnly()));
        }

        public Task<Product> GetProductById(int id)
        {
            return Task.FromResult(
                new Product { Id = id, Title = "IntegrationTest" });
        }

        public Task<Product> CreateProduct(CreateProductRequest request)
        {
            return Task.FromResult(
                new Product { Id = 99, Title = request.Title });
        }
    }
}
