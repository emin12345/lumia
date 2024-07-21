using FinalProject.DAL;
using FinalProject.Model;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class OrderController : Controller
    {
        private readonly LumiaDbContext _context;

        public OrderController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            List<Order> orders = _context.Orders.ToList();
            return View(orders);
        }
        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Order order = _context.Orders.Include(o => o.OrderItems).Include(o => o.User).FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            return View(order);
        }
        public IActionResult Accept(int id, string message)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Order order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            order.Status = true;
            order.Message = message;
            _context.SaveChanges();

            return RedirectToAction("Index", "Order");
        }

        public IActionResult Reject(int id)
        {

            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Order order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            order.Status = false;

            _context.SaveChanges();

            return RedirectToAction("Index", "Order");

        }
    }
}

