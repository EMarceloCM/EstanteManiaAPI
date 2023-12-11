using EstanteMania.API.Identity_Entities;
using EstanteMania.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.API.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Category> Category { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<CategoryBook> CategoryBook { get; set; }

        public DbSet<Carrinho> Carrinho { get; set; }
        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {

            #region Category
            mb.Entity<Category>().HasKey(x => x.Id);
            mb.Entity<Category>().Property(x => x.Name).IsRequired().HasMaxLength(80);
            mb.Entity<Category>().Property(x => x.Description).IsRequired().HasMaxLength(300);
            mb.Entity<Category>().Property(x => x.IconCSS).IsRequired().HasMaxLength(100);
            mb.Entity<Category>().Ignore(x => x.Books);
            mb.Entity<Category>().Ignore(x => x.CategorysBooks);
            #endregion

            #region Book
            mb.Entity<Book>().HasKey(x => x.Id);
            mb.Entity<Book>().Property(x => x.Title).IsRequired().HasMaxLength(100);
            mb.Entity<Book>().Property(x => x.Description).IsRequired().HasMaxLength(300);
            mb.Entity<Book>().Property(x => x.Illustrator).IsRequired().HasMaxLength(80);
            mb.Entity<Book>().Property(x => x.Publisher).IsRequired().HasMaxLength(80);
            mb.Entity<Book>().Property(x => x.Language).IsRequired().HasMaxLength(30);
            mb.Entity<Book>().Property(x => x.PageNumber).IsRequired();
            mb.Entity<Book>().Property(x => x.Condition).IsRequired().HasColumnType("nvarchar(10)");
            mb.Entity<Book>().Property(x => x.Availability).IsRequired().HasColumnType("nvarchar(20)");
            mb.Entity<Book>().Property(x => x.Type).IsRequired().HasColumnType("nvarchar(10)");
            mb.Entity<Book>().Property(x => x.Format).IsRequired().HasMaxLength(50);
            mb.Entity<Book>().Property(x => x.CoverImg).IsRequired();
            //mb.Entity<Book>().Property(x => x.ContentImg);
            mb.Entity<Book>().Property(x => x.Volume).IsRequired();
            mb.Entity<Book>().Property(x => x.Stock).IsRequired().HasPrecision(10, 2);
            mb.Entity<Book>().Property(x => x.Price).IsRequired().HasColumnType("decimal(12,2)");
            mb.Entity<Book>().Property(x => x.Discount).HasColumnType("decimal(12,2)");
            mb.Entity<Book>().Ignore(x => x.Author);
            mb.Entity<Book>().Property(x => x.AuthorId).IsRequired();
            mb.Entity<Book>().Ignore(x => x.Categories);
            mb.Entity<Book>().Ignore(x => x.CategorysBooks);
            #endregion

            #region Author
            mb.Entity<Author>().HasKey(x => x.Id);
            mb.Entity<Author>().Property(x => x.Name).IsRequired().HasMaxLength(80);
            mb.Entity<Author>().Property(x => x.Description).IsRequired().HasMaxLength(700);
            mb.Entity<Author>().Ignore(x => x.Books);
            #endregion

            #region CategoryBook
            mb.Entity<CategoryBook>().Ignore(x => x.Category);
            mb.Entity<CategoryBook>().Ignore(x => x.Book);
            #endregion

            #region Cart
            mb.Entity<Carrinho>().HasKey(x => x.Id);
            #endregion

            #region Relacionamentos e CategoryBooksId
            mb.Entity<Category>().HasMany(x => x.Books).WithMany(x => x.Categories)
                .UsingEntity<CategoryBook>(
                    x =>
                    {
                        x.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
                        x.HasOne<Book>().WithMany().HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Restrict);
                        x.HasKey(x => new {x.CategoryId, x.BookId});
                    }
                );
            mb.Entity<Author>().HasMany(x => x.Books).WithOne(x => x.Author).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);

            mb.Entity<Book>()
                .HasMany(b => b.CategorysBooks)
                .WithOne(cb => cb.Book)
                .HasForeignKey(cb => cb.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Category>()
                .HasMany(b => b.CategorysBooks)
                .WithOne(cb => cb.Category)
                .HasForeignKey(cb => cb.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Book>()
                .HasMany(b => b.Itens)
                .WithOne(cb => cb.Book)
                .HasForeignKey(cb => cb.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Carrinho>()
                .HasMany(b => b.Itens)
                .WithOne(cb => cb.Carrinho)
                .HasForeignKey(cb => cb.CarrinhoId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            mb.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(mb);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}