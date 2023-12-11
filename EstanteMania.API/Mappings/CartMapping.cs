using EstanteMania.API.DTO_s;
using EstanteMania.Models.Models;

namespace EstanteMania.API.Mappings
{
    public static class CartMapping
    {
        public static IEnumerable<CarrinhoItemDTO> ConvertCarrinhoItemsToDTO(this IEnumerable<CarrinhoItem> cartItems, IEnumerable<Book> books)
        {
            return (from cartItem in cartItems
                    join book in books
                    on cartItem.BookId equals book.Id
                    select new CarrinhoItemDTO
                    {
                        Id = cartItem.Id,
                        BookId = cartItem.BookId,
                        BookName = book.Title,
                        BookDescricao = book.Description,
                        BookCoverImage = book.CoverImg,
                        Price = book.Price,
                        CarrinhoId = cartItem.CarrinhoId,
                        Quantidade = cartItem.Quantidade,
                        TotalPrice = book.Price * cartItem.Quantidade
                    }).ToList();
        }

        public static CarrinhoItemDTO ConvertCarrinhoItemToDTO(this CarrinhoItem cartItem, Book book)
        {
            return new CarrinhoItemDTO
            {
                Id = cartItem.Id,
                BookId = cartItem.BookId,
                BookName = book.Title,
                BookDescricao = book.Description,
                BookCoverImage = book.CoverImg,
                Price = book.Price,
                CarrinhoId = cartItem.CarrinhoId,
                Quantidade = cartItem.Quantidade,
                TotalPrice = book.Price * cartItem.Quantidade
            };
        }
    }
}