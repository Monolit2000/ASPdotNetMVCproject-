namespace WebApplication1.Models
{
    public class ShoppingCartItems
    {

        public string UserId { get; set; }  
        public string CartId { get; set; }
        public int Quantity { get; set; }   
        public int ItemId { get; set; } 
        public CartItem cartitem { get; set; }


    }
}
