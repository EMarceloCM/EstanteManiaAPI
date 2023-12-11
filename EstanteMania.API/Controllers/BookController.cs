using AutoMapper;
using EstanteMania.API.DTO_s;
using EstanteMania.API.UnitOfWork.Interface;
using EstanteMania.API.Utils;
using EstanteMania.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EstanteMania.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BookController(IUnitOfWork uow, IMapper mapper) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookDTO>>> Pagination([FromQuery] QueryStringParameters parameters, [FromQuery] string? filter)
        {
            var books = await _uow.BookRepository.GetWithPagination(parameters, filter);

            var metadata = new
            {
                books.TotalCount,
                books.PageSize,
                books.CurrentPage,
                books.TotalPages,
                books.HasNext,
                books.HasPrevious
            };
            Response.Headers.Append("X_PAGINATION", JsonSerializer.Serialize(metadata));

            var booksDTO = _mapper.Map<List<BookDTO>>(books);

            if (booksDTO == null)
                return NotFound();

            var response = new PaginationBookResponseDTO
            {
                Books = booksDTO,
                TotalPages = books.TotalPages
            };

            return Ok(response);
        }

        [HttpGet("get-all-books")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAll()
        {
            var books = await _uow.BookRepository.GetAllAsync();

            if (books == null)
                return NotFound("There is not any book yet.");

            return Ok(_mapper.Map<IEnumerable<BookDTO>>(books));
        }

        [HttpGet("{id:int}", Name = "GetBook")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            var book = await _uow.BookRepository.GetByIdAsync(id);

            if (book == null)
                return NotFound($"This book with id = {id} does not exist.");

            return Ok(_mapper.Map<BookDTO>(book));
        }

        [HttpGet("search/{title}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookDTO>>> Search(string title)
        {
            var books = await _uow.BookRepository.SearchAsync(x => x.Title!.Contains(title));

            if (books == null || !books.Any())
                return NotFound("Any book was found with this sequence of caracteres.");

            return Ok(_mapper.Map<IEnumerable<BookDTO>>(books));
        }

        [HttpGet("get-books-by-category/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var books = await _uow.BookRepository.GetBooksByCategoryAsync(categoryId);

            if (!books.Any())
                return NotFound($"Any book was found in category number {categoryId}");

            return Ok(_mapper.Map<IEnumerable<BookDTO>>(books));
        }

        [HttpGet("/get-books-by-author/{authorID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByAuthor(int authorId)
        {
            var books = await _uow.BookRepository.SearchAsync(x => x.AuthorId == authorId);

            if (books == null || !books.Any())
                return NotFound($"Any book was found from author number {authorId}");

            return Ok(_mapper.Map<IEnumerable<BookDTO>>(books));
        }

        [HttpGet("/get-books-with-author/{bookId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookWithAuthorDTO>> GetWithAuthor(int bookId)
        {
            var bookWithAuthor = await _uow.BookRepository.FindBookWithAuthorByIdAsync(bookId);

            if (bookWithAuthor == null)
                return NotFound($"Any book was found with id = {bookId}");

            return _mapper.Map<BookWithAuthorDTO>(bookWithAuthor);
        }

        [HttpGet("get-books-with-categories/{searchString}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookWithCategoriesDTO>>> SearchBooksWithCategories(string searchString)
        {
            var booksWithCategoriesDTO = _mapper.Map<IEnumerable<BookWithCategoriesDTO>>(await _uow.BookRepository.FindBooksWithCategoriesAsync(searchString));

            if (!booksWithCategoriesDTO.Any())
                return NotFound("Any book was find with this sequence of caracters.");

            return Ok(booksWithCategoriesDTO);
        }

        [HttpGet("get-books-with-author/{searchString}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookWithAuthorDTO>>> SearchBooksWithAuthor(string searchString)
        {
            var bookWithAuthorDTO = _mapper.Map<IEnumerable<BookWithAuthorDTO>>(await _uow.BookRepository.FindBookWithAuthorAsync(searchString));

            if (!bookWithAuthorDTO.Any())
                return NotFound("Any book was find.");

            return Ok(bookWithAuthorDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(/*[FromQuery] int authorId, */[FromQuery] List<int> categoryIds, [FromBody] BookDTO bookDTO)
        {
            if (bookDTO == null || !ModelState.IsValid)
                return BadRequest();

            if(! await _uow.AuthorRepository.Exist(bookDTO.AuthorId))
                return NotFound("There is any authors with the given id.");

            //bookDTO.AuthorId = authorId;

            var categories = await _uow.CategoryRepository.GetAllByIdsAsync(categoryIds);
            if (categories == null || !categories.Any())
                return NotFound("There are no categories with the given ids.");

            bookDTO.Categories = categories.ToList();

            var newBook = _mapper.Map<Book>(bookDTO);

            await _uow.BookRepository.AddAsync(newBook);

            return CreatedAtRoute("GetBook", new { id = newBook.Id }, bookDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromQuery] List<int> categoryIds, /*[FromQuery] int authorId, */int id, [FromBody] BookDTO bookDTO)
        {
            if (id != bookDTO.Id || bookDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            if (! await _uow.BookRepository.Exist(id))
                return NotFound($"There is not a book with id = {id}.");

            if (! await _uow.AuthorRepository.Exist(bookDTO.AuthorId))
                return NotFound("There is any authors with the given id.");

            //bookDTO.AuthorId = authorId;

            var categories = await _uow.CategoryRepository.GetAllByIdsAsync(categoryIds);
            if (categories == null || !categories.Any())
                return NotFound("There are any categories with the given ids.");

            bookDTO.Categories = categories.ToList();

            var book = _mapper.Map<Book>(bookDTO);

            await _uow.BookRepository.UpdateBookAsync(book);
            return Ok(bookDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _uow.BookRepository.GetByIdAsync(id);

            if (book == null)
                return NotFound($"The book with id = {id} does not exist.");

            await _uow.BookRepository.DeleteAsync(id);
            return Ok(_mapper.Map<BookDTO>(book));
        }
    }
}
