using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class CartController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(LumiaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            OrderVM model = new OrderVM()
            {
                Fullname = user.FullName,
                Username = user.UserName,
                Email = user.Email,
                BasketItems = _context.BasketItems.Include(p => p.Product).ThenInclude(p => p.ProductImages).Where(c => c.UserId == user.Id).ToList()


            };


            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            return View(model);
        }
    }
}
