using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class SelectCategoryModel
    {
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
