using Microsoft.AspNetCore.Mvc;
using ResturantProj.Models;
using ResturantProj.ResContext;

namespace ResturantProj.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly MyResContext resContext;

        public MenuItemController(MyResContext myRes)
        {
            resContext = myRes;
        }
        public IActionResult Index()
        {

            return View();
        }
    }
}
