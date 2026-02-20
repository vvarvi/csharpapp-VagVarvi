
namespace CSharpApp.Core.Commands.DTO
{
    public sealed class CreateCategoryRequest
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
