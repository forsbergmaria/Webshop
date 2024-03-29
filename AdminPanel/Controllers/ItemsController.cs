﻿using Data;
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
using DataAccess;

namespace AdminPanel.Controllers
{
    public class ItemsController : Controller
    {
        ItemRepository _itemRepository { get { return new ItemRepository(); } }
        CategoryRepository _categoryRepository { get { return new CategoryRepository(); } }
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public ItemsController(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }

        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult Index()
        {
            return View();
        }

        // Display a form for creating a new item
        [Authorize(Roles = "Huvudadministratör, Moderator")]
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

        // Create a new item
        [HttpPost]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
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

                var item = new Item();
                item.Name = model.Name;
                item.Brand = model.Brand;
                item.ArticleNr = model.ArticleNr;
                item.PriceWithoutVAT = model.PriceWithoutVAT;
                item.Description = model.Description;
                item.CategoryId = chosenCategory.CategoryId;
                item.Color = model.Color;
                item.HasSize = model.HasSize;
                item.ProductImages = images;
                item.VAT = factorValue;
                item.IsPublished = false;
                if (model.Subcategory != null)
                {
                    var chosenSubcategory = _categoryRepository.GetSubcategoryById(int.Parse(model.Subcategory));
                }
                _itemRepository.AddItem(item);
                _itemRepository.AddItemToStripe(item);


            }

                return RedirectToAction("AllItems");
        }

        // Delete an item and it's images, if there are any
        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult DeleteItem(int id)
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
                _itemRepository.DeleteAllImagesFromItem(id);
                }
            }

            _itemRepository.DeleteItem(id);

            return RedirectToAction("AllItems");
        }

        // Delete an image from an item
        [HttpPost]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult DeleteImage(int id)
        {
            var image = _itemRepository.GetImageById(id);
            var webRoothPath = _env.WebRootPath;
            var filePath = webRoothPath + image.Path;
            _itemRepository.DeleteImageFromDirectory(filePath);
            _itemRepository.DeleteImageFromDB(id);

            // Returnera ett JSON-svar
            return Json(new { success = true, message = "Bilden har tagits bort." });
        }

        // Display a form for modifying an item
        [Authorize(Roles = "Huvudadministratör, Moderator")]
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

        // Modify an item
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public async Task<IActionResult> ModifyItem(ItemViewModel model, List<IFormFile> files)
        {
            ICollection<Image> images = new List<Image>();

            var chosenCategory = _categoryRepository.GetCategory(int.Parse(model.Category));
            if (files.Count > 0)
            {
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
            }

            int id = (int)TempData["id"];
            var item = _itemRepository.GetItem(id);
            item.Name = model.Name;
            item.Brand = model.Brand;
            item.ArticleNr = model.ArticleNr;
            item.PriceWithoutVAT = model.PriceWithoutVAT;
            item.Description = model.Description;
            item.CategoryId = chosenCategory.CategoryId;
            if (model.Subcategory != null)
            {
                var chosenSubcategory = _categoryRepository.GetSubcategoryById(int.Parse(model.Subcategory));
                item.SubcategoryId = chosenSubcategory.SubcategoryId;
            }
            item.Color = model.Color;
            item.HasSize = model.HasSize;
            item.ProductImages = images;

            _itemRepository.ModifyItem(item);

            return RedirectToAction("AllItems");
        }

        // Search for an item based on it's name
        [HttpGet]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public async Task<IActionResult> SearchItems(string searchString)
        {
            var query = _context.Items.Include(c => c.Category).Include(i => i.ProductImages)
                .Include(s => s.Subcategory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchTerms = searchString.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in searchTerms)
                {
                    query = query.Where(a => a.Name.Contains(term));
                }
            }

            var items = await query.ToListAsync();

            ViewBag.SearchString = searchString;

            return View(items);
        }

        // Retrieve subcategories belonging to a specific category and convert it to JSON
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

        // Display all items
        public IActionResult AllItems() 
        {
            List<Item> items = _itemRepository.GetAllItems();
            return View(items);
        }

        public IActionResult ViewMoreInfo(int id)
        {
            var item = _itemRepository.GetItem(id);
            return View(item);
        }

        public IActionResult ItemPublisherManager(int id)
        {
            var item = _itemRepository.GetItem(id);
            item.IsPublished = !item.IsPublished;
            _itemRepository.ModifyItem(item);
            return View("ViewMoreInfo", item);
        }
    }
}
