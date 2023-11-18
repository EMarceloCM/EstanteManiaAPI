using EstanteMania.API.Context;
using EstanteMania.API.Repositories.Interfaces;
using EstanteMania.API.Resources;
using EstanteMania.API.Utils;
using EstanteMania.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.API.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext context) : base(context) {}

        public async Task<bool> Exists(string? authorName = null)
        {
            if (authorName == null || string.IsNullOrWhiteSpace(authorName))
                return false;

            var category = await _context.Category.AsNoTracking().ToListAsync();
            if (category.FirstOrDefault(x => x.Name!.Trim().Equals(authorName.TrimEnd(), StringComparison.CurrentCultureIgnoreCase)) != null)
                return true;
            return false;
        }

        public async Task<AuthorWithBooks?> GetAuthorWithBooksAsync(int authorId)
        {
            var authorWithBooks = await _context.Author.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == authorId);
            
            if (authorWithBooks == null) return null;
            if (authorWithBooks.Books!.Count == 0 || authorWithBooks.Books == null)
                return new AuthorWithBooks { Author = authorWithBooks, Books = null };

            foreach (var book in authorWithBooks.Books)
            {
                book.Author = null;
            }

            return new AuthorWithBooks { Author = authorWithBooks, Books = authorWithBooks.Books };
        }

        public async Task<PageList<Author>> GetWithPagination(QueryStringParameters parameters, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return await PageList<Author>.ToPagedList(_context.Author.AsNoTracking().AsQueryable(), parameters.PageNumber, parameters.PageSize);

            return await PageList<Author>.ToPagedList(_context.Author.Where(x => x.Name!.Contains(filter)).AsNoTracking().AsQueryable(), parameters.PageNumber, parameters.PageSize);
        }
    }
}