using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Area("AdminPanel")]
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
    }
}
