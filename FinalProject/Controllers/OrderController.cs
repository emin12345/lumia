using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Utilites.Roles;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Model;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(LumiaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Checkout()
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            OrderVM model = new OrderVM()
            {
                Fullname = orderVM.Fullname,
                Username = orderVM.Username,
                Email = orderVM.Email,
                BasketItems = _context.BasketItems.Include(p => p.Product).ThenInclude(p => p.ProductImages).Where(c => c.UserId == user.Id).ToList()


            };
            if (!ModelState.IsValid) return View(model);
            TempData["Succeeded"] = false;
            if (model.BasketItems.Count == 0) return RedirectToAction("Index", "Home");


            Order order = new Order()
            {
                Country = orderVM.Country,
                State = orderVM.State,
                Adress = orderVM.Address,
                TotalPrice = 0,
                Date = DateTime.Now,
                UserId = user.Id,
            };


            foreach (BasketItem item in model.BasketItems)
            {
                order.TotalPrice += (double)item.Product.Price;
                OrderItem orderItem = new OrderItem
                {
                    Name = item.Product.Name,
                    Price = (double)item.Product.Price,
                    UserId = user.Id,
                    ProductId = item.ProductId,
                    Order = order
                };
                _context.OrderItems.Add(orderItem);
            }
            _context.BasketItems.RemoveRange(model.BasketItems);
            _context.Orders.Add(order);
            _context.SaveChanges();
            TempData["Succeeded"] = true;
            return RedirectToAction("Index", "Home");
        }
    }
}
