using WebApplication1.Models;
namespace WebApplication1.CustomService
{
    public interface ICookiAddUserService
    {
        Task CookiAddUserAsync(HttpContext context,ApplicationContext db );
    }
}
