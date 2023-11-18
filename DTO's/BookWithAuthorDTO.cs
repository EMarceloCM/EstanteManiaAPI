using EstanteMania.Models.Models;

namespace EstanteMania.API.DTO_s
{
    public class BookWithAuthorDTO
    {
        public BookDTO? Book { get; set; }
        public AuthorDTO? Author { get; set; }
    }
}
