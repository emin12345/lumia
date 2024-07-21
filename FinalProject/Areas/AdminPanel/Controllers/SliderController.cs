using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(LumiaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            List<Slider> slider = _context.Sliders.ToList();
            return View(slider);

        }

        public IActionResult Create()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Photo Daxil Edin");
                return View();
            }
            if (!slider.ImageFile.IsLenghtMatch(2))
            {
                ModelState.AddModelError("ImageFile", "Max 2MB");
                return View();
            }
            if (!slider.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Image File");
                return View();
            }

            slider.ImagePath = slider.ImageFile.SaveImg(_env.WebRootPath, "images/slider");
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Slider slider = _context.Sliders.FirstOrDefault(p => p.Id == id);
            if (slider == null) return RedirectToAction("Index", "NotFound");

            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            Slider existedSlider = _context.Sliders.FirstOrDefault(s => s.Id == slider.Id);
            if (existedSlider == null) return RedirectToAction("Index", "NotFound");
            if (!ModelState.IsValid) return View(existedSlider);

            if (slider.ImageFile != null)
            {
                existedSlider.Title = slider.Title;
                existedSlider.Order = slider.Order;
                existedSlider.MiniDesc = slider.MiniDesc;
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Photo Sec");
                    return View(existedSlider);
                }
                if (!slider.ImageFile.IsLenghtMatch(2))
                {
                    ModelState.AddModelError("ImageFile", "2MB Max");
                    return View(existedSlider);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "images/slider", existedSlider.ImagePath);
                existedSlider.ImagePath = slider.ImageFile.SaveImg(_env.WebRootPath, "images/slider");

            }

            existedSlider.Title = slider.Title;
            existedSlider.Order = slider.Order;
            existedSlider.MiniDesc = slider.MiniDesc;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var slider = _context.Sliders.FirstOrDefault(c => c.Id == id);

            if (slider == null)
            {

                return RedirectToAction("Index", "NotFound");
            }


            return View(slider);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var slider = _context.Sliders.FirstOrDefault(c => c.Id == id);

            if (slider == null)
            {
                return RedirectToAction("Index", "NotFound");
            }

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
