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
                        Path = "/images/productImages/" + fileName,
                    };

                    images.Add(productImage);
                }

                var item = new Item
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    ArticleNr = model.ArticleNr,
                    PriceWithoutVAT = model.PriceWithoutVAT,
                    Description = model.Description,
                    CategoryId = int.Parse(model.Category),
                    Color = model.Color,
                    HasSize = model.HasSize,
                    ProductImages = images,
                    VAT = factorValue,
                    IsPublished = false
                };

                _itemRepository.AddItem(item);

            }

                return RedirectToAction("AllItems");
        }

        public IActionResult AllItems() 
        {
            List<Item> items = _itemRepository.GetAllItems();
            return View(items);
        }
    }
}
