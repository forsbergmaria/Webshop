using Data;
using DataAccess.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Webshop.Controllers
{
    public class ItemController : Controller
    {
        ItemRepository itemRepository { get { return new ItemRepository(); } }
        CategoryRepository categoryRepository { get { return new CategoryRepository(); } }
        public IActionResult Index()
        {
            var items = itemRepository.GetAllItems();
            return View(items);
        }
    }
}
