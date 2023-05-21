using Data;
using DataAccess.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;

namespace Webshop.Controllers
{
    public class ItemController : Controller
    {
        ItemRepository itemRepository { get { return new ItemRepository(); } }
        ItemService itemService { get { return new ItemService(); } }
        CategoryRepository categoryRepository { get { return new CategoryRepository(); } }
        IHttpContextAccessor _ca => new HttpContextAccessor();
        ShoppingCartManager _cm { get { return new ShoppingCartManager(_ca); } }

        public ActionResult Index(string searchstring)
        {
            var items = itemService.ItemTextSearch(searchstring);
            ViewBag.Searchstring = searchstring;
            return View(items);
        }

        public IActionResult AllItems()
        {
            List<Item> allItems = itemRepository.GetAllItems();
            return View(allItems);
        }

        [HttpPost]
        public IActionResult SortItems(int sortOption)
        {
            List<Item> sortedItems = itemService.SortItems(sortOption);
            return View("AllItems", sortedItems);
        }
        public IActionResult Details(int id)
        {
            var item = itemService.GetDetailsView(id);
            var cartItems = _cm.GetCartItems();
            ViewBag.Quantity = cartItems.Quantity;
            return View(item);
        }

        //public ActionResult Category(int id)
        //{
        //    var items = itemService.GetItemsPerCategory(id);
        //    //typ get categoryselectmodel
        //    return View(items);
        //}

        public IActionResult Filter(SelectCategoryModel viewmodel)
        {
            viewmodel.Categories = categoryRepository.GetAllCategories();

            if (viewmodel.CategoryId != 0)
            {
                viewmodel.Items = itemRepository.GetAllItems()
                    .Where(i => i.CategoryId == viewmodel.CategoryId);
            }

            return View(viewmodel);
        }
    }
}
