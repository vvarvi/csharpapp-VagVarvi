using CSharpApp.Core.Commands.DTO;
using CSharpApp.Core.Entities;

namespace CSharpApp.Application.Abstractions
{
    public interface ICategoriesService
    {
        Task<IReadOnlyCollection<Category>> GetCategories(CancellationToken cancellationToken);

        Task<Category> GetCategoryById(int id);

        Task<Category> CreateCategory(CreateCategoryRequest request);
    }
}
