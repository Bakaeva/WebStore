using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();
        Section GetSectionById(int id);

        IEnumerable<Brand> GetBrands();
        Brand GetBrandById(int id);

        IEnumerable<Product> GetProducts(ProductFilter filter = null);
        Product GetProductById(int id);
        //int AddProduct(Product product);
        void UpdateProduct(Product product);
        bool DeleteProduct(int id);
    }
}
