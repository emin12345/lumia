using FinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class FaqController : Controller
    {
        private readonly LumiaDbContext _context;

        public FaqController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            return View();
        }
    }
}
