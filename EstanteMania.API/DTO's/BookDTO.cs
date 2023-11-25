﻿using EstanteMania.Models.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EstanteMania.API.DTO_s
{
    public class BookDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is required.")]
        [MinLength(3)]
        [MaxLength(50)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        [MinLength(5)]
        [MaxLength(200)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The illustrator is required.")]
        [MinLength(3)]
        [MaxLength(50)]
        public string? Illustrator { get; set; }

        [Required(ErrorMessage = "The publish date is required.")]
        [Display(Name = "Publish Date")]
        [DataType(DataType.Text)]
        //[DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly PublishDate { get; set; }

        [Required(ErrorMessage = "The publisher is required.")]
        [MinLength(3)]
        [MaxLength(50)]
        public string? Publisher { get; set; }

        [Required(ErrorMessage = "The book language is required.")]
        [MinLength(3)]
        [MaxLength(30)]
        public string? Language { get; set; }

        [Required(ErrorMessage = "The page number is required.")]
        [Display(Name = "Page Number")]
        [Range(1, 9999)]
        public int PageNumber { get; set; }

        [Required(ErrorMessage = "The condition is required.")]
        [AllowedValues("New", "VeryGood", "Good", "Old", "Vintage")]
        public string? Condition { get; set; } = Models.Models.Condition.New.ToString();

        [AllowedValues("Available", "InStock", "PreOrder", "OutOfStock", "Unavailable", "OnOrder", "Limited", "ComingSoon", "Discontinued")]
        public string? Availability { get; set; } = Models.Models.Availability.Available.ToString();

        [Required(ErrorMessage = "The type (book/manga) is required.")]
        [AllowedValues("Book", "Manga")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "The format is required.")]
        [MinLength(3)]
        [MaxLength(30)]
        public string? Format { get; set; }

        [Required(ErrorMessage = "The cover is required.")]
        [Display(Name = "Cover Image")]
        public string? CoverImg { get; set; }

        public string? ContentImg { get; set; }

        [Required(ErrorMessage = "The volume is required.")]
        [Range(1, 9999)]
        public int Volume { get; set; }

        [Required(ErrorMessage = "The price is required.")]
        [Column(TypeName = "decimal(10,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        public decimal Discount { get; set; } = 0.0m;

        [Required(ErrorMessage = "The stock is required.")]
        [Range(1, 9999)]
        public int Stock { get; set; } = 0;

        [JsonIgnore]
        public Author? Author { get; set; }

        [JsonIgnore]
        public int AuthorId { get; set; }

        [JsonIgnore]
        public ICollection<Category>? Categories { get; set; }

        [JsonIgnore]
        public ICollection<CategoryBook>? CategorysBooks { get; set; }
    }
}