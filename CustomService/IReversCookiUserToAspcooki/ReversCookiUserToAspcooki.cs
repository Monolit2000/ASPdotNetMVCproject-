using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.CustomService
{
    public class ReversCookiUserToAspcooki : IReversCookiUserToAspcooki
    {
        public async Task reversCookiUserToAspcookiAsync(HttpContext context, ApplicationContext _db)
        {
            var user = context.User.Identity;
            string? UserCooKiId = context.Request.Cookies["User"];

            if (user.IsAuthenticated && context.Request.Cookies.ContainsKey("User"))
            {
                    var userInDb = await _db.Users.FirstOrDefaultAsync(c => c.CookiId == UserCooKiId);
                    string? UserCookies = context.Request.Cookies[".AspNetCore.Cookies"];
                    userInDb.CookiId = UserCookies;
                    await _db.SaveChangesAsync();
                    //!!!!!UserCookies != UserCooKiId
                    context.Response.Cookies.Append("User", UserCookies); 
            }
        }
    }
}
