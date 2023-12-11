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
    public class AuthorController(IUnitOfWork uow, IMapper mapper) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AuthorDTO>>> Pagination([FromQuery] QueryStringParameters parameters, [FromQuery] string? filter)
        {
            var authors = await _uow.AuthorRepository.GetWithPagination(parameters, filter);

            var metadata = new
            {
                authors.TotalCount,
                authors.PageSize,
                authors.CurrentPage,
                authors.TotalPages,
                authors.HasNext,
                authors.HasPrevious
            };
            Response.Headers.Append("X_PAGINATION", JsonSerializer.Serialize(metadata));

            var authorsDTO = _mapper.Map<List<AuthorDTO>>(authors);

            if (authorsDTO == null)
                return NotFound();

            var response = new PaginationAuthorResponseDTO
            {
                Authors = authorsDTO,
                TotalPages = authors.TotalPages
            };

            return Ok(response);
        }

        [HttpGet("get-all-authors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAll()
        {
            var authors = await _uow.AuthorRepository.GetAllAsync();

            if (authors == null)
                return NotFound("There is not any author yet.");

            return Ok(_mapper.Map<IEnumerable<AuthorDTO>>(authors));
        }

        [HttpGet("{id:int}", Name = "GetAuthor")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorDTO>> GetById(int id)
        {
            var author = await _uow.AuthorRepository.GetByIdAsync(id);

            if (author == null)
                return NotFound("This author does not exist.");

            return Ok(_mapper.Map<AuthorDTO>(author));
        }

        [HttpGet("author-with-books/{authorId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorWithBooksDTO>> GetAuthorWithBooks(int authorId)
        {
            var authorWithBooks = await _uow.AuthorRepository.GetAuthorWithBooksAsync(authorId);

            if (authorWithBooks == null)
                return NotFound($"Cannot found any author with id = {authorId}");

            return _mapper.Map<AuthorWithBooksDTO>(authorWithBooks);
        }

        [HttpGet("search-author/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> Search(string name)
        {
            var authors = await _uow.AuthorRepository.SearchAsync(x => x.Name!.Contains(name));

            if (authors == null || !authors.Any())
                return NotFound("Any author was found with this sequence of caracters.");

            return Ok(_mapper.Map<IEnumerable<AuthorDTO>>(authors));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] AuthorDTO authorDTO)
        {
            if (authorDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _uow.AuthorRepository.Exists(authorDTO.Name))
            {
                ModelState.AddModelError("", "Author already exists");
                return StatusCode(422, ModelState);
            }

            var author = _mapper.Map<Author>(authorDTO);
            await _uow.AuthorRepository.AddAsync(author);

            return new CreatedAtRouteResult("GetAuthor", new { id = author.Id }, authorDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] AuthorDTO authorDTO)
        {
            if (id != authorDTO.Id || authorDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            if (! await _uow.AuthorRepository.Exist(id))
                return NotFound($"There is not an author with id = {id}.");

            var author = _mapper.Map<Author>(authorDTO);
            await _uow.AuthorRepository.UpdateAsync(author);

            return Ok(authorDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var author = await _uow.AuthorRepository.GetByIdAsync(id);

            if (author == null)
                return NotFound($"The author with id = {id} does not exist.");

            await _uow.AuthorRepository.DeleteAsync(id);
            return Ok(_mapper.Map<AuthorDTO>(author));
        }
    }
}