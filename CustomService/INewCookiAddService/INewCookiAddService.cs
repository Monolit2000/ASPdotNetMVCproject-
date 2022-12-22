using WebApplication1.Models;
namespace WebApplication1.CustomService
{
    public interface INewCookiAddService
    {
        Task CookiAddUserAsync(HttpContext context,ApplicationContext db );
    }
}
