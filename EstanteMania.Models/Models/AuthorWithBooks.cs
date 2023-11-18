namespace EstanteMania.Models.Models
{
    public class AuthorWithBooks
    {
        public Author? Author { get; set; }
        public IEnumerable<Book>? Books { get; set;}
    }
}