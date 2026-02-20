using CSharpApp.Application.Abstractions;
using CSharpApp.Core.Commands.DTO;
using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Categories.Commands
{
    public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
    {
        private readonly ICategoriesService _categoriesService;

        public CreateCategoryHandler(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var createRequest = new CreateCategoryRequest
            {
                Id = request.Id,
                Name = request.Name,
                Image = request.Image,
                CreatedAt = request.CreationAt,
                UpdatedAt = request.UpdatedAt
            };

            return await _categoriesService.CreateCategory(createRequest);
        }
    }

}
