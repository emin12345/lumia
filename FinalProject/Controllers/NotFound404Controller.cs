using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class NotFound404Controller : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
    }
}
