using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StatisticsViewModel
    {
        public Item? Item { get; set; }
        public List<Item>? TopMostSoldItems { get; set; }
        public List<Item>? TopLeastSoldItems { get; set; }
        public Dictionary<int, int>? SoldMostItemCountForEachItem { get; set; }
        public List<int>? GetLeastSoldItemCountForEachItem { get; set; }
        public int? NumberOfSales { get; set; }
        public decimal? TotalVATSales { get; set; }
        public decimal? TotalSales { get; set; }

    }
}
