using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using FinalProject.Utilites.Roles;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LumiaDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, LumiaDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM account)
        {
            if (!ModelState.IsValid) return View();
            User user = new User
            {
                FullName = string.Concat(account.Firstname, " ", account.Lastname),
                Email = account.Email,
                UserName = account.Username
            };
            IdentityResult result = await _userManager.CreateAsync(user, account.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError message in result.Errors)
                {
                    ModelState.AddModelError("", message.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            return RedirectToAction("Index", "Home");
            ;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            User user = await _userManager.FindByNameAsync(login.Username);

            if (user is null)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Due to  overtying your account has been blocked for 5 minutes");
                    return View();
                }
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
