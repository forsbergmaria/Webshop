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

        public ActionResult Index(string searchstring)
        {
            var items = itemService.ItemTextSearch(searchstring);
            return View(items);
        }
        public IActionResult Details(int id)
        {
            var item = itemService.GetDetailsView(id);
            return View(item);
        }

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
