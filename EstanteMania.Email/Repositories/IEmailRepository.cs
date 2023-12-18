using EstanteMania.Email.Messages;

namespace EstanteMania.Email.Repositories
{
    public interface IEmailRepository
    {
        Task LogEmail(UpdatePaymentResultMessage message);
    }
}
