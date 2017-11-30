using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class AccountController : Controller
    {
        protected UserManager<UserModel> UserManager;
        protected SignInManager<UserModel> SignInManager;


        //Odbieramy wstrzyknięte ze startup.
        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel();
                user.UserName = registerViewModel.Login;
                user.Email = registerViewModel.Email;
                IdentityResult identityResult = await UserManager.CreateAsync(user, registerViewModel.Password);
                if (identityResult.Succeeded)
                {
                    await SignInManager.PasswordSignInAsync(user, registerViewModel.Password, true, false);
                    return RedirectToAction("Index", "Task");
                }
                else
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel logInViewModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await SignInManager.PasswordSignInAsync(logInViewModel.Login, logInViewModel.Password, true, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Task");
                }
                else
                {
                    ModelState.AddModelError("", "Login or password incorrect");
                }
            }
            return View(logInViewModel);
        }

        
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
