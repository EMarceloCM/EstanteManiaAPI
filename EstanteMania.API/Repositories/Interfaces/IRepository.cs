using EstanteMania.Models.Models;
using System.Linq.Expressions;

namespace EstanteMania.API.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int? id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int? id);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
        Task<bool> Exist(int id);
    }
}