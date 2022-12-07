﻿using System.Text.Json;
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
        INewCookiAddService CookiAddUser;
        public PreLoginMiddleware(RequestDelegate next, INewCookiAddService cookiAddUser)
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
                    
                }
            }

            //при SingOut нужно создовать новый User[Cooki]
            else if (!context.Request.Cookies.ContainsKey("User"))
            {
                if (!context.Request.Cookies.ContainsKey("LogIned"))
                {
                    await CookiAddUser.CookiAddUserAsync(context, _db);   // await UserAddCooki(context,_db);
                }
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



