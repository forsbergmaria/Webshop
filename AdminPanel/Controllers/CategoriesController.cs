using Data;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var cat = _dbContext.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            var model = new SubcategoryViewModel
            {
                Category = cat,
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
            var selectCategory = _dbContext.Categories.ToList();
            var categories = new List<string>();
            foreach (var category in selectCategory)
            {
                categories.Add(category.Name);
            }
            var selectList = new SelectList(categories, "Name").OrderByDescending(c => c.Text);
            ViewBag.CategoryList = selectList;
            return View("CreateSubcategory");
        }

        public IActionResult CreateSubcategoryFromCategory(int id, Subcategory subcategory)
        {
            var category = _dbContext.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            var model = new Subcategory
            {
                CategoryId = id,
                Name = subcategory.Name,
            };

            if (ModelState.IsValid)
            {
                _dbContext.Subcategories.Add(model);
                _dbContext.SaveChanges();
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateSubcategory(CreateSubcategoryViewModel model) 
        {
            var category = _dbContext.Categories.Where(c => c.Name == model.CategoryName).FirstOrDefault();
            var subcategory = new Subcategory
            {
                CategoryId = category.CategoryId,
                Name = model.SubcategoryName
            };

            if(ModelState.IsValid)
            {
                _dbContext.Subcategories.Add(subcategory);
                _dbContext.SaveChanges();
            }
            return View();
        }

    }
}
