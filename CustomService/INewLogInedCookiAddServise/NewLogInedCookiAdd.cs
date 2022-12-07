using Nancy.Json;
using WebApplication1.Models;

namespace WebApplication1.CustomService
{
    public class NewLogInedCookiAdd : INewLogInedCookiAdd
    {
        public async Task LogInedCookiAddAsync(HttpContext context, ApplicationContext _db)
        {
            if (!context.Request.Cookies.ContainsKey("LogIned"))
            {
                Guid GUID = Guid.NewGuid();
                string UserGUID = new JavaScriptSerializer().Serialize(GUID);
                context.Response.Cookies.Append("LogIned", UserGUID);

                //string? UserCooKiIdd = context.Request.Cookies["LogIned"];
                //await _db.Users.AddAsync(new User { CookiId = UserGUID });
                //await _db.SaveChangesAsync();
            }
        }
    }
}
