using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        private readonly LumiaDbContext _context;

        public CategoryController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            IEnumerable<Category> categories = _context.Categories.AsEnumerable();
            return View(categories);
        }

        public IActionResult Create()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Category newcategory)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "You cannot duplicate Category name");

                return View(newcategory);
            }
            _context.Categories.Add(newcategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (id == 0) return RedirectToAction("Index", "NotFound404");
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return RedirectToAction("Index", "NotFound");
            return View(category);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Brand edited)
        {
            if (id != edited.Id) return BadRequest();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return RedirectToAction("Index", "NotFound");
            bool duplicate = _context.Categories.Any(c => c.Name == edited.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate Category name");
                return View();
            }
            category.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {

                return RedirectToAction("Index", "NotFound");
            }


            return View(category);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return RedirectToAction("Index", "NotFound");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
