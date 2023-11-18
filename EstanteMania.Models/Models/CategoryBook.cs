namespace EstanteMania.Models.Models
{
    public class CategoryBook
    {
        public CategoryBook()
        {
                
        }
        public CategoryBook(int categoryId, int bookId)
        {
            CategoryId = categoryId;
            BookId = bookId;
        }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public Book? Book { get; set; }
        public int BookId { get; set; }
    }
}