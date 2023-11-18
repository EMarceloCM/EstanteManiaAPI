namespace EstanteMania.Models.Models
{
    public class Book : Entity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Illustrator { get; set; }
        public DateOnly PublishDate { get; set; }
        public string? Publisher { get; set; }
        public string? Language { get; set; }
        public int PageNumber { get; set; }
        public string? Condition { get; set; }
        public string? Availability { get; set; }
        public string? Type { get; set; }
        public string? Format { get; set; }
        public string? CoverImg { get; set; }
        public string? ContentImg { get; set; }
        public int Volume { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; } = 0.0m;
        public int Stock { get; set; }

        public Author? Author { get; set; }
        public int AuthorId { get; set; }
        public ICollection<Category>? Categories { get; set; } = new List<Category>();
        public ICollection<CategoryBook>? CategorysBooks { get; set; } = new List<CategoryBook>();
    }
}