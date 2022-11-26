using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Text.Json;
using Nancy.Json;
using WebApplication1;
using WebApplication1.CastomMiddleware;
using WebApplication1.CustomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
//builder.Services.AddTransient< IPreLoginService, PreLoginService >();




string connectionCardItem = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionCardItem));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Cookies");
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login");

var app = builder.Build();



/*
        app.Use(async (context,next) =>
        {
            await next.Invoke();
        });
*/

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthentication();
app.UseMiddleware<PreLoginMiddleware>();


app.Map("/hello", [Authorize] () => "Hello World!");



app.Run();


//public interface IPreLoginService
//{

//    public string? name { get; }

//}
//public class PreLoginService : IPreLoginService
//{
    

//    public PreLoginService(HttpContext context)
//    {
//        name = context.Request.Cookies["User"];
//    }

//    public string? name { get; }

//}

//public interface ITimeService
//{
//    string Time { get; }

//}

//public class TimeService : ITimeService
//{
//   public string Time => DateTime.Now.ToShortTimeString();
//}