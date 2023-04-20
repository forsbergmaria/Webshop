using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Web.Razor.Tokenizer.Symbols;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class ItemsController : Controller
    {
        ItemRepository _itemRepository { get { return new ItemRepository(); } }
        CategoryRepository _categoryRepository { get { return new CategoryRepository(); } }
        private readonly IWebHostEnvironment _env;

        public ItemsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateItemForm()
        {
            var categories = _categoryRepository.GetAllCategories();
            var selectList = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).OrderByDescending(c => c.Text);

            ViewBag.Categories = selectList;
            return View("CreateItem");
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemViewModel model, List<IFormFile> files) 
        {
             decimal.TryParse(model.VAT, out decimal percentDecimal);
                
             // Lägger till 1 till procenten och dividerar med 100 för att få faktorvärdet
             decimal factorValue = 1 + (percentDecimal / 100);

    if (ModelState.IsValid)
            {
                ICollection<Image> images = new List<Image>();

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(_env.WebRootPath, "images/productImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var productImage = new Image
                    {
                        Path = "/images/productImages/" + fileName
                    };

                    images.Add(productImage);
                }

                var chosenCategory = _categoryRepository.GetCategory(int.Parse(model.Category));
                var chosenSubcategory = _categoryRepository.GetSubcategoryById(int.Parse(model.Subcategory));

                var item = new Item
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    ArticleNr = model.ArticleNr,
                    PriceWithoutVAT = model.PriceWithoutVAT,
                    Description = model.Description,
                    CategoryId = chosenCategory.CategoryId,
                    Color = model.Color,
                    HasSize = model.HasSize,
                    ProductImages = images,
                    VAT = factorValue,
                    IsPublished = false,
                    SubcategoryId = chosenSubcategory.SubcategoryId
                };

                _itemRepository.AddItem(item);

            }

                return RedirectToAction("AllItems");
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = _itemRepository.GetItem(id);
            var imagesList = _itemRepository.GetImagesByItemId(item.ItemId);
         if (imagesList != null)
            {
                foreach (var image in imagesList)
                {
                var fileName = image.Path;
                var webRootPath = _env.WebRootPath;
                var filePath = webRootPath + fileName;
                _itemRepository.DeleteImageFromDirectory(filePath);
                }
            }

            _itemRepository.DeleteItem(id);

            return RedirectToAction("AllItems");
        }

        public IActionResult ModifyItemForm(int id)
        {
            var categories = _categoryRepository.GetAllCategories();
            var selectList = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).OrderByDescending(c => c.Text);

            var subcategories = _categoryRepository.GetAllLinkedSubcategories(int.Parse(selectList.First().Value));
            var selectSubList = subcategories.Select(c => new SelectListItem
            {
                Value = c.SubcategoryId.ToString(),
                Text = c.Name
            }).OrderByDescending(c => c.Text);

            ViewBag.Categories = selectList;
            ViewBag.Subcategories = selectSubList;

            var item = _itemRepository.GetItem(id);
            decimal vatRate = item.VAT;
            int vatRatePercent = (int)((vatRate - 1) * 100);
            string vatRateStr = $"{vatRatePercent}";

            var model = new ItemViewModel
            {
                Name = item.Name,
                Brand = item.Brand,
                ArticleNr = item.ArticleNr,
                PriceWithoutVAT = item.PriceWithoutVAT,
                Description = item.Description,
                Category = item.Category.Name,
                Subcategory = item.Subcategory?.Name,
                Color = item.Color,
                HasSize = item.HasSize,
                ProductImages = _itemRepository.GetImagesByItemId(id),
                VAT = vatRateStr,
            };

            TempData["id"] = id;

            return View("ModifyItem", model);
        }

        public IActionResult ModifyItem(ItemViewModel model)
        {
            int id = (int)TempData["id"];
            var item = _itemRepository.GetItem(id);
            item.Name = model.Name;
            item.Brand = model.Brand;
            item.ArticleNr = model.ArticleNr;
            item.PriceWithoutVAT = model.PriceWithoutVAT;
            item.Description = model.Description;
            item.CategoryId = _categoryRepository.GetCategoryByName(model.Name).CategoryId;
            item.Color = model.Color;
            item.HasSize = model.HasSize;
            item.ProductImages = model.ProductImages;

            return RedirectToAction("AllItems");
        }

        [HttpGet]
        public JsonResult GetSubcategories(int categoryId)
        {
            var subcategories = _categoryRepository.GetAllLinkedSubcategories(categoryId)
                .Select(c => new SelectListItem
                {
                    Value = c.SubcategoryId.ToString(),
                    Text = c.Name
                }).OrderByDescending(c => c.Text).ToList();

            return Json(subcategories);
        }

        public IActionResult AllItems() 
        {
            List<Item> items = _itemRepository.GetAllItems();
            return View(items);
        }
    }
}
