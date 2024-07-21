using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AboutUsController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AboutUsController(LumiaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            AboutUs aboutUs = _context.AboutUs.FirstOrDefault();
            return View(aboutUs);
        }


        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            AboutUs aboutUs = _context.AboutUs.FirstOrDefault(p => p.Id == id);
            if (aboutUs == null) return RedirectToAction("Index", "NotFound");

            return View(aboutUs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AboutUs about)
        {
            AboutUs existedAbout = _context.AboutUs.FirstOrDefault(s => s.Id == about.Id);
            if (existedAbout == null) return RedirectToAction("Index", "NotFound");
            if (!ModelState.IsValid) return View(existedAbout);

            if (about.ImageFile != null)
            {
                existedAbout.Author = about.Author;
                existedAbout.Text = about.Text;
                existedAbout.IframeLink = about.IframeLink;
                if (!about.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Photo Sec");
                    return View(existedAbout);
                }
                if (!about.ImageFile.IsLenghtMatch(2))
                {
                    ModelState.AddModelError("ImageFile", "2MB Max");
                    return View(existedAbout);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "images/aboutus", existedAbout.ImagePath);
                existedAbout.ImagePath = about.ImageFile.SaveImg(_env.WebRootPath, "images/aboutus");

            }

            existedAbout.Author = about.Author;
            existedAbout.Text = about.Text;
            existedAbout.IframeLink = about.IframeLink;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Detail(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            AboutUs aboutUs = _context.AboutUs.FirstOrDefault(p => p.Id == id);
            if (aboutUs == null) return RedirectToAction("Index", "NotFound");

            return View(aboutUs);
        }

    }
}
