using Microsoft.AspNetCore.Mvc;

namespace ResturantProj.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
