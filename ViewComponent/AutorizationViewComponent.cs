using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Html;

namespace WebApplication1.ViewComponent
{
    using Microsoft.AspNetCore.Authorization;
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
            var name = user.Name;
            
            if ( user.IsAuthenticated)
            {          
                return new HtmlContentViewComponentResult(new HtmlString($"<a class=\"navitem\" href=\"/Authorization/SignOutAuthorization\"> {name} SingOut</a>"));
            }
            else return new HtmlContentViewComponentResult(new HtmlString($"<a class=\"navitem\" href=\"/Authorization/SignInAuthorization\">Login</a>  <a class=\"navitem\" href=\"/Authorization/Registration\">Registration</a>"));





        }
    }
}
