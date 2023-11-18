using EstanteMania.API.Context;
using EstanteMania.API.Repositories;
using EstanteMania.API.Repositories.Interfaces;
using EstanteMania.API.UnitOfWork.Interface;

namespace EstanteMania.API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private CategoryRepository? _categoryRepository;
        private AuthorRepository? _authorRepository;
        private BookRepository? _bookRepository;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
             _context = context;
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new CategoryRepository(_context);
            }
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                return _authorRepository ??= new AuthorRepository(_context);
            }
        }

        public IBookRepository BookRepository
        {
            get
            {
                return _bookRepository ??= new BookRepository(_context);
            }
        }

        public async void Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}