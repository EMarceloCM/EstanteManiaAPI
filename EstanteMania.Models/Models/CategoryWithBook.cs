namespace EstanteMania.Models.Models
{
    public class CategoryWithBook
    {
        public Category? Category { get; set; }
        public IEnumerable<Book>? Books { get; set; }
    }
}