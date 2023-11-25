namespace EstanteMania.API.DTO_s
{
    public class CategoryWithBookDTO
    {
        public CategoryDTO? Category { get; set; }
        public IEnumerable<BookDTO>? Books { get; set; }
    }
}