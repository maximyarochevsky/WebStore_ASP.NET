using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
using WebStore.Domain;

namespace WebStore.Services;

public class InMemoryProductData : IProductData
{
    public IEnumerable<Brand> GetBrands() => TestData.Brands;

    public IEnumerable<Section> GetSections() => TestData.Sections;

    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
    {
        IEnumerable<Product> query = TestData.Products;

        if(Filter?.SectionId is { } section_id)
            query = query.Where(p => p.SectionId == section_id);

        if(Filter?.BrandId is { } brand_id)
            query = query.Where(p => p.SectionId == brand_id);
        return query;
    }
}
