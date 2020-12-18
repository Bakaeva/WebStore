using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Data;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;
        
        public IEnumerable<Brand> GetBrands() => TestData.Brands;
    }
}
