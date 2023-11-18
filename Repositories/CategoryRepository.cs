using EstanteMania.API.Context;
using EstanteMania.API.Repositories.Interfaces;
using EstanteMania.API.Resources;
using EstanteMania.API.Utils;
using EstanteMania.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.API.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<CategoryWithBook?> GetCategoryWithBooksAsync(int categoryId)
        {
            var categoryWBooks = await _context.Category.Where(x => x.Id == categoryId)
                .Select(x => new CategoryWithBook
                {
                    Category = x,
                    Books = x.CategorysBooks!.Select(x => x.Book)
                }).AsNoTracking().FirstOrDefaultAsync();
            return categoryWBooks;
        }
        
        public async Task<IEnumerable<Category>> GetAllByIdsAsync(List<int> ids)
        {
            List<Category> categories = [];

            foreach (int id in ids)
            {
                var category = await _context.Category.FindAsync(id);
                if (category != null)
                {
                    categories.Add(category);
                }
            }

            return categories;
        }

        public async Task<bool> Exists(string? categoryName = null)
        {
            if (categoryName == null || string.IsNullOrWhiteSpace(categoryName))
                return false;

            var category = await _context.Category.AsNoTracking().ToListAsync();
            if (category.FirstOrDefault(x => x.Name!.Trim().ToLower() == categoryName.TrimEnd().ToLower()) != null)
                return true;
            return false;
        }

        public async Task<PageList<Category>> GetWithPagination(QueryStringParameters parameters, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return await PageList<Category>.ToPagedList(_context.Category.AsNoTracking().AsQueryable(), parameters.PageNumber, parameters.PageSize);

            return await PageList<Category>.ToPagedList(_context.Category.Where(x => x.Name!.Contains(filter)).AsNoTracking().AsQueryable(), parameters.PageNumber, parameters.PageSize);
        }
    }
}