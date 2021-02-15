using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();
        Section GetSectionById(int id);
        Section GetSectionByName(string name);

        IEnumerable<Brand> GetBrands();
        Brand GetBrandById(int id);
        Brand GetBrandByName(string name);

        IEnumerable<Product> GetProducts(ProductFilter filter = null);
        Product GetProductById(int id);
        int AddProduct(Product product);
        void UpdateProduct(Product product);
        bool DeleteProduct(int id);
    }
}
