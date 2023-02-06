using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ShoppingCart
    {
        [Key]
        public string CartId { get; set; }
        public string UserId { get; set; }  
        public int Quantity { get; set; }   
        public int ItemId { get; set; } 
        public CartItem cartitem { get; set; }

    }
}
