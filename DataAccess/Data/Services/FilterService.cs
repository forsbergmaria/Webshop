using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataAccess.Data.Services
{
    public class FilterService
    {
        private ItemRepository itemRepository { get { return new ItemRepository(); } }
        private CategoryRepository categoryRepository { get { return new CategoryRepository(); } }

        public List<SelectListItem> PopulateDropDownList()
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
    }
}
