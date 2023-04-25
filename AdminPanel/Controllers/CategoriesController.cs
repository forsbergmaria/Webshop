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
        CategoryRepository _categoryRepository { get { return new CategoryRepository(); } }
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

        // Display all categories
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult AllCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            return View(categories);
        }

        // Display all subcategories
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult AllSubcategories(int id)
        {
            var category = _categoryRepository.GetCategory(id);

            var model = new SubcategoryViewModel
            {
                Category = category,
                Subcategories = _categoryRepository.GetAllLinkedSubcategories(id)
            };

            return View(model);
        }

        // Display a form for creating a new category
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        // Create a new category
        [HttpPost]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult CreateCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _categoryRepository.AddCategory(category);
            }
            return View();

        }

        // Display form for creating a new subcategory for a category
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult CreateSubcategory()
        {
            var selectCategory = _categoryRepository.GetAllCategories();
            var categories = new List<string>();
            foreach (var category in selectCategory)
            {
                categories.Add(category.Name);
            }
            var selectList = new SelectList(categories, "Name").OrderByDescending(c => c.Text);
            ViewBag.CategoryList = selectList;
            return View("CreateSubcategory");
        }

        // Display form for creating a new subcategory directly from the "AllSubcategories" view
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult CreateSubcategoryFromCategoryForm(int id)
        {
            var selectedCategory = _categoryRepository.GetCategory(id);
            TempData["id"] = id;
            ViewBag.SelectedCategory = selectedCategory.Name;

            return View("CreateSubcategoryFromCategory");
        }

        // Create a new subcategory directly from the "AllSubcategories" view
        [HttpPost]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
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
                _categoryRepository.AddSubcategory(subcategory);
            }

            return RedirectToAction("AllCategories");
        }

        // Create a new subcategory
        [HttpPost]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult CreateSubcategory(CreateSubcategoryViewModel model) 
        {
            var category = _categoryRepository.GetCategoryByName(model.CategoryName);
            var subcategory = new Subcategory
            {
                CategoryId = category.CategoryId,
                Name = model.SubcategoryName
            };

            if(ModelState.IsValid)
            {
                _categoryRepository.AddSubcategory(subcategory);
            }
            return View();
        }

        // Delete a category. Only accessible for head administrators
        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult DeleteCategory(int id)
        {
            _categoryRepository.DeleteCategory(id);

            return RedirectToAction("AllCategories");
        }

        // Delete a subcategory. Only accessible for head administrators
        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult DeleteSubcategory(int id)
        {
            _categoryRepository.DeleteSubcategory(id);

            return RedirectToAction("AllCategories");
        }

        // Search for a category, based on it's name
        [HttpGet]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
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

        // Search for a subcategory, based on it's name
        [HttpGet]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
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

        public IActionResult CategoryPublisherManager(int id)
        {
            var category = _categoryRepository.GetCategory(id);
            category.IsPublished = !category.IsPublished;
            _categoryRepository.ModifyCategory(category);

            return RedirectToAction("AllCategories");
        }

        public IActionResult SubcategoryPublisherManager(int id)
        {
            var subcategory = _categoryRepository.GetSubcategoryFromCategory(id);
            subcategory.IsPublished = !subcategory.IsPublished;
            _categoryRepository.ModifySubcategory(subcategory);

            return RedirectToAction("AllCategories");
        }


        // Display a form for updating a category
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult UpdateCategoryForm(int id)
        {
            var category = _categoryRepository.GetCategory(id);

      
            TempData["id"] = id;
            return View("UpdateCategory", category);
        }

        // Updates a category
        [HttpPost]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult UpdateCategory(Category category)
        {
            int id = (int)TempData["id"];
            var cat = _categoryRepository.GetCategory(id);
           
            if (ModelState.IsValid)
            {
                cat.Name = category.Name;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("AllCategories");
        }

        // Display a form for updating a subcategory
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult UpdateSubcategoryForm(int id)
        {
            var categories = _dbContext.Categories.ToList();
            var options = new List<string>();

            foreach (var item in categories)
            {
                options.Add(item.Name);
            }

            var subcategory = _dbContext.Subcategories.Where(c => c.SubcategoryId == id).FirstOrDefault();
            var selectList = new SelectList(options).OrderByDescending(c => c.Text);
            ViewBag.CategoryList = selectList;

            TempData["id"] = id;
            return View("UpdateSubcategory", subcategory);
        }

        // Update a subcategory
        [HttpPost]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult UpdateSubcategory(Subcategory subcategory)
        {
            int id = (int)TempData["id"];
            var sub = _dbContext.Subcategories.Where(c => c.SubcategoryId == id).FirstOrDefault();
            var newCategory = _dbContext.Categories.Where(c => c.Name == subcategory.Categories.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                sub.CategoryId = newCategory.CategoryId;
                sub.Name = subcategory.Name;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("AllCategories");
        }

    }
}
