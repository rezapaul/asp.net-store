using EndPoint.Website.Models.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyStore.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserAdditional> _userManager;
        private readonly SignInManager<UserAdditional> _signInManager;
        

        public AccountController(UserManager<UserAdditional> userManager, SignInManager<UserAdditional> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("~/");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                var user = new UserAdditional
                {
                    FullName = model.FullName,
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    List<string> Roles = new List<string>();
                    Roles.Add("Customer");
                    await _userManager.AddToRolesAsync(user, Roles);
                    return RedirectToAction("Index", "Home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);

        }

        [HttpGet]
        public IActionResult Login(string returnUrl=null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return Redirect("~/");

            ViewData["returnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                    return View(model);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);

                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, model.RememberMe, new List<Claim>()
                    {
                        new Claim("FullName",user.FullName ?? ""),
                    });

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return Redirect("~/");
                }

                if (result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "اکانت کاربری شما به مدت پانزده دقیقه بدلیل سه بار اشتباه وارد کردن رمز قفل شده است";
                    return View(model);
                }
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if(user == null)
            {
                return Json(true);
            }
            return Json("این ایمیل قبلا استفاده شده است");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUserNameInUse(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);
            if(user == null)
            {
                return Json(true);
            }
            return Json("این نام کاربری وجود دارد");
        }


        
    }
}
