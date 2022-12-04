using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.CustomService
{
    public class CoookiUserRegestrator : ICoookiUserRegestratorService
    {
        public async Task cookiUserRegestrator(HttpContext context, ApplicationContext _db )
        {
            if (context.Request.Cookies.ContainsKey("User"))
            {
              
                

            }
        }
    }
}
