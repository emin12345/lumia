using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class OrderHistoryController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LumiaDbContext _context;

        public OrderHistoryController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, LumiaDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            List<Order> orders = _context.Orders.Where(o => o.UserId == userId).ToList();
            return View(orders);
        }
    }
}
