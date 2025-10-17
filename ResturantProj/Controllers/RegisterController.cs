using Microsoft.AspNetCore.Mvc;
using ResturantProj.ResContext;

namespace ResturantProj.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();       
        }

        public IActionResult Login()
        {

            return View();
        }

    }
}
