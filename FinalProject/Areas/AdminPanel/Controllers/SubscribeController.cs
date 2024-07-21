using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SubscribeController : Controller
    {
        private readonly LumiaDbContext _context;

        public SubscribeController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            IEnumerable<Subscriber> subscribes = _context.Subscribers.AsEnumerable();
            return View(subscribes);
        }
    }
}
