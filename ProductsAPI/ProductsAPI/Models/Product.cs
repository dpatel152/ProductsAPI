using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models
{    
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "SKU is required.")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "BuyerId is required.")]
        public string BuyerId { get; set; }

        public bool Active { get; set; }
    }
}

