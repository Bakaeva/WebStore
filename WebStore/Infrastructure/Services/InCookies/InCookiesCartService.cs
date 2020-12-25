using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Mapping;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        readonly IProductData _productData;
        readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>Соответствующее корзине название в Cookies</summary>
        readonly string _cartName;

        Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;
                var cart_cookie = context.Request.Cookies[_cartName];
                if (cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookies(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set => ReplaceCookies(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        void ReplaceCookies(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cookie);
        }

        public InCookiesCartService(IProductData productData, IHttpContextAccessor httpContextAccessor)
        {
            _productData = productData;
            _httpContextAccessor = httpContextAccessor;

            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"WebStore.Cart{user_name}";
        }

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item == null)
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item == null) return;

            if (item.Quantity > 1)
                item.Quantity--;
            else
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item == null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var product_view_models = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items.Select(item => (product_view_models[item.ProductId], item.Quantity))
            };
        }
    }
}
