using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Controllers
{

    [Area("Account")]
    public class HController : Controller
    {
        public IActionResult Indexx()
        {
            return View();
        }
    }
}
