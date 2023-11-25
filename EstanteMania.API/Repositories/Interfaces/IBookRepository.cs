using EstanteMania.API.Resources;
using EstanteMania.API.Utils;
using EstanteMania.Models.Models;

namespace EstanteMania.API.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task <IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<PageList<Book>> GetWithPagination(QueryStringParameters parameters, string filter);
        Task<IEnumerable<BookWithCategories>> FindBooksWithCategoriesAsync(string criterio);
        Task<IEnumerable<BookWithAuthor>> FindBookWithAuthorAsync(string criterio);
        Task<BookWithAuthor> FindBookWithAuthorByIdAsync(int bookId);
        Task UpdateBookAsync(Book book);
        IQueryable<Book> GetBooksQueryable();
    }
}