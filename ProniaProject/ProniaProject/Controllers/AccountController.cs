using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ProniaProject.Models;
using ProniaProject.ViewModels;
using System.Text.RegularExpressions;

namespace ProniaProject.Conrollers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _sighinManeger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _usermanager = userManager;
            _sighinManeger = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if(!ModelState.IsValid) return View();

            string regex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if(Regex.IsMatch(userVM.Email, regex))
            {
                ModelState.AddModelError(userVM.Email, "Email is not Valid");
            }

            AppUser user = new AppUser
            {
                UserName = userVM.UserName,
                Email = userVM.Email,
                Surname = userVM.Surname,
                Name = userVM.Name,
                
            };

            IdentityResult result = await _usermanager.CreateAsync(user, userVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
                return View();
            }

           await _sighinManeger.SignInAsync(user,false);

           return RedirectToAction(nameof(HomeController.Index),"Home");

            
        }
        public async Task<IActionResult> LogOut()
        {
            await _sighinManeger.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
