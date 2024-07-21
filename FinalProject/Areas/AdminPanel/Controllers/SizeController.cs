using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SizeController : Controller
    {

        private readonly LumiaDbContext _context;
        public SizeController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            IEnumerable<Size> size = _context.Sizes.AsEnumerable();
            return View(size);
        }


        public IActionResult Create()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Size newSize)
        {

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            bool Isdublicate = _context.Sizes.Any(c => c.Name == newSize.Name);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.Sizes.Add(newSize);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (id == 0) return NotFound();
            Size? size = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null) return NotFound();
            return View(size);
        }

        [HttpPost]
        public IActionResult Edit(int id, Size editsize)
        {
            if (id != editsize.Id) return NotFound();
            Size? size = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null) return NotFound();
            bool duplicate = _context.Sizes.Any(s => s.Name == editsize.Name && size.Name != editsize.Name);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This size name is now available");
                return View();
            }
            size.Name = editsize.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }




        public IActionResult Delete(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (id == 0) return NotFound();
            Size? size = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null) return NotFound();
            return View(size);
        }

        [HttpPost]
        public IActionResult Delete(int id, Size deletesize)
        {
            if (id != deletesize.Id) return NotFound();
            Size? size = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null) return NotFound();
            _context.Sizes.Remove(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
