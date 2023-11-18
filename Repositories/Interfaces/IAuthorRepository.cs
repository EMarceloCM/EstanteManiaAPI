using EstanteMania.API.Resources;
using EstanteMania.API.Utils;
using EstanteMania.Models.Models;

namespace EstanteMania.API.Repositories.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<AuthorWithBooks?> GetAuthorWithBooksAsync(int authorId);
        Task<PageList<Author>> GetWithPagination(QueryStringParameters parameters, string filter);
        Task<bool> Exists(string? authorName = null);
    }
}