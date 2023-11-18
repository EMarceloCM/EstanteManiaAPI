using AutoMapper;
using EstanteMania.API.DTO_s;
using EstanteMania.API.UnitOfWork.Interface;
using EstanteMania.API.Utils;
using EstanteMania.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EstanteMania.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IUnitOfWork uow, IMapper mapper) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CategoryDTO>>> Pagination([FromQuery] QueryStringParameters parameters, [FromQuery] string? filter)
        {
            var categories = await _uow.CategoryRepository.GetWithPagination(parameters, filter);

            var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.TotalPages,
                categories.HasNext,
                categories.HasPrevious
            };
            Response.Headers.Append("X_PAGINATION", JsonSerializer.Serialize(metadata));

            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);

            if (categoriesDTO == null)
                return NotFound();

            return Ok(categoriesDTO);
        }

        [HttpGet("/get-all-categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var categorires = await _uow.CategoryRepository.GetAllAsync();

            if (categorires == null)
                return NotFound("There is not any category yet.");

            var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categorires);
            return Ok(categoriesDTO);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await _uow.CategoryRepository.GetByIdAsync(id);

            if (category == null)
                return NotFound("This category does not exist.");

            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpGet("/category-with-books/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryWithBookDTO>> GetCategoryWithBooks(int categoryId)
        {
            var categoryWithBooks = await _uow.CategoryRepository.GetCategoryWithBooksAsync(categoryId);

            if (categoryWithBooks == null)
                return NotFound($"Cannot found any category with id = {categoryId}");

            return _mapper.Map<CategoryWithBookDTO>(categoryWithBooks); ;
        }

        [HttpGet("/search-category/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Search(string name)
        {
            var categories = await _uow.CategoryRepository.SearchAsync(x => x.Name!.Contains(name));

            if (categories == null || !categories.Any())
                return NotFound("Any category was found with this sequence of caracters.");

            return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(categories));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _uow.CategoryRepository.Exists(categoryDTO.Name))
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            var newCategory = _mapper.Map<Category>(categoryDTO);

            await _uow.CategoryRepository.AddAsync(newCategory);
            return new CreatedAtRouteResult("GetCategory", new { id = newCategory.Id }, categoryDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.Id || categoryDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            if (! await _uow.CategoryRepository.Exist(id))
                return NotFound($"There is not a category with id = {id}.");

            var category = _mapper.Map<Category>(categoryDTO);

            await _uow.CategoryRepository.UpdateAsync(category);
            return Ok(categoryDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _uow.CategoryRepository.GetByIdAsync(id);

            if (category == null)
                return NotFound($"The category with id = {id} does not exist.");

            await _uow.CategoryRepository.DeleteAsync(id);
            return Ok(_mapper.Map<CategoryDTO>(category));
        }
    }
}