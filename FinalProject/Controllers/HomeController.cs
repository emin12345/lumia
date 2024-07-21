using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FinalProject.Extensions;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly LumiaDbContext _context;

        public HomeController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                sliders = _context.Sliders.OrderBy(s => s.Order).ToList(),


            };
            ViewBag.NewArrival = _context.Products
                .Include(p => p.ProductImages).Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).OrderByDescending(p => p.Id).Take(8).ToList();




            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;



            ViewBag.Women = _context.Products
   .Include(p => p.ProductImages)
   .Where(p => p.ProductCategories.Any(c => c.Category.Name == "Women"))
   .ToList();

            ViewBag.Men = _context.Products
  .Include(p => p.ProductImages)
  .Where(p => p.ProductCategories.Any(c => c.Category.Name == "Men"))
  .ToList();



            return View(homeVM);
        }




        public IActionResult Search(string search)
        {
            var query = _context.Products.Include(p => p.ProductImages).AsQueryable().Where(x => x.Name.Contains(search));
            List<Product> products = query.OrderByDescending(x => x.Id).Take(3).ToList();
            return PartialView("_SearchPartial", products);
        }


        public IActionResult NewsLetter(string email)
        {
            if (_context.IsRegisteredEmail(email))
            {

                return RedirectToAction("Index", "Home");
            };

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("hellojob440@gmail.com", "Lumia");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = "News";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = string.Empty;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/newsletter.html"))
            {
                body = reader.ReadToEnd();
            }
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("hellojob440@gmail.com", "eomddhluuxosvnoy");
            smtpClient.Send(mailMessage);
            TempData["EmailSender"] = true;

            _context.SaveEmail(email);
            return RedirectToAction("Index", "Home");
        }
    }

}