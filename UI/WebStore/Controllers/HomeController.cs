using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
     
        public IActionResult Index([FromServices]IProductData ProductData)
        {
            var products = ProductData.GetProducts()
                .OrderBy(p => p.Order)
                .Take(6)
                .ToView();
            ViewBag.Products = products;
            return View();
        }
    }
}
