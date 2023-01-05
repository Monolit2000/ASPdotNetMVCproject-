using Nancy.Json;

namespace WebApplication1.CustomService
{
    public class CustomCookiAdd : ICustomCookiAddService
    {
        IHttpContextAccessor _accessor;   
        public CustomCookiAdd(IHttpContextAccessor accessor)
        {
            _accessor = accessor;   

        }
        public async Task customCookiAdd(string name/*, HttpContext context*/)
        {
            if(!_accessor.HttpContext.Request.Cookies.ContainsKey($"{name}") )
            {

                Guid GUID = Guid.NewGuid();
                string UserGUID = new JavaScriptSerializer().Serialize(GUID);
                _accessor.HttpContext.Response.Cookies.Append($"{name}", UserGUID);
            }
        }
    }
}
