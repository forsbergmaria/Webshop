using Models;
using SwedbankPay.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models.ViewModels
{
    public class ShoppingCart
    {
        public List<Item>? Items { get; set; }
        public int Quantity;
        public Dictionary<int, int>? ItemQuantity;
        public Dictionary<int, Dictionary<string, int>>? ItemSize;
        public decimal? Total;
    }

    public class ShoppingCartItem
    {
        //public List<int>? Ids { get; set; }
        public int Id { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
}