namespace EstanteMania.Models.Models
{
    public class Carrinho
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ICollection<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
        public string? CouponCode { get; set; }
    }
}