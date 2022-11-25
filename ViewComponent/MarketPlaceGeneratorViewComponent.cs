
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
namespace WebApplication1.ViewComponent
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    //using WebApplication1.Models;
    
    public class MarketPlaceGeneratorViewComponent : ViewComponent
    {
        ApplicationContext _db;
        public MarketPlaceGeneratorViewComponent( ApplicationContext context )
        {
            _db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("item-card", await _db.CartItems.ToListAsync());
        }
    }



}
    
