using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class OrderRepository
    {
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
                    .Include(o => o.CustomerFirstName)
                    .Include(o => o.CustomerLastName)
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
        public void CreateOrder(Order order)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

    }
}
