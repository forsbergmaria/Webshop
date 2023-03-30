using Data;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AdminPanel.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CategoryRepository _categoryRepository;

        public CategoriesController(ApplicationDbContext dbContext, CategoryRepository categoryRepository)
        {
            _dbContext = dbContext;
            _categoryRepository = categoryRepository;
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

        public IActionResult CreateSubcategoryFromCategoryForm(int id)
        {
            var selectedCategory = _dbContext.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
            TempData["id"] = id;
            ViewBag.SelectedCategory = selectedCategory.Name;

            return View("CreateSubcategoryFromCategory");
        }

        [HttpPost]
        public IActionResult CreateSubcategoryFromCategory(CreateSubcategory2ViewModel model)
        {
            int id = (int)TempData["id"];
            var subcategory = new Subcategory
            {
                CategoryId = id,
                Name = model.Name
            };

            if (ModelState.IsValid)
            {
                _dbContext.Subcategories.Add(subcategory);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("AllCategories");
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

        public IActionResult DeleteCategory(int id)
        {
            var category = _dbContext.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
            var undefined = _dbContext.Categories.Where(c => c.Name.Equals("Odefinierad")).FirstOrDefault();
            var subcategories = _dbContext.Subcategories.Where(c => c.Categories.CategoryId == id).ToList();
            var items = _dbContext.Items.ToList();

            List<Item> itemsList = new List<Item>();

            foreach (var item in items )
            {
                if (item.CategoryId == id)
                {
                    itemsList.Add(item);
                }
            }
            foreach (var item in itemsList)
            {
                item.CategoryId = undefined.CategoryId;
                item.Subcategory = null;
            }

            _dbContext.Remove(category);
            _dbContext.SaveChanges();

            return RedirectToAction("AllCategories");
        }

        public IActionResult DeleteSubcategory(int id)
        {
            var undefined = _dbContext.Categories.Where(c => c.Name.Equals("Odefinierad")).FirstOrDefault();
            var subcategory = _dbContext.Subcategories.Where(c => c.SubcategoryId == id).FirstOrDefault();
            var items = _dbContext.Items.ToList();

            List<Item> itemsList = new List<Item>();
            foreach (var item in items)
            {
                itemsList.Add(item);
            }
            foreach (var item in itemsList)
            {
                item.SubcategoryId = subcategory.SubcategoryId;
                item.SubcategoryId = null;
            }
            
            _dbContext.Subcategories.Remove(subcategory);
            _dbContext.SaveChanges();

            return RedirectToAction("AllCategories");
        }

        [HttpGet]
        public async Task<IActionResult> SearchCategory(string searchString)
        {
            var query = _dbContext.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchTerms = searchString.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in searchTerms)
                {
                    query = query.Where(a => a.Name.Contains(term));
                }
            }

            var categories = await query.ToListAsync();

            ViewBag.SearchString = searchString;

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSubcategory(int id, string searchString)
        {
            var query = _dbContext.Subcategories.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchTerms = searchString.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in searchTerms)
                {
                    query = query.Where(a => a.Name.Contains(term));
                }
            }

            var cat = _dbContext.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            var subCategories = await query.ToListAsync();

            var model = new SubcategoryViewModel
            {
                Category = cat,
                Subcategories = subCategories
            };

            ViewBag.SearchString = searchString;

            return View(model);
        }


    }
}
