using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Infastructure.Mapping;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers;

public class CatalogController : Controller
{
    public readonly IProductData _ProductData;

    public CatalogController(IProductData ProductData) => _ProductData = ProductData;
    public IActionResult Index(int? SectionId, int? BrandId)
    {
        var filter = new ProductFilter
        {
            BrandId = BrandId,
            SectionId = SectionId,
        };

        var products = _ProductData.GetProducts(filter);

        var catalog_model = new CatalogViewModel
        {
            BrandId = BrandId,
            SectionId = SectionId,

            Products = products.OrderBy(p => p.Order).ToView()
        };

        return View(catalog_model);
    }

    public IActionResult Details(int id)
    {
        var product = _ProductData.GetProductById(id);

        if (product is null)
            return NotFound();
        return View(product.ToView());
    }
}
