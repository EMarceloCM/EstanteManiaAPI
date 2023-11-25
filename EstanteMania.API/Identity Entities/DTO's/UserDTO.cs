using System.ComponentModel.DataAnnotations;

namespace EstanteMania.API.Identity_Entities.DTO_s
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}