using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<Brand> GetBrands() => _db.Brands;        

        public IEnumerable<Section> GetSections() => _db.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (filter?.SectionId != null)
                query = query.Where(product => product.SectionId == filter.SectionId);

            if (filter?.BrandId != null)
                query = query.Where(product => product.BrandId == filter.BrandId);

            return query;
        }
    }
}
