using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ItemIndexView
    {
        public List<Item> Items { get; set; }
        public List<Subcategory>? Subcategories { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Size>? Sizes { get; set; }
    }
    public class ItemDetailsView
    {
        public Item? Item { get; set; }
        public List<Size>? Sizes { get; set; }
        public int Quantity { get; set; }
        public List<Image>? Images { get; set; }
    }
}
