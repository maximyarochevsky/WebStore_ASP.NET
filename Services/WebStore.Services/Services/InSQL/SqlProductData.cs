﻿using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL;

public class SqlProductData : IProductData
{
    private readonly WebStoreDB _db;
    public SqlProductData(WebStoreDB db)
    {
        _db = db;
    }
    public IEnumerable<Brand> GetBrands()
    {
        return _db.Brands;
    }

    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
    {
        IQueryable<Product> query = _db.Products.Include(p => p.Brand).Include(p => p.Section);

        if (Filter?.Ids?.Length > 0)
        {
            query = query.Where(product => Filter.Ids.Contains(product.Id));
        }

        else
        {
            if (Filter?.SectionId is { } section_id)
                query = query.Where(p => p.SectionId == section_id);

            if (Filter?.BrandId is { } brand_id)
                query = query.Where(p => p.BrandId == brand_id);
        }

        return query;
    }

    public IEnumerable<Section> GetSections()
    {
        return _db.Sections;
    }

    public Product? GetProductById(int id)
    {
        return _db.Products.Include(p => p.Brand).Include(p => p.Section).FirstOrDefault(p => p.Id == id);
    }

    public Brand? GetBrandById(int? Id)
    {
        var brand = _db.Brands
            .Include(b => b.Products)
            .FirstOrDefault(b => b.Id == Id);

        return brand;
    }

    public Section GetSectionById(int Id)
    {
        var section = _db.Sections
            .Include(s => s.Products)
            .FirstOrDefault(s => s.Id == Id);

        return section;
    }

    public bool Edit(Product product)
    {
        if (product is null)
            throw new ArgumentException(nameof(product));

        var db_product = GetProductById(product.Id);

        if (db_product is null)
            return false;

        _db.Products.Update(db_product);
        _db.SaveChanges();

        return true;
    }

    public bool Delete(int Id)
    {
        var product = _db.Products.FirstOrDefault(p => p.Id == Id);

        if (product is null)
            return false;

        _db.Products.Remove(product);
        _db.SaveChanges();

        return true;
    }

    public Product CreateProduct(string Name, int Order, decimal Price, string ImageIrl, string Section, string Brand)
    {
        throw new NotImplementedException();
    }

}
