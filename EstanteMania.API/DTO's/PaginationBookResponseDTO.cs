﻿namespace EstanteMania.API.DTO_s
{
    public class PaginationBookResponseDTO
    {
        public List<BookDTO>? Books { get; set; }
        public int TotalPages { get; set; }
    }
}