using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class VendorController : Controller
    {
        private readonly LumiaDbContext _context;

        public VendorController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            IEnumerable<Vendor> vendors = _context.Vendors.AsEnumerable();
            return View(vendors);
        }

        public IActionResult Create()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Vendor newvendor)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "You cannot duplicate Vendor name");

                return View(newvendor);
            }
            _context.Vendors.Add(newvendor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (id == 0) return RedirectToAction("Index", "NotFound");
            Vendor vendor = _context.Vendors.FirstOrDefault(c => c.Id == id);
            if (vendor is null) return RedirectToAction("Index", "NotFound");
            return View(vendor);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Vendor edited)
        {
            if (id != edited.Id) return BadRequest();
            Vendor vendor = _context.Vendors.FirstOrDefault(c => c.Id == id);
            if (vendor is null) return RedirectToAction("Index", "NotFound");
            bool duplicate = _context.Vendors.Any(c => c.Name == edited.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate Vendor name");
                return View();
            }
            vendor.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var vendor = _context.Vendors.FirstOrDefault(c => c.Id == id);

            if (vendor == null)
            {

                return RedirectToAction("Index", "NotFound");
            }


            return View(vendor);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var vendor = _context.Vendors.FirstOrDefault(c => c.Id == id);

            if (vendor == null)
            {
                return RedirectToAction("Index", "NotFound");
            }

            _context.Vendors.Remove(vendor);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
