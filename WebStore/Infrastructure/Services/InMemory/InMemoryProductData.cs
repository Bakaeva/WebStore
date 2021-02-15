using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Data;
using WebStore.Domain;
using System;

namespace WebStore.Infrastructure.Services.InMemory
{
    [Obsolete("Класс устарел, потому что данные уже не хранятся в памяти. Используйте класс SqlProductData.cs", true)]
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

        public Section GetSectionById(int id) => throw new NotSupportedException();

        public Brand GetBrandById(int id) => throw new NotSupportedException();

        public Product GetProductById(int id) => throw new NotSupportedException();

        int IProductData.AddProduct(Product product) => throw new NotSupportedException();

        void IProductData.UpdateProduct(Product product) => throw new NotSupportedException();

        bool IProductData.DeleteProduct(int id) => throw new NotSupportedException();

        Section IProductData.GetSectionByName(string name) => throw new NotSupportedException();

        Brand IProductData.GetBrandByName(string name) => throw new NotSupportedException();
    }
}
