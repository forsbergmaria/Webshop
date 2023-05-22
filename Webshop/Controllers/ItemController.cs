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

        public ActionResult Index(string searchstring, string color, string brand)
        {
            // Return items containing "searchstring" from the method ItemTextSearch
            var items = itemService.ItemTextSearch(searchstring);

            // Apply additional filters based on color and type
            if (!string.IsNullOrEmpty(color))
            {
                items = items.Where(item => item.Color == color).ToList();
            }

            if (!string.IsNullOrEmpty(brand))
            {
                items = items.Where(item => item.Brand == brand).ToList();
            }

            // Set the filter values in ViewBag for display in the view
            ViewBag.Searchstring = searchstring;
            ViewBag.Color = color;
            ViewBag.Brand = brand;

            return View(items);
        }

        public IActionResult Details(int id)
        {
            var item = itemService.GetDetailsView(id);
            return View(item);
        }

        public ActionResult Category(int id, int subId)
        {
            var items = itemService.GetItemsPerCategory(id);
            //typ get categoryselectmodel
            if(subId != 0)
            {
                items.Items = itemService.GetItemsPerSubcategory(subId);
            }
            return View(items);
        }

        public IActionResult Filter(SelectCategoryModel viewmodel)
        {
            //Get all categories to display
            viewmodel.Categories = categoryRepository.GetAllCategories();

            //If categories exist
            if (viewmodel.CategoryId != 0)
            {
                //Get all the items from selected category
                viewmodel.Items = itemRepository.GetAllItems()
                    .Where(i => i.CategoryId == viewmodel.CategoryId);
            }

            return View(viewmodel);
        }
    }
}
