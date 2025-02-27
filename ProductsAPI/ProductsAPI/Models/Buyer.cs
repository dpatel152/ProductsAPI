using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models
{
    public class Buyer
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}

