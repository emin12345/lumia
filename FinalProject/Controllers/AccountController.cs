using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using FinalProject.Entities;
using FinalProject.DAL;
using FinalProject.ViewModels;
using FinalProject.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Principal;
using FinalProject.Interface;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LumiaDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, LumiaDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult UserDetail()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            User user = _userManager.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            return View(user);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserDetailAsync(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = _userManager.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);

            if (user == null)
            {

                ModelState.AddModelError("", "User not found");
                return View(model);
            }

            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.TelNumber = model.TelNumber;






            _context.SaveChanges();
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("UserDetail", "Account");
        }


        public IActionResult Register()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
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
                UserName = account.Username,
                PhoneNumber = account.PhoneNumber,
                DateTime = DateTime.Now,

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
            else
            {
                var memberRoleExists = await _roleManager.RoleExistsAsync("Member");
                if (!memberRoleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Member"));
                }
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("hellojob440@gmail.com", "Lumia");
            mail.To.Add(new MailAddress(user.Email));

            mail.Subject = "Verify Email";


            mail.Body = string.Empty;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/verifyemail.html"))
            {
                body = reader.ReadToEnd();
            }
            mail.Body = body.Replace("{{link}}", link);
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;


            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("hellojob440@gmail.com", "eomddhluuxosvnoy");

            smtp.Send(mail);


            await _userManager.AddToRoleAsync(user, "Member");
            TempData["Register"] = true;
            return RedirectToAction("Index", "Home");
            ;
        }



        [HttpPost]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = _userManager.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);

                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "NotFound404");
                    }
                }


            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]



        [HttpPost]
        public async Task<IActionResult> ResetService(AccountVM account)
        {
            User user = await _userManager.FindByEmailAsync(account.User.Email);
            AccountVM model = new AccountVM
            {
                User = user,
            };
            if (!ModelState.IsValid) return View(model);
            account.Password = model.Password;
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            return View();
        }
        public IActionResult Forgot()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Forgot(AccountVM account)
        {
            User user = await _userManager.FindByEmailAsync(account.User.Email);

            if (user == null) BadRequest();

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());


            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("hellojob440@gmail.com", "Lumia");
            mail.To.Add(new MailAddress(user.Email));

            mail.Subject = "Reset Password";
            mail.Body = string.Empty;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/reset.html"))
            {
                body = reader.ReadToEnd();
            }
            mail.Body = body.Replace("{{reset}}", link);
            mail.IsBodyHtml = true;


            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;


            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("hellojob440@gmail.com", "eomddhluuxosvnoy");
            TempData["Forgot"] = true;
            smtp.Send(mail);
            return RedirectToAction("Index", "Home");


        }


        ////



        public async Task<IActionResult> ResetPassword(string email, string token)
        {

            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) BadRequest();

            AccountVM model = new AccountVM
            {
                User = user,
                Token = token
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(AccountVM account)
        {
            User user = await _userManager.FindByEmailAsync(account.User.Email);
            AccountVM model = new AccountVM
            {
                User = user,
                Token = account.Token
            };
            if (!ModelState.IsValid) return View(model);//IsValid
            await _userManager.ResetPasswordAsync(user, account.Token, account.Password);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            TempData["Login"] = true;
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["Logout"] = true;
            return RedirectToAction("Index", "Home");
        }




    }
}
