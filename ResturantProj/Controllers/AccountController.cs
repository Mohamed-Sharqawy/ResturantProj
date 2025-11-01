using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResturantProj.Models;
using ResturantProj.ResContext;
using ResturantProj.VMs;
using System.Threading.Tasks;

namespace ResturantProj.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly SignInManager<CustomUser> signInManager;
        public AccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CrtUserVM crt)
        {
            if (ModelState.IsValid)
            {
                var newUser = new CustomUser()
                {
                    UserName = crt.UserName,
                    Address = crt.Address
                };

                IdentityResult res = await userManager.CreateAsync(newUser, crt.Password);

                if (res.Succeeded)
                {
                    await signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Category");
                }

                foreach (var err in res.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(crt);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogVM log)
        {
            if (ModelState.IsValid)
            {
                var sign = await signInManager.PasswordSignInAsync(log.Username, log.Password, false, true);

                if (sign.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }

                ModelState.AddModelError("", "Invalid Login Attempt");
            }

            return View(log);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
