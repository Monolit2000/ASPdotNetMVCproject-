using Nancy.Json;
using WebApplication1.Models;

namespace WebApplication1.CustomService
{
    public class CookiAddUser : INewCookiAddService
    {

        public async Task CookiAddUserAsync(HttpContext context, ApplicationContext _db)
        {
            Guid GUID = Guid.NewGuid();
            CookieOptions options = new CookieOptions();
            options.HttpOnly = true;
            context.Response.Cookies.Append("User", GUID.ToString(), new CookieOptions {HttpOnly = true });
            await _db.Users.AddAsync(new User { CookiId = GUID.ToString() });
            await _db.SaveChangesAsync();
        }

    }
}
