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
        INewCookiAddUserService cookiAddUser;
        ICoookiUserRegestratorService CookiUserRegestratorService;
        IReversCookiUserToAspcooki ReversCookiUserToAspcooki;

        //  HttpContext _context;
        public AuthorizationController(ApplicationContext context, 
                                       INewCookiAddUserService cookiAddUser,
                                       IReversCookiUserToAspcooki ReversCookiUserToAspcooki,
                                       ICoookiUserRegestratorService coookiUserRegestratorService)
                                       
        {

            this.ReversCookiUserToAspcooki = ReversCookiUserToAspcooki;
            this.CookiUserRegestratorService = coookiUserRegestratorService;
            _db = context;
            this.cookiAddUser = cookiAddUser;
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
               // string? userCookiId = Request.Cookies["User"];
                return Content($" {user.CookiId} Вже зареєстрований ;) ");
            }
            else
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(claimsIdentity));
                

              //  string? userCookiId = Request.Cookies["User"];
                //string? userCookiId1 = Request.Cookies[".AspNetCore.Cookies"];
                //Response.Cookies.Append("User", userCookiId1);
                string? userCookiId = Request.Cookies["User"];


                var userr = await _db.Users.FirstOrDefaultAsync(c => c.CookiId == userCookiId);
                userr.Email = user.Email;
                userr.Password = user.Password;
                userr.CookiId = userCookiId;
                await _db.SaveChangesAsync();
                //var httpcontext = HttpContext;
                //await ReversCookiUserToAspcooki.reversCookiUserToAspcookiAsync(httpcontext, _db);
                return RedirectToAction(/*"Index", "Home"*/"ReversCookiUserToAspcookiAction");
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
            var httpcontext =  HttpContext;
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
            // await CookiUserRegestratorService.cookiUserRegestrator(httpcontext,_db);
            httpcontext.Response.Cookies.Append("User",userr.CookiId);
            return RedirectToAction("Index", "Home");   
        }

        public async Task <IActionResult> ReversCookiUserToAspcookiAction()
        {
           var httpcontext = HttpContext;
           await ReversCookiUserToAspcooki.reversCookiUserToAspcookiAsync(httpcontext, _db);

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> SignOutAuthorization()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var _context = HttpContext;
            await cookiAddUser.CookiAddUserAsync(_context, _db);

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
