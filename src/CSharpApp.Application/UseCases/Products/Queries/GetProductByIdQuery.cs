using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Products.Queries
{
    public sealed record GetProductByIdQuery(int Id) : IRequest<Product>;
}
