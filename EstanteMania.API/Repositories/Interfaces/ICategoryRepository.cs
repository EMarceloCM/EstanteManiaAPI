using EstanteMania.API.Resources;
using EstanteMania.API.Utils;
using EstanteMania.Models.Models;

namespace EstanteMania.API.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<CategoryWithBook?> GetCategoryWithBooksAsync(int categoryId);
        Task<List<CategoryWithBook>?> GetAllCategoriesWithBooksAsync();
        Task<IEnumerable<Category>> GetAllByIdsAsync(List<int> ids);
        Task<PageList<Category>> GetWithPagination(QueryStringParameters parameters, string filter);
        Task<bool> Exists(string? categoryName = null);
    }
}