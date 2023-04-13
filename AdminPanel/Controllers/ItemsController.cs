using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System.Net.Sockets;
using System.Web.Razor.Tokenizer.Symbols;

namespace AdminPanel.Controllers
{
    public class ItemsController : Controller
    {
        ItemRepository _itemRepository { get { return new ItemRepository(); } }
        CategoryRepository _categoryRepository { get { return new CategoryRepository(); } }
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

        public IActionResult CreateItem(ItemViewModel model) 
        {
            int categoryId = int.Parse(model.Category);

            var item = new Item
            {
                Name = model.Name,
                Brand = model.Brand,
                ArticleNr = model.ArticleNr,
                PriceWithoutVAT = model.PriceWithoutVAT,
                Description = model.Description,
                CategoryId = categoryId,
                Color = model.Color,
                HasSize = model.HasSize,
                VAT = model.VAT,
                ProductImages = model.ProductImages,
                IsPublished = false
            };
            
            _itemRepository.AddItem(item);
            return RedirectToAction("AllItems");
        }

        public IActionResult AllItems() 
        {
            List<Item> items = _itemRepository.GetAllItems();
            return View(items);
        }
        
        public Image GetImage(int id)
        {
            return _itemRepository.GetImage(id);
        }
    }
}
