using EstanteMania.API.Repositories.Interfaces;

namespace EstanteMania.API.UnitOfWork.Interface
{
    public interface IUnitOfWork :IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IBookRepository BookRepository { get; }
        ICartRepository CartRepository { get; }
        void Commit();
    }
}
