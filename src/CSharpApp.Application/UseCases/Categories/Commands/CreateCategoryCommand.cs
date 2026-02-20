using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Categories.Commands
{
    public sealed record CreateCategoryCommand(
                                                int Id,
                                                string Name,
                                                string Image,
                                                DateTime CreationAt,
                                                DateTime UpdatedAt)
    : IRequest<Category>;
}
