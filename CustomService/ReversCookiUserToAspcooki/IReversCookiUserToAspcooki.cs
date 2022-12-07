using WebApplication1.Models;

namespace WebApplication1.CustomService
{
    public interface IReversCookiUserToAspcooki
    {
        Task reversCookiUserToAspcookiAsync(HttpContext context, ApplicationContext db);
    }
}
