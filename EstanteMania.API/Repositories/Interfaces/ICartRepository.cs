﻿using EstanteMania.API.DTO_s;
using EstanteMania.Models.Models;

namespace EstanteMania.API.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<CarrinhoItem> AddItem(AddBookToCartDTO addToCartDTO, string userId);
        Task<CarrinhoItem> UpdateQuantity(int id, UpdateBookQuantityOnCartDTO updateBookQuantityDTO);
        Task<CarrinhoItem> DeleteItem(int id);
        Task<CarrinhoItem> GetItem(int id);
        Task<IEnumerable<CarrinhoItem>> GetItems(string userId);
        Task<int> GetCartFromUser(string userId);
        Task<int> CreateCart(string userId);
    }
}