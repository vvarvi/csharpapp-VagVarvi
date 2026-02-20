using CSharpApp.Core.Common;
using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Products.Queries
{
    public sealed record GetProductsQuery() : IRequest<Result<IReadOnlyCollection<Product>>>;
}
