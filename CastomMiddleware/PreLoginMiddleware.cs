using System.Text.Json;
using Nancy.Json;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.CastomMiddleware
{
    public class PreLoginMiddleware
    {
        private readonly RequestDelegate next;
        //  private readonly ApplicationContext _db;
        public PreLoginMiddleware(RequestDelegate next /*, ApplicationContext context*/)
        {
            // _db = context;
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
                    if (!_db.Users.Any(a => a.CookiId == UserCooKiId))
                    {
                        await _db.Users.AddAsync(new User { CookiId = UserCooKiId });
                        await _db.SaveChangesAsync();
                    }
                }
                else if(user.IsAuthenticated)
                {

                    var userInDb = await _db.Users.FirstOrDefaultAsync(c => c.CookiId == UserCooKiId);
                    string? UserCookies = context.Request.Cookies[".AspNetCore.Cookies"];
                   // userInDb.CookiId = UserCookies;
                    await _db.SaveChangesAsync();
                    context.Response.Cookies.Append("User", UserCooKiId);
                    //var putin = await _db.Users.FirstOrDefaultAsync(p => p.CookiId == UserCooKiId);
                    //if (putin != null) 
                    //_db.Users.Remove(putin);
                    //await _db.SaveChangesAsync();

                }
            }

            //if (user.IsAuthenticated)
            //{
            //    var userInDb = await _db.Users.FirstOrDefaultAsync(c => c.CookiId == UserCooKiId );
            //    string? UserCookies = context.Request.Cookies[".AspNetCore.Cookies"];
            //    userInDb.CookiId = UserCookies;
            //    await _db.SaveChangesAsync();
            //    context.Response.Cookies.Append("User", UserCookies);



            //}

            else if (!context.Request.Cookies.ContainsKey("User"))
            {
                Guid GUID = Guid.NewGuid();
                string UserGUID = new JavaScriptSerializer().Serialize(GUID);
                context.Response.Cookies.Append("User", UserGUID);

                string? UserCooKiIdd = context.Request.Cookies["User"];
                await _db.Users.AddAsync(new User { CookiId = UserGUID });
                await _db.SaveChangesAsync();

            }
                              await next.Invoke(context);
//if(user.IsAuthenticated)
            //{
            //    string? UserCooKiId = context.Request.Cookies[".AspNetCore.Cookies"];

            //    context.Response.Cookies.Append("User", UserCooKiId);
            //}


            //}
            //else
            //{


            //    var Userlist = await _db.Users.ToListAsync();
            //    //создаём клиента в базе данных и добавляем ему в свойства куки
            //    foreach (var user in Userlist)
            //    {
            //        if (user.CookiId != UserGUID)
            //        {
            //            await _db.Users.AddAsync(new User { CookiId = UserGUID });
            //            await _db.SaveChangesAsync();
            //        }
            //    }
            //}

            

            }
        }
    }



