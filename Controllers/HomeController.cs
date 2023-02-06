using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.CustomService;
//using System.Net;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        ApplicationContext db;

        IHttpContextAccessor _context;
        public HomeController(
            ILogger<HomeController> logger,
            ApplicationContext Dbcontext,
            IHttpContextAccessor context,
            ICustomCookiAddService cookiAdd)
        {

            db = Dbcontext;
        }
        // IHttpContextAccessor accessor

        public IActionResult Index(int? companyId)
        {
            return View();
        }
        public async Task<IActionResult> ShowAddItems()
        {
            return View("item-card", await db.CartItems.ToListAsync());
        }

        [HttpGet]
        //  [Authorize(Policy = "Admin")]
        public IActionResult AddItem()
        {
            return View();
        }


        [HttpPost]


        public async Task<IActionResult> AddItem(CartItem Item)
        {
            db.CartItems.Add(Item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> getItemToCard(int id)
        {
            ViewBag.id = id;
            return View("getItemToCardView");
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int? idItem)
        {
            if (idItem != null)
            {
                CartItem? item = await db.CartItems.FirstOrDefaultAsync(p => p.ItemId == idItem);
                if (item != null)
                {
                    db.CartItems.Remove(item);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        //[HttpPost]

        //public async Task<IActionResult> AddToShoppingCartItems()
        //{
        //}
        public async void AddShoppingCart(int ItemId)
        {
            var ShoppingCartId = GetShoppingCartId();
            var ShoppingCart = db.ShoppingCartItems.SingleOrDefault(
                c => c.UserId == ShoppingCartId
                && c.ItemId == ItemId);

            if (ShoppingCart == null)
            {
                ShoppingCart = new ShoppingCart
                {
                    UserId = ShoppingCartId,
                    CartId = Guid.NewGuid().ToString(),
                    ItemId = ItemId,
                    cartitem = await db.CartItems.SingleOrDefaultAsync(p => p.ItemId == ItemId),
                    Quantity = 1
                };
                db.ShoppingCartItems.Add(ShoppingCart);
            }
            else
            {
                ShoppingCart.Quantity++;
            }
            db.SaveChanges();
        }

        public string GetShoppingCartId()
        {
            return Request.Cookies["ShoppingCartId"];
        }


        public List<ShoppingCart> GetShoppingCart()
        {
            var ShoppingCartId = GetShoppingCartId();

            return db.ShoppingCartItems.Where(c => c.UserId == ShoppingCartId).ToList();
        }

        // [Authorize]

        //[HttpPost]
        //public async Task<IActionResult> AddCartItems( int ItemId )
        //{
        //   await AddShoppingCart(ItemId);

        //   return View( GetShoppingCart() );   

        //}

        public async Task<IActionResult> AddUserItemCaunt(int ItemId)
        {

            AddShoppingCart(ItemId);
            string? UserCooKiId = Request.Cookies["User"];
            Console.WriteLine($"{UserCooKiId}//////////////////////////////////////////////");
            ViewBag.idItem = ItemId;
            string? CastomUserId = "CastonUser111";

            CartItem? item = await db.CartItems.FirstOrDefaultAsync(p => p.ItemId == ItemId);
            User? user = await db.Users.FirstOrDefaultAsync(u => u.CookiId == UserCooKiId);
            user?.CartItems?.Add(item);
            await db.SaveChangesAsync();

            ViewBag.TestlistCartinUser = db.Users.Include(c => c.CartItems).ToList();

            return RedirectToAction("Index");
        }



        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task <IActionResult> DeleteUserItemShaip(int ItemId)
        {
            //Response.Cookies.Append("ShoppingCartId", User.Identity.Name);

            string? UserCooKiId = Request.Cookies["User"];
            ViewBag.idItem = ItemId;
            var UserList = await db.Users.ToListAsync();
            CartItem? item = await db.CartItems.FirstOrDefaultAsync(p => p.ItemId == ItemId);
            User? user = await db.Users.FirstOrDefaultAsync(u => u.CookiId == UserCooKiId);
             db.Users.Include(c => c.CartItems).ToList();
                    user?.CartItems?.Remove(item);
          
            await db.SaveChangesAsync();

            ViewBag.TestlistCartinUser = db.Users.Include(c => c.CartItems).ToList();


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SearchField()
        {
            return View("SearchField");
        }

        [HttpPost]
        public IActionResult SearchField(string furnitur)
        {
            return View("Test", furnitur);
        }


        [HttpGet]
        public IActionResult Privacy()
        {
            return View("SearchField");
        }
        [HttpPost]
        public IActionResult Privacy(string furnitur)
        {
            return View("Test", furnitur);
        }

        public IActionResult Sait()
        {
            return View();
        }

     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}