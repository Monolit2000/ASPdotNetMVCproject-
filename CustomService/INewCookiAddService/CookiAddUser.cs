using Nancy.Json;
using WebApplication1.Models;

namespace WebApplication1.CustomService
{
    public class CookiAddUser : INewCookiAddService
    {

        public async Task CookiAddUserAsync(HttpContext context, ApplicationContext _db)
        {
            Guid GUID = Guid.NewGuid();
            string UserGUID = new JavaScriptSerializer().Serialize(GUID);
            context.Response.Cookies.Append("User", UserGUID);

            string? UserCooKiIdd = context.Request.Cookies["User"];
            await _db.Users.AddAsync(new User { CookiId = UserGUID });
            await _db.SaveChangesAsync();
        }

    }
}
