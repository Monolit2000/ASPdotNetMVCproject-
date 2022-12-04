using WebApplication1.Models;
namespace WebApplication1.CustomService
{
    public interface INewCookiAddUserService
    {
        Task CookiAddUserAsync(HttpContext context,ApplicationContext db );
    }
}
