using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HomeViewModel
    {
        public string UserFirstName { get; set; }
        public List<int> TotalSales { get; set; }
        public List<string> ItemsSold { get; set; }
        public int UnitsSold { get; set; }
    }
}
