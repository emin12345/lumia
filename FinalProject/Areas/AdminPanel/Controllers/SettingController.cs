using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SettingController : Controller
    {
        private readonly LumiaDbContext _context;

        public SettingController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            IEnumerable<Setting> collections = _context.Settings.AsEnumerable();
            return View(collections);
        }
        public IActionResult Create()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Setting setting)
        {
            if (ModelState.IsValid)
            {
                _context.Settings.Add(setting);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setting);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Setting setting = _context.Settings.Find(id);

            if (setting == null)
            {
                return RedirectToAction("Index", "NotFound");
            }

            return View(setting);
        }

        [HttpPost]
        public IActionResult Edit(int id, Setting setting)
        {

            if (id != setting.Id)
            {
                return RedirectToAction("Index", "NotFound");
            }

            if (ModelState.IsValid)
            {
                _context.Update(setting);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setting);
        }
        [HttpGet]

        public async Task<IActionResult> Delete(int? id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (id == null)
            {
                return RedirectToAction("Index", "NotFound");
            }

            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);

            if (setting == null)
            {
                return RedirectToAction("Index", "NotFound");
            }

            return View(setting);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Setting setting = await _context.Settings.FindAsync(id);

            if (setting == null)
            {
                return RedirectToAction("Index", "NotFound");
            }

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
