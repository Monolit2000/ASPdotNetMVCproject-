using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AuthorizationController : Controller
    {
        //HomeController _homeController;
        //public AuthorizationController(HomeController homeController)
        //{
        //    _homeController = homeController;
        //}
        ApplicationContext _db;

        public AuthorizationController(ApplicationContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return PartialView("Registration");  
        }


        [HttpPost]
        public async Task<IActionResult> Registration(User user)
        {
            if (_db.Users.Any(c => c.Email == user.Email))
            {
                string? userCookiId = Request.Cookies["User"];
                return Content($" {userCookiId} Вже зареєстрований ;) ");
            }
            else
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(claimsIdentity));

                string? userCookiId = Request.Cookies["User"];
                var userr = await _db.Users.FirstOrDefaultAsync(c => c.CookiId == userCookiId);
                userr.Email = user.Email;
                userr.Password = user.Password;
                userr.CookiId = userCookiId;
                //await _db.Users.AddAsync(new User { Password = user.Password,
                //                                       Email = user.Email,
                //                                     CookiId = userCookiId });
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }


          
        }

        [HttpGet]
        public IActionResult SignInAuthorization()
        {
            return PartialView("SignInAuthorization");
        }

        [HttpPost]
        public async Task<IActionResult> SignInAuthorization(User user)
        {
            //return $"{user.id} --- {user.CookiId} --- {user.Email} --- {user.Password}";
            var userr = await _db.Users.FirstOrDefaultAsync(c => c.Email == user.Email && c.Password == user.Password);
            if (userr != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
            else return RedirectToAction("Registration");
            //await _db.Users.AddAsync(new User { Password = user.Password, Email = user.Email});
            //await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");   
        }

        
        public async Task<IActionResult> SignOutAuthorization()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        // GET: AuthorizationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthorizationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorizationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorizationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthorizationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorizationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthorizationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
