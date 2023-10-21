using Microsoft.AspNetCore.Mvc;

namespace Jobverse.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}