namespace EstanteMania.API.DTO_s
{
    public class CarrinhoItemDTO
    {
        public int Id { get; set; }
        public int CarrinhoId { get; set; }
        public int BookId { get; set; }
        public int Quantidade { get; set; }

        public string? BookName { get; set; }
        public string? BookDescricao { get; set; }
        public string? BookCoverImage { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
