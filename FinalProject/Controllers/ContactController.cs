using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly LumiaDbContext _context;

        public ContactController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            return View();
        }

        [HttpPost]
        public ActionResult Save(ContactUs contact)
        {


            _context.ContactUs.Add(contact);
            _context.SaveChanges();
            TempData["Contact"] = true;

            return RedirectToAction("Index", "Contact");
        }

    }
}
