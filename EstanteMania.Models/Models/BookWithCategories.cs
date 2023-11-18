namespace EstanteMania.Models.Models
{
    public class BookWithCategories
    {
        public Book? Book { get; set; }
        public IEnumerable<Category>? Categories { get; set;}
    }
}
