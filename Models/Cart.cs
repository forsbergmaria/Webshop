using SwedbankPay.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models
{
    public class Cart
    {
        private List<CartLine> CartLineCollection { get; set; } = new List<CartLine>();

        [JsonInclude]
        public virtual IEnumerable<CartLine> CartLines
        {
            get => CartLineCollection;
            private set => CartLineCollection = value.ToList();
        }
        public string PaymentOrderLink { get; set; }
        public string PaymentLink { get; set; }
        public bool Vat { get; set; }
        public PaymentInstrument Instrument { get; set; }
        public string ConsumerUiScriptSource { get; set; }
        public string ConsumerProfileRef { get; set; }


        public virtual void AddItem(Item item, int quantity)
        {
            var line = CartLineCollection.FirstOrDefault(p => p.Item.ItemId == item.ItemId);

            if (line == null)
            {
                CartLineCollection.Add(new CartLine
                {
                    Item = item,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }


        public virtual decimal CalculateTotal()
        {
            return CartLineCollection.Sum(e => e.Quantity * e.Item.PriceWithoutVAT);
        }


        public virtual void Clear()
        {
            CartLineCollection.Clear();
        }


        public virtual void RemoveItem(Item item, int quantity)
        {
            var line = CartLineCollection.FirstOrDefault(p => p.Item.ItemId == item.ItemId);

            if (line != null)
            {
                if (quantity >= line.Quantity)
                {
                    CartLineCollection.Remove(line);
                }
                else
                {
                    line.Quantity -= quantity;
                }
            }
        }


        public virtual void Update()
        {
        }
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Item Item { get; set; }

        [Required(ErrorMessage = "Please provide a number greater than zero!")]
        [Display(Name = "Unit quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }


        public decimal CalculateTotal()
        {
            return Quantity * Item.PriceWithoutVAT;
        }
    }
}
