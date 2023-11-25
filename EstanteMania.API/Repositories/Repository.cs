using EstanteMania.API.Context;
using EstanteMania.API.Repositories.Interfaces;
using EstanteMania.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EstanteMania.API.Repositories
{
#nullable disable
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int? id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            DbSet.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int? id)
        {
            try
            {
                var entity = await DbSet.FindAsync(id);
                DbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                throw;
            }
        }

        public async Task<bool> Exist(int id)
        {
            if(await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) != null)
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
