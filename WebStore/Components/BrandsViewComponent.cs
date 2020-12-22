using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        readonly IProductData _productData;

        public BrandsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke() => View(getBrands());

        IEnumerable<BrandViewModel> getBrands() =>
            _productData.GetBrands()
                .OrderBy(brand => brand.Order)
                .Select(brand => new BrandViewModel
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    ProductsCount = brand.Products.Count()
                });

        //public async Task<IViewComponentResult> InvokeAsync() => View();
    }
}
