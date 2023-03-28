using Data;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AdminPanel.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoriesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllCategories()
        {
            List<Category> categories = _dbContext.Categories.ToList();
            return View(categories);
        }

        public IActionResult AllSubcategories(int id)
        {
            var category = _dbContext.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            var model = new SubcategoryViewModel
            {
                CategoryName = category.Name,
                Subcategories = _dbContext.Subcategories.Where(c => c.CategoryId == id).ToList()
            };

            return View(model);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
            }
            return View();

        }

        public IActionResult CreateSubcategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSubcategory(Subcategory subcategory) 
        {
            if(ModelState.IsValid)
            {
                _dbContext.Subcategories.Add(subcategory);
                _dbContext.SaveChanges();
            }
            return View();
        }

    }
}
