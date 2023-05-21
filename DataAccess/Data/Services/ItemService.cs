using Data;
using DataAccess.Data.Repositories;
using Models;
using Models.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace DataAccess.Data.Services
{
    public class ItemService
    {
        private ItemRepository itemRepository { get { return new ItemRepository(); } }
        private StatisticsRepository statisticsRepository { get { return new StatisticsRepository(); } }
        private CategoryRepository categoryRepository { get { return new CategoryRepository(); } }
        private SizeRepository sizeRepository { get { return new SizeRepository(); } }
        public List<Item> ItemTextSearch(string searchstring)
        {
            List<Item> items = itemRepository.GetAllItems();
            if(string.IsNullOrEmpty(searchstring))
            {
                return items;
            }
            else
            {
                var filteredItems = items.Where(c => c.Name.ToLower().Contains(searchstring.ToLower()) ||
                c.Brand.ToLower().Contains(searchstring.ToLower()) ||
                c.Category.Name.ToLower().Contains(searchstring.ToLower()) ||
                c.Description.ToLower().Contains(searchstring.ToLower()) ||
                c.Subcategory.Name != null && c.Subcategory.Name.ToLower().Contains(searchstring.ToLower()) ||
                c.Color != null && c.Color.ToLower().Contains(searchstring.ToLower())).ToList();
                return filteredItems;
            }
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

        public List<Item> SortItems(int sortOption)
        {
            try
            {
                List<Item> sortedList = new List<Item>();

                switch (sortOption)
                {
                    case 2:
                        sortedList = itemRepository.GetAllItems()
                            .OrderBy(i => i.PriceWithoutVAT * i.VAT)
                            .ToList();
                        break;

                    case 3:
                        sortedList = itemRepository.GetAllItems()
                            .OrderByDescending(i => i.PriceWithoutVAT * i.VAT)
                            .ToList();
                        break;

                    case 4:
                        sortedList = itemRepository.GetAllItems()
                            .OrderBy(i => i.Name)
                            .ToList();
                        break;

                    case 5:
                        sortedList = itemRepository.GetAllItems()
                            .OrderByDescending(i => i.Name)
                            .ToList();
                        break;

                    default:
                        int totalItems = itemRepository.CountItems();
                        sortedList = statisticsRepository.GetMostSoldItems(totalItems, DateTime.Now.AddDays(-14), DateTime.Now);
                        break;
                }

                return sortedList;
            }
            catch(Exception ex)
            {
                throw;
            }
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

        public ItemDetailsView GetDetailsView(int id)
        {
            Item item = itemRepository.GetItem(id);
            ItemDetailsView details = new ItemDetailsView();
            if (item.IsPublished == true)
            {
                if(item.HasSize == true)
                {
                    details.Sizes = sizeRepository.GetAllSizes();
                }
                if(item.ProductImages != null)
                {
                    details.Images = item.ProductImages.ToList();
                }
                details.Quantity = 1;
                details.Item = item;
            }
            
            return details;
        }

        public SelectCategoryModel GetItemsPerCategory (int id)
        {
            List<Item> items = itemRepository.GetAllItems().Where(i => i.CategoryId == id).ToList();
            List<Subcategory> subcategories = categoryRepository.GetAllSubcategories().Where(s => s.CategoryId == id).ToList();
            SelectCategoryModel model = new SelectCategoryModel
            {
                Items = items,
                Subcategories = subcategories
            };
            return model;
        }
    }
}
