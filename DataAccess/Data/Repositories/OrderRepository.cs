using DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Models;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;

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

        // Returns a list of Items from an order
        public List<Item> GetAllOrderItems(int orderId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    List<Item> items = context.Items.ToList();
                    List<Item> orderedItems = new List<Item>();
                    var order = context.OrderContainsItem.ToList();

                    foreach (var o in order)
                    {
                        foreach (var i in items)
                        {
                            if (i.ItemId == o.ItemId)
                            {
                                orderedItems.Add(i);
                            }
                        }
                    }

                    return orderedItems;
                }
            }
            catch (NullReferenceException ex)
            {
                throw;
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
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
                return context.Orders
                    .Where(o => o.OrderId == id).FirstOrDefault();
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
                StripeConfiguration.ApiKey = "sk_test_51MnVSuJ9NmDaISNLt2DpWzyfEpec4JZF1Zf9gwPkecoDj2OYmXX9ThWfvXB2nEbadLp51BI6AuooidYslZ6yykDg00pjXolXbJ";

                Order order = new Order();
                order.OrderDate = session.Created;
                order.CustomerName = session.ShippingDetails.Name;
                order.CustomerAddress = session.ShippingDetails.Address.Line1;
                order.CustomerAddress2 = session.ShippingDetails.Address.Line2;
                order.CustomerPhone = session.ShippingDetails.Phone;
                order.CustomerCity = session.ShippingDetails.Address.City;
                order.CustomerZipCode = session.ShippingDetails.Address.PostalCode;

                var shippingStatus = context.ShippingStatuses.Where(s => s.Name == "Ohanterad").FirstOrDefault();

                order.ShippingMethodId = session.ShippingCost.ShippingRateId;
                order.ShippingStatusId = shippingStatus.StatusId;
                
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
                        var productService = new ProductService();
                        var product = productService.Get(item.Price.ProductId);
                        var metadata = product.Metadata["Size"];
                        Debug.WriteLine(metadata);
                        _itemRepository.AddItemTransaction(theItem, "Försäljning", (int)item.Quantity, metadata);
                    }
                    
                }

                context.SaveChanges();
            }
        }

        public string GetShippingMethodName(string shippingRateId)
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51MnVSuJ9NmDaISNLt2DpWzyfEpec4JZF1Zf9gwPkecoDj2OYmXX9ThWfvXB2nEbadLp51BI6AuooidYslZ6yykDg00pjXolXbJ";

                var service = new ShippingRateService();
                var shippingMethod = service.Get(shippingRateId);
                var shippingMethodName = shippingMethod.DisplayName;

                return shippingMethodName;
            }
            catch (NullReferenceException ex)
            {
                throw;
            }
            catch (StripeException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetShippingStatusName(int statusId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var status = context.ShippingStatuses
                        .Where(s => s.StatusId == statusId).FirstOrDefault();
                    return status.Name;
                }
            }
            catch (NullReferenceException ex)
            {
                throw;
            }
            catch (StripeException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
