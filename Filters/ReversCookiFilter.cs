using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebApplication1.CustomService;
using WebApplication1.Models;

namespace WebApplication1.Filters
{
    public class ReversCookiFilter : Attribute, IActionFilter
    {
        //IReversCookiUserToAspcooki _CeversCookiUserToAspcooki;

        //public ReversCookiFilter(IReversCookiUserToAspcooki _CeversCookiUserToAspcooki)
        //{
        //    this._CeversCookiUserToAspcooki = _CeversCookiUserToAspcooki;
        //}

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            using (ApplicationContext _db = new ApplicationContext())
            {
                // var con = context.HttpContext;
                var user = context.HttpContext.User.Identity;
                string? UserCooKiId = context.HttpContext.Request.Cookies["User"];

                if (user.IsAuthenticated && context.HttpContext.Request.Cookies.ContainsKey("User"))
                {
                    var userInDb = _db.Users.FirstOrDefault(c => c.CookiId == UserCooKiId);
                    string? UserCookies = context.HttpContext.Request.Cookies[".AspNetCore.Cookies"];
                    userInDb.CookiId = UserCookies;
                    _db.SaveChangesAsync();
                    //!!!!!UserCookies != UserCooKiId
                    context.HttpContext.Response.Cookies.Append("User", UserCookies);
                }
            }
        }

        
       
    }
}
