using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BrandController : Controller
    {
        private readonly LumiaDbContext _context;

        public BrandController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            IEnumerable<Brand> brands = _context.Brands.AsEnumerable();
            return View(brands);
        }

        public IActionResult Create()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Brand newbrand)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "You cannot duplicate Brand name");

                return View(newbrand);
            }
            _context.Brands.Add(newbrand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (id == 0) return RedirectToAction("Index", "NotFound");
            Brand brand = _context.Brands.FirstOrDefault(c => c.Id == id);
            if (brand is null) return RedirectToAction("Index", "NotFound");
            return View(brand);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Brand edited)
        {
            if (id != edited.Id) return BadRequest();
            Brand brand = _context.Brands.FirstOrDefault(c => c.Id == id);
            if (brand is null) return RedirectToAction("Index", "NotFound");
            bool duplicate = _context.Brands.Any(c => c.Name == edited.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate Brand name");
                return View();
            }
            brand.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var brand = _context.Brands.FirstOrDefault(c => c.Id == id);

            if (brand == null)
            {

                return RedirectToAction("Index", "NotFound");
            }


            return View(brand);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var brand = _context.Brands.FirstOrDefault(c => c.Id == id);

            if (brand == null)
            {
                return RedirectToAction("Index", "NotFound");
            }

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
