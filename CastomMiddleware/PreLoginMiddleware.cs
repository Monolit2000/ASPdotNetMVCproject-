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
        //ApplicationContext _db;
        //HttpContext _context;
        INewCookiAddUserService CookiAddUser;
        public PreLoginMiddleware(RequestDelegate next, INewCookiAddUserService cookiAddUser)
        {
            this.CookiAddUser = cookiAddUser;
           // _context = context;
            //_db = DBcontext;
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext context, ApplicationContext _db)
        {
            var user = context.User.Identity;
            string? UserCooKiId = context.Request.Cookies["User"];

            if (context.Request.Cookies.ContainsKey("User"))
            {
                if (!user.IsAuthenticated)
                {
                   
                    /////////////////////////////////////////////////
                    if (!_db.Users.Any(a => a.CookiId == UserCooKiId))
                    ///////////////////////////////////////////////////
                    {
                        await _db.Users.AddAsync(new User { CookiId = UserCooKiId });
                        await _db.SaveChangesAsync();
                    }
                    //  await _db.Users.AddAsync(new User { CookiId = UserCooKiId });
                    //  await _db.SaveChangesAsync();
                }
                else if (user.IsAuthenticated)
                {
                    if (context.Request.Cookies.ContainsKey("User"))
                    {
                        var userInDb = await _db.Users.FirstOrDefaultAsync(c => c.CookiId == UserCooKiId);
                        //if (userInDb != null) context.Response.Cookies.Delete();

                        string? UserCookies = context.Request.Cookies[".AspNetCore.Cookies"];
                        userInDb.CookiId = UserCookies;
                        await _db.SaveChangesAsync();

                        //!!!!!UserCookies != UserCooKiId
                        context.Response.Cookies.Append("User", UserCookies);
                    }
                }
            }

            //при SingOut нужно создовать новый User[Cooki]
            else if (!context.Request.Cookies.ContainsKey("User"))
            {
               await CookiAddUser.CookiAddUserAsync(context, _db);   // await UserAddCooki(context,_db);
            }
            await next.Invoke(context);
        
        }

        //public async Task UserAddCooki(HttpContext context, ApplicationContext _db)
        //{
        //    Guid GUID = Guid.NewGuid();
        //    string UserGUID = new JavaScriptSerializer().Serialize(GUID);
        //    context.Response.Cookies.Append("User", UserGUID);

        //    string? UserCooKiIdd = context.Request.Cookies["User"];
        //    await _db.Users.AddAsync(new User { CookiId = UserGUID });
        //    await _db.SaveChangesAsync();
        //}
    }


    

}



