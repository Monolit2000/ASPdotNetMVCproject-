using System.Text.Json;
using Nancy.Json;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApplication1.CustomService;

namespace WebApplication1.CastomMiddleware
{
    public class PreLoginMiddleware
    {
        private readonly RequestDelegate next;
        INewCookiAddService CookiAddUser;
        public PreLoginMiddleware(RequestDelegate next, INewCookiAddService cookiAddUser)
        {
            this.CookiAddUser = cookiAddUser;
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context, ApplicationContext _db)
        {

            if (!context.Request.Cookies.ContainsKey("User") && !context.Request.Cookies.ContainsKey("LogIned"))
            {
                await CookiAddUser.CookiAddUserAsync(context, _db);
            }
            await next.Invoke(context);

        }
    }


    

}



