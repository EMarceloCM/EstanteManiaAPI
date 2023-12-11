namespace EstanteMania.Models.Models
{
    public class CarrinhoItem
    {
        public int Id { get; set; }
        public int CarrinhoId { get; set; }
        public int BookId { get; set; }
        public int Quantidade { get; set; }
        public Carrinho? Carrinho { get; set; }
        public Book? Book { get; set; }
    }
}