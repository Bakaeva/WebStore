using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Interfaces;
using WebStore.Domain.Entities;
using System;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        public CatalogController(IProductData productData) => _productData = productData;

        public IActionResult Index() => View(_productData.GetProducts());

        #region Edit
        public IActionResult Edit(int id)
        {
            var product = _productData.GetProductById(id);
            if (product is null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (!ModelState.IsValid) return View(product);

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
    }
}
