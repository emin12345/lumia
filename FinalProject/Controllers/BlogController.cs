using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public BlogController(LumiaDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {


            ViewBag.TotalPage = Math.Ceiling((double)_context.Blogs.Count() / 4);
            ViewBag.CurrentPage = page;


            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            List<Blog> blog = _context.Blogs.Include(p => p.BlogComments).ThenInclude(p => p.User).Skip((page - 1) * 4).Take(4).ToList();
            return View(blog);
        }
        public IActionResult Detail(int id)
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            if (id == 0) return RedirectToAction("Index", "NotFound404");
            Blog? blog = _context.Blogs.Include(p => p.BlogComments).ThenInclude(p => p.User).FirstOrDefault(p => p.Id == id);
            if (blog == null) RedirectToAction("Index", "NotFound404");
            ViewBag.Blog = _context.Blogs
               .Where(x => x.Id != id).Take(3).ToList();
            return View(blog);
        }




        [Authorize]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddComment(BlogComment comment)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ModelState.IsValid) return RedirectToAction("Detail", "Blog", new { id = comment.BlogId });
            if (_context.Products.Any(f => f.Id == comment.BlogId)) return NotFound();

            BlogComment cmnt = new BlogComment
            {
                Text = comment.Text,
                BlogId = comment.BlogId,
                CreatedTime = DateTime.Now,
                UserId = user.Id,
                IsAccess = false,



            };
            _context.BlogComments.Add(cmnt);
            _context.SaveChanges();
            return RedirectToAction("Detail", "Blog", new { id = comment.BlogId });
        }




        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {

            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("Detail", "Blog");
            if (_context.Comments.Any(c => c.Id == id && c.IsAccess == false)) return NotFound();
            BlogComment comment = _context.BlogComments.FirstOrDefault(c => c.Id == id);
            _context.BlogComments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction("Detail", "Blog", new { id = comment.BlogId });

        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Blog blog)
        {
            if (!ModelState.IsValid) return View();
            if (blog.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Photo Daxil Edin");
                return View();
            }
            if (!blog.ImageFile.IsLenghtMatch(2))
            {
                ModelState.AddModelError("ImageFile", "Max 2MB");
                return View();
            }
            if (!blog.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Image File");
                return View();
            }



            blog.ImagePath = blog.ImageFile.SaveImg(_env.WebRootPath, "images/blogs");
            blog.CreatedTime = DateTime.Now;
            blog.Accept = false;
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}
