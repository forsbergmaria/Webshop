using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerAddress2 { get; set; }
        public string? CustomerZipCode { get; set; }
        //public string CustomerEmail { get; set; }
        public string? CustomerCity { get; set; }
        public string? ShippingMethod { get; set; }
        public string? ShippingStatus { get; set; }
        public List<Item>? OrderedItems { get; set; }

    }
}
