namespace EstanteMania.Models.Models
{
    public class Author : Entity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}