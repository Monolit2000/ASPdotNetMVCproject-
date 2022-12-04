using WebApplication1.Models;

namespace WebApplication1.CustomService
{
    public interface ICoookiUserRegestratorService
    {
        Task cookiUserRegestrator(HttpContext context, ApplicationContext db);
    }
}
