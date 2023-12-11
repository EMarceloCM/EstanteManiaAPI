using EstanteMania.API.DTO_s;
using EstanteMania.API.Identity_Entities;
using EstanteMania.API.Mappings;
using EstanteMania.API.UnitOfWork.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EstanteMania.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CartController(IUnitOfWork uow, ILogger<CartController> logger, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly ILogger<CartController> _logger = logger;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpGet("{userId}/GetItens")]
        public async Task<ActionResult<IEnumerable<CarrinhoItemDTO>>> GetItems(string userId)
        {
            try
            {
                var cartItems = await _uow.CartRepository.GetItems(userId);
                if (cartItems == null) return NoContent();

                var books = await _uow.BookRepository.GetAllAsync() ?? throw new Exception("There is any book yet...");

                var carrinhoItemDTO = cartItems.ConvertCarrinhoItemsToDTO(books);
                return Ok(carrinhoItemDTO);
            }
            catch(Exception ex)
            {
                logger.LogError("## Error while trying to get cart itens");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}", Name = "GetItem")]
        public async Task<ActionResult<CarrinhoItemDTO>> GetItem(int id)
        {
            try
            {
                var cartItem = await _uow.CartRepository.GetItem(id);
                if (cartItem == null) return NotFound();

                var books = await _uow.BookRepository.GetByIdAsync(cartItem.BookId);
                if (books == null) return NotFound();

                var carrinhoItemDTO = cartItem.ConvertCarrinhoItemToDTO(books);
                return Ok(carrinhoItemDTO);
            }
            catch (Exception ex)
            {
                logger.LogError("## Error while trying to get cart item");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<CarrinhoItemDTO>> PostItem([FromBody] AddBookToCartDTO cartItemDTO, string userId)
        {
            try
            {
                //var user = _userManager.GetUserId(User);

                var newCartItem = await _uow.CartRepository.AddItem(cartItemDTO, userId);
                if (newCartItem == null) return NoContent();

                var book = await _uow.BookRepository.GetByIdAsync(newCartItem.BookId);
                if (book == null) throw new Exception($"Book not found (Id: {cartItemDTO.BookId })");
                
                var newCartItemDTO = newCartItem.ConvertCarrinhoItemToDTO(book);
                //return CreatedAtAction("GetItem", new { id = newCartItemDTO.Id, newCartItemDTO});
                return Ok(newCartItemDTO);
            }
            catch (Exception ex)
            {
                logger.LogError("## Error while trying to create the new cart");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.StackTrace);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CarrinhoItemDTO>> DeleteItem(int id)
        {
            try
            {
                var cartItem = await _uow.CartRepository.DeleteItem(id);
                if (cartItem == null) return NotFound();

                var book = await _uow.BookRepository.GetByIdAsync(cartItem.BookId);
                if (book == null) return NotFound();

                var cartItemDTO = cartItem.ConvertCarrinhoItemToDTO(book);
                return Ok(cartItemDTO);
            }
            catch (Exception ex)
            {
                logger.LogError("## Error while trying to delete the item");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.StackTrace);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CarrinhoItemDTO>> UpdateQuantity(int id, UpdateBookQuantityOnCartDTO updateBookQuantityOnCartDTO)
        {
            try
            {
                var cartItem = await _uow.CartRepository.UpdateQuantity(id, updateBookQuantityOnCartDTO);
                if (cartItem == null)
                {
                    return NotFound();
                }

                var book = await _uow.BookRepository.GetByIdAsync(cartItem.BookId);
                var cartItemDTO = cartItem.ConvertCarrinhoItemToDTO(book);
                return Ok(cartItemDTO);
            }
            catch (Exception ex)
            {
                logger.LogError("## Error while trying to update the item");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.StackTrace);
            }
        }

        [HttpGet("get-cart-from-user/{userId}")]
        public async Task<ActionResult<int>> GetCartFromUser(string userId)
        {
            try
            {
                var cartId = await _uow.CartRepository.GetCartFromUser(userId);
                if (cartId == 0) return NotFound();

                return Ok(cartId);
            }
            catch (Exception ex)
            {
                logger.LogError("## Error while trying to get the cart");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.StackTrace);
            }
        }

        [HttpGet("create-cart/{userId}")]
        public async Task<ActionResult<int>> CreateCartForUser(string userId)
        {
            try
            {
                var newCartId = await _uow.CartRepository.CreateCart(userId);
                if (newCartId == 0) return BadRequest();

                return Ok(newCartId);
            }
            catch (Exception ex)
            {
                logger.LogError("## Error while trying to get the cart");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.StackTrace);
            }
        }
    }
}
