using CSharpApp.Core.Entities;
using MediatR;

namespace CSharpApp.Application.UseCases.Products.Commands
{
    public sealed record CreateProductCommand(
                                                string Title,
                                                decimal Price,
                                                string Description,
                                                int CategoryId,
                                                List<string> Images)
    : IRequest<Product>;
}
