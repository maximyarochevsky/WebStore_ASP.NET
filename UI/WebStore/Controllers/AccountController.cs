﻿using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var user = new User
            {
                UserName = Model.UserName,
            };

            var registraion_result = await _UserManager.CreateAsync(user, Model.Password).ConfigureAwait(true);
            if (registraion_result.Succeeded)
            {
                await _UserManager.AddToRoleAsync(user, Role.Users).ConfigureAwait(true);

                await _SignInManager.SignInAsync(user, false).ConfigureAwait(true);

                return RedirectToAction("Index","Home");
            }

            foreach (var error in registraion_result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(Model);
        }
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = ReturnUrl});
        }

        [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel Model)
		{
			if (ModelState.IsValid)
				return View(Model);

            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                Model.RememberMe,
                true).ConfigureAwait(true);

            if (login_result.Succeeded)
            {
                //return Redirect(Model.ReturnUrl); не безопасно

                //if (Url.IsLocalUrl(Model.ReturnUrl))
                //    return Redirect(Model.ReturnUrl);
                //return RedirectToAction("Index", "Home");

                return LocalRedirect(Model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Неверное имя пользователя, или пароль");
			return View(Model);
		}

		public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync().ConfigureAwait(true);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

