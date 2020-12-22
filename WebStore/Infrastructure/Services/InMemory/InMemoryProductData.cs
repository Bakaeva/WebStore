using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Data;
using WebStore.Domain;

namespace WebStore.Infrastructure.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            var query = TestData.Products;

            //if (filter?.SectionId != null)
            //    query = query.Where(product => product.SectionId == filter.SectionId);
            if (filter?.SectionId is { } section_id)
                query = query.Where(product => product.SectionId == section_id);

            if (filter?.BrandId is { } brand_id)
                query = query.Where(product => product.BrandId == brand_id);

            return query;
        }
    }
}
