using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.CustomService;
using Nancy.Security;
using System.Net.Http;
using WebApplication1.Filters;
using Nancy.Json;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        //HomeController _homeController;
        //public AccountController(HomeController homeController)
        //{
        //    _homeController = homeController;
        //}

        ApplicationContext _db;
        INewCookiAddService _СookiAddUser;
        INewLogInedCookiAdd _NewLogInedCookiAdd;

        IReversCookiUserToAspcooki _CeversCookiUserToAspcooki;

        public AccountController(ApplicationContext context,
                                       INewCookiAddService cookiAddUser,
                                       IReversCookiUserToAspcooki _reversCookiUserToAspcooki,
                                       INewLogInedCookiAdd NewLogInedCookiAdd)
        {
            _NewLogInedCookiAdd = NewLogInedCookiAdd;
            _CeversCookiUserToAspcooki = _reversCookiUserToAspcooki;
            _db = context;
            _СookiAddUser = cookiAddUser;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            var _httpcontext = HttpContext;
            _СookiAddUser.CookiAddUserAsync(_httpcontext, _db);
            return PartialView("Registration");
        }

      //  [ReversCookiFilter]
        [HttpPost]
        public async Task<IActionResult> Registration(User user)
        {
            if (_db.Users.Any(c => c.Email == user.Email))
            {
               // string? userCookiId = Request.Cookies["User"];
                return Content($" {user.Email} Вже зареєстрований ;) ");
            }
            else
            {
                var _httpcontext = HttpContext;

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(claimsIdentity));


                string? userCookiId = Request.Cookies["User"];


                var userr = await _db.Users.FirstOrDefaultAsync(c => c.CookiId == userCookiId);
                userr.Email = user.Email;
                userr.Password = user.Password;
                userr.CookiId = userCookiId;
                await _db.SaveChangesAsync();


                await _NewLogInedCookiAdd.LogInedCookiAddAsync(_httpcontext,_db);
                return RedirectToAction("ReversCookiUserToAspcookiAction"/*"Index", "Home"*/);
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
            var _httpcontext =  HttpContext;

            var userInDB = await _db.Users.FirstOrDefaultAsync(c => c.Email == user.Email && c.Password == user.Password);
            if (userInDB != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
                 
            else return RedirectToAction("Registration");
            _httpcontext.Response.Cookies.Append("User",userInDB.CookiId);

            await _NewLogInedCookiAdd.LogInedCookiAddAsync(_httpcontext, _db);
            return RedirectToAction("Index", "Home");   
        }

        public async Task <IActionResult> ReversCookiUserToAspcookiAction()
        {
           var httpcontext = HttpContext;
           await _CeversCookiUserToAspcooki.reversCookiUserToAspcookiAsync(httpcontext, _db);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> SignOutAuthorization()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var _context = HttpContext;
           // await _СookiAddUser.CookiAddUserAsync(_context, _db);

            return RedirectToAction("Index", "Home");
        }

      

      

     
    }
}
