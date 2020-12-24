using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Регистрация нового пользователя
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                UserName = model.UserName
            };

            var registration_result = await _userManager.CreateAsync(user, model.Password);
            if (registration_result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.User);

                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        #endregion
    }
}
