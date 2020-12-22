using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        readonly IProductData _productData;

        public SectionsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke()
        {
            Domain.Entities.Section[] sections = _productData.GetSections().ToArray();

            var parent_section_views = sections.Where(s => s.ParentId is null)
               .Select(s => new SectionViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Order = s.Order,
                   ProductsCount = s.Products.Count()
               })
               .ToDictionary(s => s.Id);

            foreach (var group in sections.Where(s => s.ParentId != null).GroupBy(s => s.ParentId))
            {
                SectionViewModel parentViewModel = parent_section_views[(int)group.Key];
                parentViewModel.ChildSections.AddRange(group.OrderBy(s => s.Order).Select(child_sect => new SectionViewModel
                {
                    Id = child_sect.Id,
                    Name = child_sect.Name,
                    Order = child_sect.Order,
                    ParentSection = parentViewModel,
                    ProductsCount = child_sect.Products.Count()
                }));
            }

            return View(parent_section_views.Values.OrderBy(s => s.Order));
        }

        //public async Task<IViewComponentResult> InvokeAsync() => View();
    }
}
