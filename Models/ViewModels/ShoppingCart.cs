﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set;}
        public List<Item>? Items { get; set; }
        public int Quantity;

    }
}