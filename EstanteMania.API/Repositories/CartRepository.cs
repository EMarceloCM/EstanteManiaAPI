﻿using EstanteMania.API.Context;
using EstanteMania.API.DTO_s;
using EstanteMania.API.Messages;
using EstanteMania.API.Repositories.Interfaces;
using EstanteMania.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarrinhoItem> GetItem(int id)
        {
            return await (from carrinho in _context.Carrinho
                          join carrinhoItem in _context.CarrinhoItens
                          on carrinho.Id equals carrinhoItem.CarrinhoId
                          where carrinho.Id == id
                          select new CarrinhoItem
                          {
                              Id = carrinhoItem.Id,
                              BookId = carrinhoItem.BookId,
                              CarrinhoId = carrinhoItem.CarrinhoId,
                              Quantidade = carrinhoItem.Quantidade
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CarrinhoItem>> GetItems(string userId)
        {
            return await (from carrinho in _context.Carrinho
                          join carrinhoItem in _context.CarrinhoItens
                          on carrinho.Id equals carrinhoItem.CarrinhoId
                          where carrinho.UserId == userId
                          select new CarrinhoItem
                          {
                              Id = carrinhoItem.Id,
                              BookId = carrinhoItem.BookId,
                              CarrinhoId = carrinhoItem.CarrinhoId,
                              Quantidade = carrinhoItem.Quantidade
                          }).ToListAsync();
        }

        public async Task<CarrinhoItem> AddItem(AddBookToCartDTO addToCartDTO, string userId)
        {
            if (await HasItemOnCart(addToCartDTO.CarrinhoId, addToCartDTO.BookId) == false)
            {
                var item = await (from book in _context.Book
                                  where book.Id == addToCartDTO.BookId
                                  select new CarrinhoItem
                                  {
                                      CarrinhoId = addToCartDTO.CarrinhoId,
                                      BookId = addToCartDTO.BookId,
                                      Quantidade = addToCartDTO.Quantidade
                                  }).SingleOrDefaultAsync();
                if (item != null)
                {
                    var cart = _context.Carrinho.FindAsync(addToCartDTO.CarrinhoId).Result;
                    if (cart == null)
                    {
                        await _context.Carrinho.AddAsync(new Carrinho { UserId =  userId});
                        await _context.SaveChangesAsync();
                    
                        item.CarrinhoId = await GetCartFromUser(userId);
                    }

                    var result = await _context.CarrinhoItens.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<CarrinhoItem> DeleteItem(int id)
        {
            var item = await _context.CarrinhoItens.FindAsync(id);
            if (item != null)
            {
                _context.CarrinhoItens.Remove(item);
                await _context.SaveChangesAsync();
            }

            return item;
        }

        public async Task<CarrinhoItem> UpdateQuantity(int id, UpdateBookQuantityOnCartDTO updateBookQuantityDTO)
        {
            var cartItem = await _context.CarrinhoItens.FindAsync(id);
            if (cartItem != null)
            {
                cartItem.Quantidade = updateBookQuantityDTO.Quantidade;
                await _context.SaveChangesAsync();
                return cartItem;
            }
            return null;
        }

        private async Task<bool> HasItemOnCart(int carrinhoId, int bookId)
        {
            return await _context.CarrinhoItens.AnyAsync(x => x.CarrinhoId == carrinhoId && x.BookId == bookId);
        }

        public async Task<int> GetCartFromUser(string userId)
        {
            var carrinho = await _context.Carrinho.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (carrinho == null)
            {
                var id = CreateCart(userId);
                return id.Result;
            }

            return carrinho.Id;
        }

        public async Task<int> CreateCart(string userId)
        {
            var newCart = await _context.Carrinho.AddAsync(new Carrinho { UserId = userId });
            await _context.SaveChangesAsync();

            return newCart.Entity.Id;
        }

        public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
        {
            var cart = await _context.Carrinho.Where(x => x.UserId == userId).AsNoTracking().FirstOrDefaultAsync();
            if (cart != null)
            {
                cart.CouponCode = couponCode;

                _context.Carrinho.Update(cart);
                _context.Entry(cart).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCouponAsync(string userId)
        {
            var cart = await _context.Carrinho.Where(x => x.UserId == userId).AsNoTracking().FirstOrDefaultAsync();
            if (cart != null)
            {
                cart.CouponCode = null;

                _context.Carrinho.Update(cart);
                _context.Entry(cart).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string?> GetCouponFromUserAsync(string userId)
        {
            return await _context.Carrinho.Where(x => x.UserId == userId).Select(x => x.CouponCode).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task UpdatePayment(int cartId, int status)
        {
            var cart = await _context.Carrinho.FindAsync(cartId);
            cart.Payment_Status = status;
            _context.Entry(cart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCartAsync(string userId)
        {
            var cart = await _context.Carrinho.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (cart != null)
            {
                _context.Carrinho.Remove(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}