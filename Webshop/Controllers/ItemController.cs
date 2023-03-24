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
        CategoryRepository categoryRepository { get { return new CategoryRepository(); } }

        FilterService filterService { get { return new FilterService(); } }
        public IActionResult Index()
        {
            var items = itemRepository.GetAllItems();
            return View(items);
        }
        public IActionResult Details(int id)
        {
            var item = itemRepository.GetItem(id);
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
