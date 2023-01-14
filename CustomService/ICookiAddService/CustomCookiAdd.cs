using Nancy.Json;

namespace WebApplication1.CustomService
{
    public class CustomCookiAdd : ICustomCookiAddService
    {
        IHttpContextAccessor _HttpCookieAccessor;   
        public CustomCookiAdd(IHttpContextAccessor accessor)
        {
            _HttpCookieAccessor = accessor;   

        }
        public async Task customCookiAdd(string name)
        {
            if(!_HttpCookieAccessor.HttpContext.Request.Cookies.ContainsKey($"{name}") )
            {

                Guid GUID = Guid.NewGuid();
                string UserGUID = new JavaScriptSerializer().Serialize(GUID);
                _HttpCookieAccessor.HttpContext.Response.Cookies.Append($"{name}", UserGUID);
            }
        }
    }
}
