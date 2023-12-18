using EstanteMania.Email.Models;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.Email.Context
{
    public class EmailDbContext : DbContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
        {}
    
        public DbSet<EmailLog> Emails { get; set; }
    }
}
