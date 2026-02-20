using CSharpApp.Core.Commands.DTO;
using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Products.Commands
{
    public sealed class CreateCategoryHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductsService _productsService;

        public CreateCategoryHandler(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var createRequest = new CreateProductRequest
            {
                Title = request.Title,
                Price = request.Price,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Images = request.Images
            };

            return await _productsService.CreateProduct(createRequest);
        }
    }

}
