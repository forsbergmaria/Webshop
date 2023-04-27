using SwedbankPay.Sdk.PaymentOrders;
using SwedbankPay.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Models.Extensions
{
    public static class CartLineExtensions
    {
        public static IEnumerable<IOrderItem> ToOrderItems(this IEnumerable<CartLine> lines)
        {
            foreach (var line in lines)
            {
                yield return new OrderItem(line.Item.ItemId.ToString(), line.Item.Name, OrderItemType.FromValue(line.Item.Category.Name),
                                           line.Item.Class,
                                           line.Quantity, "st", new Amount(line.Item.PriceWithoutVAT), 0,
                                           new Amount(line.CalculateTotal()),
                                           new Amount(line.Item.VAT == 0
                                                                  ? 0
                                                                  : line.CalculateTotal() * line.Item.VAT
                                                                    / 100));
            }
        }
    }
}
