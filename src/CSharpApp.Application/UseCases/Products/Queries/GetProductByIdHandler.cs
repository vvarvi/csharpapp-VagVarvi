using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Products.Queries
{
    public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductsService _productsService;

        public GetProductByIdHandler(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productsService
                .GetProductById(request.Id);
        }
    }

}
