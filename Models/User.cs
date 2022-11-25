
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string? Email { get; set; } 
        public string? Password { get; set; }
        public string? CookiId { get; set; }
        public List<CartItem>? CartItems { get; set; } = new List<CartItem>();
    }
}
