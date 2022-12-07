using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
namespace WebApplication1.ViewComponent
{
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class sheipViewComponent : ViewComponent
    {
    
        ApplicationContext _db;
        public sheipViewComponent(ApplicationContext db)
        {
            _db = db;
         
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
             
            string? GuidId = Request.Cookies["User"];
            string? CastomUserId = "CastonUser111";
            _db.Users.Include(c => c.CartItems).ToList();
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.CookiId == GuidId);
            List<CartItem> ItemCountinShape = new List<CartItem>();

            if ( user != null)
            {
                foreach (var prod in user?.CartItems)
                {
                    ItemCountinShape.Add(prod);
                }
            }


            int caaunt = ItemCountinShape.Count();
            return View("ShapeItemCaunt", ItemCountinShape);
        }
    

    }
}
