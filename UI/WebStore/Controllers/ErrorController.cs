using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
