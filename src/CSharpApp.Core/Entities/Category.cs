
namespace CSharpApp.Core.Entities
{
    public sealed class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime? CreationAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
