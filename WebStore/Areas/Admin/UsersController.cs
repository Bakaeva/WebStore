using Microsoft.AspNetCore.Mvc;

namespace WebStore.Areas.Admin
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public IActionResult Index() => View();
    }
}
