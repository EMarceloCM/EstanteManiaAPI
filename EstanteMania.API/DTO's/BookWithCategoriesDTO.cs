﻿namespace EstanteMania.API.DTO_s
{
    public class BookWithCategoriesDTO
    {
        public BookDTO? Book { get; set; }
        public IEnumerable<CategoryDTO>? Categories { get; set; }
    }
}
