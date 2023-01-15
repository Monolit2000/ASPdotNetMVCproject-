
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class AnonymousUser 
    {
        [Key]
        public int AnonId { get; set; }
        public string? CookiId { get; set; }
       // public List<int>? CartItemsId { get; set; } 

    }
}
