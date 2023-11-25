namespace EstanteMania.API.DTO_s
{
    public class PaginationAuthorResponseDTO
    {
        public List<AuthorDTO>? Authors { get; set; }
        public int TotalPages { get; set; }
    }
}
