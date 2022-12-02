
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

       // [Required]
        public string? Email { get; set; }

      //  [Required]
        public string? Password { get; set; }
        public string? CookiId { get; set; }
        public List<CartItem>? CartItems { get; set; } = new List<CartItem>();
    }
}
