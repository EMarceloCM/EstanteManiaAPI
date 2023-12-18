using EstanteMania.Email.Context;
using EstanteMania.Email.Messages;
using EstanteMania.Email.Models;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.Email.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<EmailDbContext> _context;

        public EmailRepository(DbContextOptions<EmailDbContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(UpdatePaymentResultMessage message)
        {
            EmailLog email = new()
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully!"
            };

            await using var db = new EmailDbContext(_context);
            db.Emails.Add(email);
            await db.SaveChangesAsync();
        }
    }
}
