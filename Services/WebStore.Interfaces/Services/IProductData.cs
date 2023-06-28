using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces;
public interface IProductData
{
    IEnumerable<Section> GetSections();

    Section? GetSectionById(int id);

    IEnumerable<Brand> GetBrands();

    Brand? GetBrandById(int? id);

    IEnumerable<Product> GetProducts(ProductFilter? Filter = null);

    Product? GetProductById(int id);

    Product CreateProduct(string Name, string Order, decimal Price, string ImageIrl, string Section, string Brand);

    bool Edit(Product product);

    bool Delete(int Id);
}
