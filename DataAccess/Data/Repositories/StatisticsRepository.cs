using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories
{
    public class StatisticsRepository
    {
        // Gets the top five most sold items, based on a specific start and end date
        public List<Item> GetTopFiveMostSoldItems(DateTime startDate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var topFiveItems = context.Items
                    .Join(context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate),
                        item => item.ItemId,
                        transaction => transaction.ItemId,
                        (item, transaction) => new { Item = item, Quantity = transaction.Quantity })
                    .GroupBy(x => x.Item)
                    .OrderByDescending(g => g.Sum(x => x.Quantity))
                    .Take(5)
                    .Select(g => g.Key)
                    .AsEnumerable()
                    .ToList();

                return topFiveItems;
            }
        }

        // Returns a list of Items representing the top five most sold items
        public Dictionary<int, int> GetSoldItemCountForEachItem()
        {
            using (var context = new ApplicationDbContext())
            {
                var soldTransactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Where(t => t.Quantity > 0).ToList();

                var soldItemCountDict = new Dictionary<int, int>();

                foreach (var transaction in soldTransactions)
                {
                    if (soldItemCountDict.ContainsKey(transaction.ItemId))
                    {
                        soldItemCountDict[transaction.ItemId] += transaction.Quantity;
                    }
                    else
                    {
                        soldItemCountDict[transaction.ItemId] = transaction.Quantity;
                    }
                }

                return soldItemCountDict;
            }
        }

        // Returns an integer representing total sales for a specific timespan
        public int GetTotalSalesByDate(DateTime startDate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var transactions = context.ItemTransactions.Where(t => t.TransactionDate.Date >= startDate
                && t.TransactionDate.Date <= endDate).ToList();

                return transactions.Count();
            }
        }

        // Returns an integer representing total sales since start
        public int GetTotalSalesSinceStart()
        {
            using (var context = new ApplicationDbContext())
            {
                var transactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning").ToList();
                return transactions.Count();
            }
        }
    }
}
