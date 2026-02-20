using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Categories.Queries
{
    public sealed record GetCategoryByIdQuery(int Id) : IRequest<Category>;
}
