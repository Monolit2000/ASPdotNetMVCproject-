using Nancy.Json;

namespace WebApplication1.CustomService
{
    public class CustomCookiAdd : ICustomCookiAddService
    {
        public async Task customCookiAdd(string name, HttpContext context)
        {
            if(!context.Request.Cookies.ContainsKey($"{name}") )
            {
                Guid GUID = Guid.NewGuid();
                string UserGUID = new JavaScriptSerializer().Serialize(GUID);
                context.Response.Cookies.Append($"{name}", UserGUID);
            }
        }
    }
}
