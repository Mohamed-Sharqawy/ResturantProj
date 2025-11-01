using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturantProj.Models;
using ResturantProj.ResContext;

namespace ResturantProj.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly MyResContext resContext;

        public CategoryController(MyResContext context)
        {
            resContext = context;
        }
        public IActionResult Index()
        {
            var categories = resContext.Categories.ToList();
            return View(categories);
        }

        public IActionResult AddCat()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCat(string name, IFormFile? imagefile)
        {
            string ImagePath = "Imgs/res1.jpg";
            if (imagefile != null && imagefile.Length > 0)
            {
                var filename = Path.GetFileName(imagefile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fullPath = Path.Combine(uploadPath, filename);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagefile.CopyTo(stream);
                }

                ImagePath = "/imgs/" + filename;
            }
            var cat = new Category()
            {
                Name = name,
                ImgUrl = ImagePath
            };
            resContext.Categories.Add(cat);
            resContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult GetDishes(int id)
        {
            var cat = resContext.Categories.Include(c => c.MenuItems).FirstOrDefault(cat => cat.Id == id);

            if (cat == null)
            {
                return NotFound();
            }

            cat.MenuItems ??= new List<MenuItem>();
            
            return View(cat);
        }

        public IActionResult Edit(int id)
        {
            var cat =  resContext.Categories.FirstOrDefault(c => c.Id == id);
            return View(cat);
        }

        

        [HttpPost]
        public IActionResult Edit(int id, Category category, IFormFile? imagefile)
        {
            var existingCategory = resContext.Categories.FirstOrDefault(c => c.Id == id);

            if (existingCategory == null)
                return NotFound();

            
            existingCategory.Name = category.Name;

            
            if (imagefile != null && imagefile.Length > 0)
            {
                var filename = Path.GetFileName(imagefile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fullPath = Path.Combine(uploadPath, filename);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagefile.CopyTo(stream);
                }

                
                existingCategory.ImgUrl = "/imgs/" + filename;
            }

            
            existingCategory.UpdatedAt = DateTime.Now;

            resContext.Update(existingCategory);
            resContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var category = resContext.Categories.FirstOrDefault(prd => prd.Id == id);

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var cat = resContext.Categories.FirstOrDefault(c => c.Id == id);

            cat.IsDeleted = true;
            cat.IsActive = false;
            resContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AddAddNewMenuItem(int categoryId)
        {
            var category = resContext.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                return NotFound();

            var newItem = new MenuItem
            {
                CategoryId = category.Id
            };

            return View(newItem);
        }

        [HttpPost]
        public IActionResult AddAddNewMenuItem(MenuItem item, IFormFile? imagefile)
        {
            string imagePath = "/imgs/res1.jpg";

            if (imagefile != null && imagefile.Length > 0)
            {
                var filename = Path.GetFileName(imagefile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fullPath = Path.Combine(uploadPath, filename);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagefile.CopyTo(stream);
                }

                imagePath = "/imgs/" + filename;
            }

            item.ImgUrl = imagePath;

            
            if (string.IsNullOrWhiteSpace(item.Description))
                item.Description = "No description provided.";

            resContext.MenuItems.Add(item);
            resContext.SaveChanges();

            return RedirectToAction("GetDishes", "Category", new { id = item.CategoryId });
        }

        [HttpGet]
        public IActionResult MenuItemEdit(int id)
        {
            var item = resContext.MenuItems.FirstOrDefault(m => m.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        public IActionResult MenuItemEdit(MenuItem updatedItem, IFormFile? imagefile)
        {
            var existingItem = resContext.MenuItems.FirstOrDefault(m => m.Id == updatedItem.Id);
            if (existingItem == null)
                return NotFound();

            
            existingItem.Name = updatedItem.Name;
            existingItem.Description = updatedItem.Description;
            existingItem.Price = updatedItem.Price;
            existingItem.PreparationTimeMinutes = updatedItem.PreparationTimeMinutes;
            existingItem.IsAvailable = updatedItem.IsAvailable;

            
            if (imagefile != null && imagefile.Length > 0)
            {
                var fileName = Path.GetFileName(imagefile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagefile.CopyTo(stream);
                }

                existingItem.ImgUrl = "/imgs/" + fileName;
            }

            resContext.SaveChanges();

            return RedirectToAction("GetDishes", new { id = existingItem.CategoryId });
        }

        [HttpPost]
        public IActionResult MenuItemDelete(int id)
        {
            var item = resContext.MenuItems.FirstOrDefault(m => m.Id == id);
            if (item == null)
                return NotFound();

            item.IsDeleted = true;
            item.IsAvailable = false;
            resContext.SaveChanges();

            return RedirectToAction("GetDishes", new { id = item.CategoryId });
        }


    }
}
