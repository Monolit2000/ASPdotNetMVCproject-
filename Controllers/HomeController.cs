using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Microsoft.AspNetCore.Authorization;
//using System.Net;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        ApplicationContext db;
        public HomeController(ILogger<HomeController> logger,
            ApplicationContext Dbcontext)
        {

            db = Dbcontext;
        }


        public IActionResult Index(int? companyId)
        {


            return View();
        }

        public async Task<IActionResult> ShowAddItems()
        {
            return View("item-card", await db.CartItems.ToListAsync());
        }



        [HttpGet]
        public IActionResult AddItem()
        {
            return View();
        }


        [HttpPost]

        // Добавление карточки с товаром и нахуя я делаю админку
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

        // Удаление карточки с товаром и нахуя я делаю админку 
        [HttpPost]
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



        [HttpPost]
       // [Authorize]
        public async Task<IActionResult> AddUserItemCaunt(int ItemId)
        {         
            string? UserCooKiId = Request.Cookies["User"];
            ViewBag.idItem = ItemId;
            string? CastomUserId = "CastonUser111";
     
            CartItem? item = await db.CartItems.FirstOrDefaultAsync(p => p.ItemId == ItemId);
            User? user = await db.Users.FirstOrDefaultAsync(u => u.CookiId == UserCooKiId);
                    user?.CartItems?.Add(item);
                    await db.SaveChangesAsync();
            
                    ViewBag.TestlistCartinUser = db.Users.Include(c => c.CartItems).ToList();

            return View("Index");
        }

        

        [HttpPost]
        public async Task <IActionResult> DeleteUserItemShaip(int ItemId)
        {
            string? UserCooKiId = Request.Cookies["User"];
            ViewBag.idItem = ItemId;
            var UserList = await db.Users.ToListAsync();
            CartItem? item = await db.CartItems.FirstOrDefaultAsync(p => p.ItemId == ItemId);
            User? user = await db.Users.FirstOrDefaultAsync(u => u.CookiId == UserCooKiId);
             db.Users.Include(c => c.CartItems).ToList();
                    user?.CartItems?.Remove(item);
          
            await db.SaveChangesAsync();

            ViewBag.TestlistCartinUser = db.Users.Include(c => c.CartItems).ToList();
                 
            
            return View("Index");
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