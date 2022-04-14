﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laba4.ViewModels;
using Laba4.Models;
using System;
using Microsoft.AspNetCore.Identity;

namespace Laba4.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.LastLogin = DateTime.Now;
                model.Register = DateTime.Now;
                User user = new User { Email = model.Email, UserName = model.Email, LastLogin = model.LastLogin, Name = model.Name, Register = model.Register, Status = true };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded && (await _userManager.FindByEmailAsync(model.Email)).Status)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    user.LastLogin = System.DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Users");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль или вы заблокированы");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _userManager.UpdateAsync(user);
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> RedirectToUsersTable()
        {
            return RedirectToAction("Index","Users");
        }
    }
}