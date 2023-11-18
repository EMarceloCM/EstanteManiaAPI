using EstanteMania.API.Context;
using EstanteMania.API.Repositories.Interfaces;
using EstanteMania.API.Resources;
using EstanteMania.API.Utils;
using EstanteMania.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.API.Repositories
{
#nullable disable
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base (context) {}

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _context.CategoryBook.Where(x => x.CategoryId == categoryId).Select(x => x.Book).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BookWithAuthor>> FindBookWithAuthorAsync(string criterio)
        {
            var authorWithBooks = await _context.Book.Where(x => x.Title!.Contains(criterio) || x.Description!.Contains(criterio) || x.Author!.Name!.Contains(criterio))
                .Select(x => new BookWithAuthor
                {
                    Book = x,
                    Author = x.Author
                })
                .AsNoTracking().ToListAsync();
            return authorWithBooks;
        }

        public async Task<IEnumerable<BookWithCategories>> FindBooksWithCategoriesAsync(string criterio)
        {
            var bookWithCategories = await _context.Book.Where(x => x.Title!.Contains(criterio) || x.Description!.Contains(criterio) || x.Author.Name!.Contains(criterio))
                .Select(x => new BookWithCategories
                {
                    Book = x,
                    Categories = x.CategorysBooks.Select(x => x.Category)
                })
                .AsNoTracking().ToListAsync();

            return bookWithCategories;
        }

        public IQueryable<Book> GetBooksQueryable()
        {
            return _context.Book.AsNoTracking().AsQueryable();
        }

        public async Task UpdateBookAsync(Book book)
        {
            await DeleteRelationship(book);

            _context.Entry(book).State = EntityState.Detached;
            _context.Update(book);
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private async Task DeleteRelationship(Book book)
        {
            var existingBook = await _context.Book
                .Include(b => b.CategorysBooks).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == book.Id);

            if (existingBook != null)
            {
                var removedRelations = existingBook.CategorysBooks
                    .Where(existingRelation => !book.CategorysBooks.Any(newRelation => newRelation.CategoryId == existingRelation.CategoryId))
                    .ToList();

                _context.CategoryBook.RemoveRange(removedRelations);
                await _context.SaveChangesAsync();
            }

            //instancia do que está no banco não do que vai ser atualizado
            //limpa-la para não dar erro
            _context.Entry(existingBook).State = EntityState.Detached;
        }

        public async Task<PageList<Book>> GetWithPagination(QueryStringParameters parameters, string filter = null)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return await PageList<Book>.ToPagedList(GetBooksQueryable(), parameters.PageNumber, parameters.PageSize);

            return await PageList<Book>.ToPagedList(GetBooksQueryable().Where(x => x.Title.Contains(filter)), parameters.PageNumber, parameters.PageSize);
        }
    }
}