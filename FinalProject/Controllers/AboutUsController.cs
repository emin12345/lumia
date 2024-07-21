using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly LumiaDbContext _context;

        public AboutUsController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            AboutUs aboutUs = _context.AboutUs.FirstOrDefault();
            return View(aboutUs);
        }
    }
}
