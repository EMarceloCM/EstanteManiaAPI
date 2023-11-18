using EstanteMania.Models.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EstanteMania.API.DTO_s
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The category name is required.")]
        [MinLength(3)]
        [MaxLength(80)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The category description is required.")]
        [MinLength(5)]
        [MaxLength(100)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The icon's name is required.")]
        [MinLength(3)]
        [MaxLength(100)]
        public string? IconCSS { get; set; }

        [JsonIgnore]
        public ICollection<Book>? Books { get; set; }

        [JsonIgnore]
        public ICollection<CategoryBook>? CategorysBooks { get; set; }
        public List<int>? BooksIds { get; set; }
    }
}