using CSharpApp.Core.Common;
using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Products.Queries
{
    public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, Result<IReadOnlyCollection<Product>>>
    {
        private readonly IProductsService _productsService;

        public GetProductsHandler(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<Result<IReadOnlyCollection<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productsService.GetProducts(cancellationToken);
        }
    }
}
