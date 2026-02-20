
namespace CSharpApp.Core.Commands.DTO
{
    public sealed class CreateProductRequest
    {
        public string Title { get; set; } = default!;
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
        public int CategoryId { get; set; }
        public List<string> Images { get; set; } = new();
    }
}
