using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class StatisticsViewModel
    {
        public Item? Item { get; set; }
        public List<Item>? TopMostSoldItems { get; set; }
        public List<Item>? TopLeastSoldItems { get; set; }
        public Dictionary<Item, int>? MostSoldItemCountForEachItem { get; set; }
        public Dictionary<Item, int>? LeastSoldItemCountForEachItem { get; set; }
        public Tuple<int, decimal>? NumberOfSales { get; set; }
        public List<Item>? ItemsNeverSold { get; set; }
        public decimal? TotalVATSales { get; set; }
        public decimal? TotalSales { get; set; }

    }
}
