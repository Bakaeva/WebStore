using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        readonly ICartService _cartService;

        public CartController(ICartService cartService) => _cartService = cartService;

        public IActionResult Index() => View(_cartService.TransformFromCart());

        public IActionResult AddToCart(int id, int cnt = 1)
        {
            _cartService.AddToCart(id, cnt);

            //var referer = Request.Headers["Referer"]; // http://localhost:5000/Catalog/Details/n
            ////bool test = Url.IsLocalUrl(referer); // false
            //var uri = new System.Uri(referer);
            //if (uri.Host == "localhost") // это условие не будет работать после развёртывания приложения на хостинге 
            //    return LocalRedirect(uri.LocalPath);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            _cartService.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}
