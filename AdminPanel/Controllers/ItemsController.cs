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
using DataAccess;
using DataAccess.Data.Repositories;
using Models.ViewModels;

namespace AdminPanel.Controllers
{
    public class ItemsController : Controller
    {
        ItemRepository _itemRepository { get { return new ItemRepository(); } }
        SizeRepository _sizeRepository { get { return new SizeRepository(); } }
        CategoryRepository _categoryRepository { get { return new CategoryRepository(); } }
        StatisticsRepository _statisticsRepository { get { return new StatisticsRepository(); } }


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
            var sizes = _sizeRepository.GetAllSizes();
            var categories = _categoryRepository.GetAllCategories();
            var selectListCategories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).OrderByDescending(c => c.Text);

            ViewBag.Categories = selectListCategories;
            ViewBag.Sizes = _sizeRepository.GetAllSizes();
            return View("CreateItem");
        }

        // Create a new item
        [HttpPost]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public async Task<IActionResult> CreateItem(ItemViewModel model, List<IFormFile> files, int[] selectedSizes, bool hasSize)
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
                item.HasSize = hasSize;
                item.ProductImages = images;
                item.VAT = factorValue;
                item.IsPublished = false;
                if (model.Subcategory != null)
                {
                    var chosenSubcategory = _categoryRepository.GetSubcategoryById(int.Parse(model.Subcategory));
                }
                if (hasSize)
                {
                }
                _itemRepository.AddItem(item);
                _itemRepository.AddItemToStripe(item);
                if (selectedSizes != null)
                {
                    foreach (var size in selectedSizes)
                    {
                        _sizeRepository.AssignSizeToItem(item, size);
                    }
                }

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
            var model = new List<ItemViewModel>();
            foreach (var item in items)
            {
                model.Add(new ItemViewModel
                {
                    ArticleNr = item.ArticleNr,
                    PriceWithoutVAT = item.PriceWithoutVAT,
                    Category = item.Category.Name,
                    Brand = item.Brand,
                    Color = item.Color,
                    ItemId = item.ItemId,
                    Description = item.Description,
                    Name = item.Name,
                    IsPublished = item.IsPublished,
                    HasSize = item.HasSize,
                    ProductImages = item.ProductImages,
                    VAT = item.VAT.ToString(),
                    ItemBalance = _statisticsRepository.GetBalanceForOneItem(item.ItemId)
                });
            }

            return View(model);
        }

        // Returns all sizes that are connected to a specific item
        [HttpGet]
        public JsonResult GetItemSizes(int itemId)
        {
            var sizes = _sizeRepository.GetAllSizesForOneItem(itemId)
                .Select(s => new SelectListItem
            {
             Value = s.SizeId.ToString(),
             Text = s.Name
                }).OrderByDescending(s => s.Text)
                     .ToList();
            return Json(sizes);
        }
        public IActionResult ViewMoreInfo(int id)
        {
            var item = _itemRepository.GetItem(id);
            return View(item);
        }

        // Adjust stock for a specific item without sizes, by adding a transaction to the DB
        [HttpPost]
        public IActionResult AdjustStock(int itemId, int quantity, string transactionType)
        {
            var transaction = new ItemTransaction();
            transaction.ItemId = itemId;
            transaction.Quantity = quantity;
            transaction.TransactionType = transactionType;
            transaction.TransactionDate = DateTime.Now;

            _statisticsRepository.AddTransaction(transaction);
            var item = _itemRepository.GetItem(itemId);

            return View("ViewMoreInfo", item);
        }

        // Adjust stock for a specific item with sizes, by adding a transaction to the DB
        [HttpPost]
        public IActionResult AdjustStockSizes(int itemId, int quantity, string transactionType, int size)
        {
            var transaction = new TransactionWithSizes();
            transaction.ItemId = itemId;
            transaction.Quantity = quantity;
            transaction.TransactionType = transactionType;
            transaction.TransactionDate = DateTime.Now;
            transaction.SizeId = size;

            _statisticsRepository.AddTransaction(transaction);

            var item = _itemRepository.GetItem(itemId);
            return View("ViewMoreInfo", item);

        }

        // Display a form for creating a new size
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult CreateSizeForm()
        {
            List<Size> sizes = _sizeRepository.GetAllSizes();
            ViewBag.Sizes = sizes.OrderBy(i => i.Name);
            return View("CreateSize");
        }


        [HttpPost]
        public IActionResult AddSize([FromForm] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Storleksnamnet får inte vara tomt");
            }

            var size = new Size { Name = name };
            _sizeRepository.AddSizeAsync(size);


            return RedirectToAction("CreateSizeForm");
        }

        public IActionResult DeleteSize(int id)
        {
            _sizeRepository.DeleteSizeFromItemHasSize(id);
            _sizeRepository.DeleteSize(id);
            return RedirectToAction("CreateSizeForm");
        }

        [HttpPost]
        public async Task <IActionResult> UpdateSize(int sizeId, string sizeName)
        {
            if (string.IsNullOrWhiteSpace(sizeName))
            {
                return BadRequest("Storleksnamnet får inte vara tomt");
            }
            
            

            Size size = _sizeRepository.GetSize(sizeId);
            size.Name = sizeName;
            await _sizeRepository.ModifySizeAsync(size);
            
           
            return RedirectToAction("CreateSizeForm");
        }




        // Method for adding a new size to the DB, if the modelstate ís valid
        [HttpPost]
        public IActionResult CreateSize(Size size)
        {
            if (ModelState.IsValid)
            {
               _sizeRepository.AddSize(size);
            }
         
            return View();
        }

        // Method for setting the "IsPublished" property to true or false for a specific item
        public IActionResult ItemPublisherManager(int id)
        {
            var item = _itemRepository.GetItem(id);
            item.IsPublished = !item.IsPublished;
            _itemRepository.ModifyItem(item);
            return View("ViewMoreInfo", item);
        }
    }
}
