using WebApplication1.Models;

namespace WebApplication1.CustomService
{
    public interface INewLogInedCookiAdd
    {
        Task LogInedCookiAddAsync(HttpContext context, ApplicationContext _db);
    }
}
