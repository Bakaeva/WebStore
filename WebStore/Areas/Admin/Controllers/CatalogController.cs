using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Interfaces;
using WebStore.Domain.Entities;
using System;
using WebStore.ViewModels;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using System.Linq;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        public CatalogController(IProductData productData) => _productData = productData;

        public IActionResult Index() => View(_productData.GetProducts());

        public IActionResult Create() => View("Edit", new ProductViewModel());

        #region Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new ProductViewModel()); // вызов представления-формы добавления товара

            if (id < 0)
                return BadRequest();

            var product = _productData.GetProductById((int)id);
            if (product is null) return NotFound();

            return View(new ProductViewModel
            {
                Id = (int)id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Brand = product.Brand.Name,
                Section = product.Section.Name,
            }); // вызов представления-формы редактирования товара
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid) return View(model);

            var product = new Product
            {
                Id = model.Id,
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                Brand = _productData.GetBrandByName(model.Brand),
                Section = _productData.GetSectionByName(model.Section),
            };

            if (product.Id == 0)
                _productData.AddProduct(product);
            else
                _productData.UpdateProduct(product);

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            var product = _productData.GetProductById(id);
            if (product is null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            if (id < 0)
                return BadRequest();

            _productData.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        public IActionResult Commit()
        {
            return NoContent(); // временная заглушка
        }
    }
}
