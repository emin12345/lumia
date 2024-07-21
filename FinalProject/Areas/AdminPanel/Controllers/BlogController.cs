using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BlogController : Controller
    {

        private readonly LumiaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(LumiaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            ViewBag.TotalPage = Math.Ceiling((double)_context.Blogs.Count() / 3);
            ViewBag.CurrentPage = page;



            List<Blog> blog = _context.Blogs.Include(p => p.BlogComments).AsNoTracking().Skip((page - 1) * 3).Take(3).ToList();
            return View(blog);
        }
        public IActionResult Create()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
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
            blog.Accept = true;
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Blog blog = _context.Blogs.FirstOrDefault(p => p.Id == id);
            if (blog == null) return RedirectToAction("Index", "NotFound");

            return View(blog);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Blog blog)
        {
            Blog existedBlog = _context.Blogs.FirstOrDefault(s => s.Id == blog.Id);
            if (existedBlog == null) return RedirectToAction("Index", "NotFound");
            if (!ModelState.IsValid) return View(existedBlog);

            if (blog.ImageFile != null)
            {
                existedBlog.Name = blog.Name;
                existedBlog.Text = blog.Text;
                existedBlog.Description = blog.Description;
                if (!blog.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Photo Sec");
                    return View(existedBlog);
                }
                if (!blog.ImageFile.IsLenghtMatch(2))
                {
                    ModelState.AddModelError("ImageFile", "2MB Max");
                    return View(existedBlog);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "images/blogs", existedBlog.ImagePath);
                existedBlog.ImagePath = blog.ImageFile.SaveImg(_env.WebRootPath, "images/blogs");

            }

            existedBlog.Name = blog.Name;
            existedBlog.Text = blog.Text;
            existedBlog.Description = blog.Description;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }




        public IActionResult Comments(int BlogId)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (!_context.BlogComments.Any(c => c.BlogId == BlogId)) return RedirectToAction("Index", "Blog");

            List<BlogComment> comment = _context.BlogComments.Include(c => c.User).Where(c => c.BlogId == BlogId).ToList();
            return View(comment);
        }
        public IActionResult CommentAccept(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (_context.Comments.Any(c => c.Id == id)) return RedirectToAction("Index", "Blog");
            BlogComment comment = _context.BlogComments.SingleOrDefault(c => c.Id == id);
            comment.IsAccess = true;
            _context.SaveChanges();
            return RedirectToAction("Comments", "Blog", new { BlogId = comment.BlogId });
        }
        public IActionResult CommentReject(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (!_context.BlogComments.Any(c => c.Id == id)) return RedirectToAction("Index", "Blog");
            BlogComment comment = _context.BlogComments.SingleOrDefault(c => c.Id == id);
            comment.IsAccess = false;
            _context.SaveChanges();
            return RedirectToAction("Comments", "Blog", new { BlogId = comment.BlogId });
        }
        public IActionResult AcceptBlog(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Blog blog = _context.Blogs.SingleOrDefault(c => c.Id == id);
            blog.Accept = true;
            _context.SaveChanges();
            return RedirectToAction("Index", "Blog");

        }
        public IActionResult RejectBlog(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            Blog blog = _context.Blogs.SingleOrDefault(c => c.Id == id);
            blog.Accept = false;
            _context.SaveChanges();
            return RedirectToAction("Index", "Blog");

        }
    }
}
