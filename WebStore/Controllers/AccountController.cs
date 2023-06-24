using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
		public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
                _SignInManager = SignInManager;
                _UserManager = UserManager; 
        } 
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (ModelState.IsValid)
                return View(Model);

            var user = new User
            {
                UserName = Model.UserName,
            };

            var registraion_result = await _UserManager.CreateAsync(user, Model.Password);
            if (registraion_result.Succeeded)
            {
                await _SignInManager.SignInAsync(user, false);

                return RedirectToAction("Index","Home");
            }

        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

