using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        readonly IConfiguration _configuration;        

        public HomeController(IConfiguration configuration) => _configuration = configuration;

        public IActionResult Index() => View();

        public IActionResult Blog() => View();
        public IActionResult BlogSingle() => View();
        public IActionResult Cart() => View();
        public IActionResult Checkout() => View();
        public IActionResult ContactUs() => View();
        public IActionResult Login() => View();
        public IActionResult ProductDetails() => View();
        public IActionResult Shop() => View();
        public IActionResult Error404() => View();
    }
}
