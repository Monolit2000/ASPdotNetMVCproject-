namespace WebApplication1.CustomService
{
    public interface ICustomCookiAddService
    {
        Task customCookiAdd(string name, HttpContext context);
    }
}
