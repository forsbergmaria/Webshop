using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SubcategoryViewModel
    {
        public Category Category { get; set; }
        public List<Subcategory> Subcategories { get; set; }
    }
}
