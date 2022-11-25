using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CartItem
    {

        [Key]
        public int ItemId { get; set; }
        public string? ItemName { get; set; } 
        public int Price { get; set; }
        public List<User>? Users { get; set; } = new List<User>();
       
    }
}
