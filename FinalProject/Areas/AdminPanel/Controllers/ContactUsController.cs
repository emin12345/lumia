using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using FinalProject.ViewModels;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]

    public class ContactUsController : Controller
    {
        private readonly LumiaDbContext _context;

        public ContactUsController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            List<ContactUs> contactUs = _context.ContactUs.ToList();
            return View(contactUs);
        }

        public IActionResult SendEmailToUser()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            return View();
        }

        [HttpPost]
        public ActionResult SendEmailToUser(AdminMessageVM model)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            string recipientEmail = model.RecipientEmail;
            string message = model.Message;

            string adminEmail = "hellojob440@gmail.com";
            string adminPassword = "eomddhluuxosvnoy";
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(adminEmail, adminPassword);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(adminEmail, "Lumia");
            mailMessage.To.Add(new MailAddress(recipientEmail));
            mailMessage.Subject = "Lumia Admin";
            mailMessage.Body = message;

            try
            {
                client.Send(mailMessage);
                return RedirectToAction("Index", "ContactUs");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
        }


    }
}
