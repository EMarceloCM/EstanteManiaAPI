namespace EstanteMania.API.Messages
{
    public class UpdatePaymentResultDTO
    {
        public long OrderId { get; set; }
        public int Status { get; set; }
        public string Email { get; set; } = null!;
    }
}
