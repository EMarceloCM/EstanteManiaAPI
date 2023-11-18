namespace EstanteMania.Models.Models
{
    public class Category : Entity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? IconCSS { get; set; }
        public ICollection<Book>? Books { get; set; } = new List<Book>();
        public ICollection<CategoryBook>? CategorysBooks { get; } = new List<CategoryBook>();
    }
}