using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System.Net.Sockets;

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
            var category = _categoryRepository.GetCategoryByName(model.Category);
            Item item = new Item();
            item.Name = model.Name;
            item.ArticleNr = model.ArticleNr;
            item.PriceWithoutVAT = model.PriceWithoutVAT;
            item.Description = model.Description;
            item.CategoryId = category.CategoryId;
            item.Color = model.Color;
            item.HasSize = model.HasSize;
            item.VAT = model.VAT;
            item.ProductImages = model.ProductImages;
            item.IsPublished = false;

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
