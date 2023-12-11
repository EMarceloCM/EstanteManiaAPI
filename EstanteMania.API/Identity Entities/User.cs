using EstanteMania.Models.Models;
using System.Text.Json.Serialization;

namespace EstanteMania.API.Identity_Entities
{
    public class User
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }

        [JsonIgnore]
        public Carrinho? Carrinho { get; set; }
    }
}