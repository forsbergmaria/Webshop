using Data;
using Models;
using Models.ViewModels;
using System.Web.Mvc;

namespace DataAccess.Data.Services
{
    public class ItemService
    {
        private ItemRepository itemRepository { get { return new ItemRepository(); } }
        private CategoryRepository categoryRepository { get { return new CategoryRepository(); } }
        public List<Item> SearchByCategory(string searchString)
        {
            List<Item> listofitems = itemRepository.GetAllItems();

            if (!String.IsNullOrEmpty(searchString))
            {
                listofitems = listofitems.FindAll(c => c.Category.Name.ToLower()
                .Contains(searchString.ToLower())).ToList();
            }

            return listofitems;
        }

        public List<SelectListItem> PopulateCategoryList()
        {
            var allCategories = categoryRepository.GetAllCategories();

            List<SelectListItem> categories = allCategories.ConvertAll(c =>
            {
                return new SelectListItem()
                {
                    Text = c.Name.ToString(),
                    Value = c.CategoryId.ToString(),
                    Selected = false
                };
            });

            return categories;
        }

        public ItemIndexView GetViewModel()
        {
            var items = itemRepository.GetAllItems();
            var categories = categoryRepository.GetAllCategories();
            var subs = categoryRepository.GetAllSubcategories();
            ItemIndexView viewmodel = new ItemIndexView();

            foreach(var item in items)
            {
                viewmodel.Items.Add(item);
            }
            foreach (var category in categories)
            {
                viewmodel.Categories.Add(category);
            }
            foreach (var sub in subs)
            {
                viewmodel.Subcategories.Add(sub);
            }
            return viewmodel;
        }
    }
}
