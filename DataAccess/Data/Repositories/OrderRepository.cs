using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;
using Stripe;
using Stripe.Checkout;

namespace Data
{
    public class OrderRepository
    {
        ItemRepository _itemRepository { get { return new ItemRepository(); } }

        //Checks if any orders exist in the database
        public bool OrdersExist()
        {
            using (var context = new ApplicationDbContext())
            {
                var allOrders = context.Orders.ToList();

                if (allOrders.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //Returns a list of orders from the database
        public List<Order> GetAllOrders()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Orders.ToList();
            }
        }

        //Returns a specific order from the database
        public Order GetOrder(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Orders.Include(o => o.CustomerPhone)
                    .Include(o => o.CustomerCity)
                    .Include(o => o.CustomerZipCode)
                    .Include(o => o.CustomerName)
                    .Include(o => o.CustomerAddress)
                    .FirstOrDefault(i => i.OrderId == id);
            }
        }

        //Removes an order from the database
        public bool DeleteOrder(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var order = context.Orders.FirstOrDefault(c => c.OrderId == id);
                if (order != null)
                {
                    return false;
                }
                else
                {
                    context.Orders.Remove(order);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        //Modify existing order
        public Order ModifyOrder(Order order)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(order).State = EntityState.Modified;
                context.SaveChanges();
                return order;
            }
        }

        //Creates a new order
        public void CreateOrder(Session session, StripeList<LineItem> lineItems)
        {
            using (var context = new ApplicationDbContext())
            {
                Order order = new Order();
                order.OrderDate = session.Created;
                order.CustomerName = session.ShippingDetails.Name;
                order.CustomerAddress = session.ShippingDetails.Address.Line1;
                order.CustomerAddress2 = session.ShippingDetails.Address.Line2;
                order.CustomerPhone = session.ShippingDetails.Phone;
                order.CustomerCity = session.ShippingDetails.Address.City;
                order.CustomerZipCode = session.ShippingDetails.Address.PostalCode;
                context.Orders.Add(order);
                context.SaveChanges();

                foreach (var item in lineItems)
                {
                    var orderedItem = new OrderContainsItem();
                    orderedItem.OrderId = order.OrderId;
                    orderedItem.StripeItemId = item.Price.ProductId;
                    var theItem = context.Items.Where(i => i.StripeItemId.Equals(item.Price.ProductId)).FirstOrDefault();
                    orderedItem.ItemId = theItem.ItemId;
                    orderedItem.ItemQuantity = item.Quantity;
                    orderedItem.Total = item.AmountTotal / 100;
                    context.OrderContainsItem.Add(orderedItem);
                    if (theItem.HasSize == false)
                    {
                        _itemRepository.AddItemTransaction(theItem, "Försäljning", (int)item.Quantity, null);
                    }
                    else
                    {
                        var metaDataValue = GetSizeFromStripe(item.Price.ProductId);
                        _itemRepository.AddItemTransaction(theItem, "Försäljning", (int)item.Quantity, metaDataValue);
                    }
                    
                }

                context.SaveChanges();
            }
        }

        public string GetSizeFromStripe(string productId)
        {
            StripeConfiguration.ApiKey = "sk_test_51MnVSuJ9NmDaISNLt2DpWzyfEpec4JZF1Zf9gwPkecoDj2OYmXX9ThWfvXB2nEbadLp51BI6AuooidYslZ6yykDg00pjXolXbJ";

            var productService = new ProductService();

            var productGetOptions = new ProductGetOptions
            {
                Expand = new List<string> { "metadata" }
            };

            var product = productService.Get(productId, productGetOptions);

            var metadataValue = product.Metadata["Size"];

            return metadataValue;

        }
    }
}
