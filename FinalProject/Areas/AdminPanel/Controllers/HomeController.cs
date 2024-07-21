using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Model;
using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class HomeController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly UserManager<User> _userManager;
        public HomeController(LumiaDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {

            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;


            ViewBag.WomenCount = _context.Products
   .Include(p => p.ProductImages)
   .Where(p => p.ProductCategories.Any(c => c.Category.Name == "Women")).Count();



            ViewBag.MenCount = _context.Products
  .Include(p => p.ProductImages)
  .Where(p => p.ProductCategories.Any(c => c.Category.Name == "Men")).Count();


            var messages = _context.ContactUs.ToList();
            ViewBag.Messages = messages;

            var subscribe = _context.Subscribers.Count();
            ViewBag.Subscribe = subscribe;

            var categoryProducts = _context.Products.Include(p => p.ProductCategories).ThenInclude(p => p.Category).ToList();
            ViewBag.CategoryProducts = categoryProducts;
            //

            var categoryCounts = new Dictionary<string, int>();

            foreach (var product in categoryProducts)
            {
                foreach (var productCategory in product.ProductCategories)
                {
                    var categoryName = productCategory.Category.Name;
                    if (categoryCounts.ContainsKey(categoryName))
                    {
                        categoryCounts[categoryName]++;
                    }
                    else
                    {
                        categoryCounts[categoryName] = 1;
                    }
                }
            }

            ViewBag.CategoryCounts = categoryCounts;


            //

            var orderCountAdminPage = _context.OrderItems.Count();

            ViewBag.OrderCountAdminPage = orderCountAdminPage;






            var blogComment = _context.BlogComments.Include(c => c.User).Include(c => c.Blog).ToList();
            ViewBag.BlogComment = blogComment;



            List<User> users = _userManager.Users.ToList();
            ViewBag.UserCount = users.Count;

            List<Product> products = _context.Products.ToList();
            ViewBag.ProductsCount = products.Count;

            List<Blog> blogs = _context.Blogs.ToList();
            ViewBag.BlogCount = blogs.Count;



            List<OrderItem> orderItems = _context.OrderItems.ToList();
            Dictionary<int, int> orderCounts = new Dictionary<int, int>();


            foreach (OrderItem item in orderItems)
            {
                if (orderCounts.ContainsKey(item.OrderId))
                    orderCounts[item.OrderId]++;
                else
                    orderCounts[item.OrderId] = 1;
            }


           




            Dictionary<string, int> productCounts = new Dictionary<string, int>();


            List<Order>? orders = _context.Orders.ToList();

            double totalPrices = 0;

            foreach (var order in orders)
            {
                totalPrices += order.TotalPrice;
            }
            ViewBag.TotalOrderPrices = totalPrices;
            return View(orders);
        }


    }

}