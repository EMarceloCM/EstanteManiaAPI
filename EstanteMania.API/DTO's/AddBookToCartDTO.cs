using System.ComponentModel.DataAnnotations;

namespace EstanteMania.API.DTO_s
{
    public class AddBookToCartDTO
    {
        [Required]
        public int CarrinhoId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int Quantidade { get; set; }
    }
}
