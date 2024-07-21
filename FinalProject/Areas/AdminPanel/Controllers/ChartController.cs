using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]

    public class ChartController : Controller
    {
        private readonly LumiaDbContext _context;

        public ChartController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            IEnumerable<Product>? products = _context.Products.Include(p => p.ProductImages)
                                                       .Include(p => p.ProductSizeColors).ThenInclude(p => p.Size)
                                                         .Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).AsEnumerable();




            //Satilan Productlar

            List<OrderItem> orderItems = _context.OrderItems.ToList();
            Dictionary<string, int> soldCountsByProduct = new Dictionary<string, int>();


            foreach (var orderItem in orderItems)
            {
                string productName = orderItem.Name;
                if (soldCountsByProduct.ContainsKey(orderItem.Name))
                {
                    soldCountsByProduct[orderItem.Name]++;
                }
                else
                {
                    soldCountsByProduct[orderItem.Name] = 1;
                }
            }







            ViewBag.SatilanProductlar = soldCountsByProduct;


            //
            return View(products);
        }
    }
}
