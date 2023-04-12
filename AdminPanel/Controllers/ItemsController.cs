using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AdminPanel.Controllers
{
    public class ItemsController : Controller
    {
        ItemRepository _itemRepository { get { return new ItemRepository(); } }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateItemForm()
        {
            return View("CreateItem");
        }

        public IActionResult CreateItem(Item item) 
        {
            _itemRepository.AddItem(item);
            return RedirectToAction("AllItems");
        }

        public IActionResult AllItems() 
        { 
            List<Item> items = _itemRepository.GetAllItems();
            return View(items);
        }
    }
}
