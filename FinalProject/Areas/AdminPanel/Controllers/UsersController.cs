using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UsersController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(LumiaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            ViewBag.TotalPage = Math.Ceiling((double)_context.Users.Count() / 2);
            ViewBag.CurrentPage = page;
            IEnumerable<IdentityUser> users = _context.Users.Select(u => new IdentityUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                LockoutEnd = u.LockoutEnd,

            }).AsNoTracking().Skip((page - 1) * 2).Take(2).AsEnumerable();


            return View(users);
        }


        public async Task<IActionResult> BlockUser(string userId)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                return RedirectToAction("Index", "Users");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> NotBlockUser(string userId)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
                return RedirectToAction("Index", "Users");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
