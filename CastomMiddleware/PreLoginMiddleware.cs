using System.Text.Json;
using WebApplication1.Models;
using System.Security.Cryptography.X509Certificates;
using WebApplication1.CustomService;
using System.Xml.Serialization;
using Nancy.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication1.CastomMiddleware
{
    public class PreLoginMiddleware
    {
        const string _cookieScheme = "AnonimCookies";
        private readonly RequestDelegate next;
        INewCookiAddService CookiAddUser;
        public PreLoginMiddleware(RequestDelegate next, INewCookiAddService cookiAddUser)
        {
            this.CookiAddUser = cookiAddUser;
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context, ApplicationContext _db, IHttpContextAccessor _context)
        {
            var test = context.Request.Cookies["User"];

            //var role =  context.User.FindFirst("role").Value;

            
             
                if (!context.Request.Cookies.ContainsKey("User") && !context.Request.Cookies.ContainsKey("LogIned")) 
                {


                //await context.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "eeAnonimUser") })));

                //var claims = new List<Claim> { new Claim("role" , "AnonimUser") };
                //ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, _cookieScheme);
                //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await context.SignOutAsync(_cookieScheme);
                //await context.SignInAsync(claimsPrincipal);

                await CookiAddUser.CookiAddUserAsync(context, _db);
                }
                // if (context.Request.Cookies.ContainsKey("User") && context.Request.Cookies.ContainsKey("Anonimus"))
                //{
                //    await context.SignOutAsync(_cookieScheme);
                //}

            await next.Invoke(context);

        }
    }


    

}



