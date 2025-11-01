using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturantProj.Models;
using ResturantProj.ResContext;
using System.Threading.Tasks;

namespace ResturantProj.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly MyResContext resContext;

        public MenuItemController(MyResContext myRes)
        {
            resContext = myRes;
        }
        public async Task<IActionResult> Index()
        {
            var menuitms = await resContext.MenuItems.Include(m => m.Category).ToListAsync();
            return View(menuitms);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleAvailability(int id)
        {
            var item = await resContext.MenuItems.FindAsync(id);
            if (item == null)
                return NotFound();

            // Toggle availability
            item.IsAvailable = !item.IsAvailable;

            // Reset daily order count
            item.DailyOrderCount = 0;

            resContext.Update(item);
            await resContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Assuming this page is your Index
        }

    }
}
