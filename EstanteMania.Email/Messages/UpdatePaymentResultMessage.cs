namespace EstanteMania.Email.Messages
{
    public class UpdatePaymentResultMessage
    {
        public long OrderId { get; set; }
        public int Status { get; set; }
        public string Email { get; set; } = null!;
    }
}
