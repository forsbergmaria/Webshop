using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StatisticsViewModel
    {
        public List<Item> TopSoldItems { get; set; }
        public Dictionary<int, int> SoldItemCountForEachItem { get; set; }
    }
}
