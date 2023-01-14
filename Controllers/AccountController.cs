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
using System;




namespace WebApplication1.Controllers
{

    public class AccountController : Controller
    {
        const string cookieString = "Cookies";
        const string cookieScheme = "AnonimCookies";
    
        // IHttpContextAccessor accessor

        ApplicationContext _db;
        INewCookiAddService _СookiAddUser;
        INewLogInedCookiAdd _NewLogInedCookiAdd;
        ICustomCookiAddService _CustomCookiAddService;
        IReversCookiUserToAspcooki _CeversCookiUserToAspcooki;

        public AccountController(ApplicationContext context,
                                 INewCookiAddService cookiAddUser,
                                 INewLogInedCookiAdd NewLogInedCookiAdd,
                                 ICustomCookiAddService CustomCookiAddService,
                                 IReversCookiUserToAspcooki reversCookiUserToAspcooki)
        {
            _db = context;
            _СookiAddUser = cookiAddUser;                          
            _NewLogInedCookiAdd = NewLogInedCookiAdd;
            _CustomCookiAddService = CustomCookiAddService; 
            _CeversCookiUserToAspcooki = reversCookiUserToAspcooki;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            var _httpcontext = HttpContext;
            _CustomCookiAddService.customCookiAdd("Barier"/*,_httpcontext*/);
            if(/*!_httpcontext.Request.Cookies.ContainsKey("Barier") ||*/ !_httpcontext.Request.Cookies.ContainsKey("LogIned"))
            {
                    _СookiAddUser.CookiAddUserAsync(_httpcontext, _db);
            }
            return PartialView("Registration");
        }

      //[ReversCookiFilter]
      //Переробити 
        [HttpPost]
        public async Task<IActionResult> Registration(User user)
        {
            if (!ModelState.IsValid)
                return Content("Модель не пройшла валідацію");

            var _httpcontext = HttpContext;
            _httpcontext.Response.Cookies.Delete("Barier");

            
            if (_db.Users.Any(c => c.Email == user.Email))
            {
                return Content($" {user.Email} Вже зареєстрований ;) ");
            }
            else
            {

                var claims = new List<Claim> 
                { 
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("role", "User"),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, cookieString);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);

                string? userCookiId = Request.Cookies["User"];
     
                var userr = await _db.Users.FirstOrDefaultAsync(c => c.CookiId == userCookiId);
                if (userr == null)
                  return Content($" {user.Email} Not found in database ;)");
                
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
            //await _CustomCookiAddService.customCookiAdd("Barier", _httpcontext);
            if (_httpcontext.Request.Cookies.ContainsKey("Barier"))
            {
                _httpcontext.Response.Cookies.Delete("Barier");
            }

            var userInDB = await _db.Users.FirstOrDefaultAsync(c => c.Email == user.Email && c.Password == user.Password);
            if (userInDB != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("role", "User"),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, cookieString);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal, new AuthenticationProperties() 
                {
                    IsPersistent = true,
                });
                
                _httpcontext.Response.Cookies.Append("User", userInDB.CookiId);

                await _NewLogInedCookiAdd.LogInedCookiAddAsync(_httpcontext, _db);
                return RedirectToAction("Index", "Home");
            }
                 
            else return RedirectToAction("Registration");   
        }

        public async Task<IActionResult> SignOutAuthorization()
        {
            await HttpContext.SignOutAsync(cookieString);
            var _context = HttpContext;
            await _СookiAddUser.CookiAddUserAsync(_context,_db);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ReversCookiUserToAspcookiAction()
        {
            var httpcontext = HttpContext;
            await _CeversCookiUserToAspcooki.reversCookiUserToAspcookiAsync(httpcontext, _db);

            return RedirectToAction("Index", "Home");
        }

    }
}
