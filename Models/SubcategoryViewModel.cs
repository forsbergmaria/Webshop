using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SubcategoryViewModel
    {
        public string CategoryName { get; set; }
        public List<Subcategory> Subcategories { get; set; }
    }
}
