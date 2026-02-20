using CSharpApp.Core.Commands.DTO;
using CSharpApp.Core.Common;
using CSharpApp.Core.Entities;

namespace CSharpApp.Core.Interfaces
{
    public interface IProductsService
    {
        Task<Result<IReadOnlyCollection<Product>>> GetProducts(CancellationToken cancellationToken);

        Task<Product> GetProductById(int id);

        Task<Product> CreateProduct(CreateProductRequest request);
    }
}