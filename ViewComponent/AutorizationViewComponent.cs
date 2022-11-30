using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Html;

namespace WebApplication1.ViewComponent
{
    using Microsoft.AspNetCore.Mvc;
    public class AutorizationViewComponent : ViewComponent
    {
        ApplicationContext db;
       
        public AutorizationViewComponent( ApplicationContext db  )
        {
                this.db = db;
                
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var userClaim = context.User.FindFirst(ClaimTypes.Name);
            var user = User.Identity;
            
            if (/*user is ClaimsIdentity claimsIdentity */ user.IsAuthenticated)
            {
                return new HtmlContentViewComponentResult(new HtmlString($"<a class=\"navitem\" href=\"/Authorization/GetAuthorizationField\">SingOut</a>"));
            }
            else return new HtmlContentViewComponentResult(new HtmlString($"<a class=\"navitem\" href=\"/Authorization/GetAuthorizationField\">Login</a>"));





        }
    }
}
