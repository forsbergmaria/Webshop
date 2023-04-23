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
        // Gets the top n most sold items, based on a specific start and end date
        public List<Item> GetTopMostSoldItems(int quantity, DateTime startDate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var topItems = context.Items
                    .Join(context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Where(t => t.TransactionDate.Date >= startDate && t.TransactionDate.Date <= endDate),
                        item => item.ItemId,
                        transaction => transaction.ItemId,
                        (item, transaction) => new { Item = item, Quantity = transaction.Quantity })
                    .GroupBy(x => x.Item)
                    .OrderByDescending(g => g.Sum(x => x.Quantity))
                    .Take(quantity)
                    .Select(g => g.Key)
                    .AsEnumerable()
                    .ToList();

                return topItems;
            }
        }

        // Gets the top n most sold items, based on a specific start and end date
        public List<Item> GetTopLeastSoldItems(int quantity, DateTime startDate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var topItems = context.Items
                    .Join(context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Where(t => t.TransactionDate.Date >= startDate && t.TransactionDate.Date <= endDate),
                        item => item.ItemId,
                        transaction => transaction.ItemId,
                        (item, transaction) => new { Item = item, Quantity = transaction.Quantity })
                    .GroupBy(x => x.Item)
                    .OrderBy(g => g.Sum(x => x.Quantity))
                    .Take(quantity)
                    .Select(g => g.Key)
                    .AsEnumerable()
                    .ToList();

                return topItems;
            }
        }

        // Returns a list of Items representing the top five most sold items
        public Dictionary<int, int> GetMostSoldItemCountForEachItem()
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

        public List<int> GetLeastSoldItemCountForEachItem()
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

                var leastSoldItemCount = soldItemCountDict.Min(kvp => kvp.Value);
                var leastSoldItems = soldItemCountDict.Where(kvp => kvp.Value == leastSoldItemCount)
                                                     .Select(kvp => kvp.Key)
                                                     .ToList();

                return leastSoldItems;
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

        // Returns an integer representing a total number of sales since start
        public Tuple<int, decimal> GetTotalSalesSinceStart()
        {
            using (var context = new ApplicationDbContext())
            {
                var soldTransactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning").ToList();

                int totalTransactions = soldTransactions.Count();
                decimal totalSales = 0;

                foreach (var transaction in soldTransactions)
                {
                    var item = context.Items.FirstOrDefault(i => i.ItemId == transaction.ItemId);
                    if (item != null)
                    {
                        totalSales += transaction.Quantity * item.PriceWithoutVAT;
                    }
                }

                return new Tuple<int, decimal>(totalTransactions, totalSales);
            }
        }

        // Calculates total VAT revenues for all items, for a specific timespan
        public decimal GetTotalVATSales(DateTime startdate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var transactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Where(t => t.TransactionDate.Date >= startdate && t.TransactionDate.Date <= endDate).ToList();
                decimal totalVATSales = 0;

                foreach (var transaction in transactions)
                {
                    var item = context.Items.FirstOrDefault(i => i.ItemId == transaction.ItemId);
                    if (item != null)
                    {
                        decimal VATAmount = transaction.Quantity * (item.PriceWithoutVAT * item.VAT);
                        totalVATSales += VATAmount;
                    }
                }

                return totalVATSales;
            }
        }

        // Calculates total VAT revenues for all items since start
        public decimal GetTotalVATSalesSinceStart()
        {
            using (var context = new ApplicationDbContext())
            {
                var transactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning").ToList();
                decimal totalVATSales = 0;

                foreach (var transaction in transactions)
                {
                    var item = context.Items.FirstOrDefault(i => i.ItemId == transaction.ItemId);
                    if (item != null)
                    {
                        // Beräkna momsintäkter för varje transaktion
                        decimal VATAmount = transaction.Quantity * (item.PriceWithoutVAT * item.VAT);
                        totalVATSales += VATAmount;
                    }
                }

                return totalVATSales;
            }
        }

        // Calculates total VAT revenues for a specific item since start
        public decimal GetTotalVATSalesForItemSinceStart(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var transactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning").ToList();
                decimal totalVATSales = 0;

                foreach (var transaction in transactions)
                {
                    var item = context.Items.FirstOrDefault(i => i.ItemId == transaction.ItemId);
                    if (item != null)
                    {
                        // Beräkna momsintäkter för varje transaktion
                        decimal VATAmount = transaction.Quantity * (item.PriceWithoutVAT * item.VAT);
                        totalVATSales += VATAmount;
                    }
                }

                return totalVATSales;
            }
        }

        // Calculates total VAT revenues for a specific item, for a specific timespan
        public decimal GetTotalVATSalesForItem(int id, DateTime startdate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var transactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Where(t => t.TransactionDate.Date >= startdate && t.TransactionDate.Date <= endDate).ToList();
                decimal totalVATSales = 0;

                foreach (var transaction in transactions)
                {
                    var item = context.Items.FirstOrDefault(i => i.ItemId == transaction.ItemId);
                    if (item != null)
                    {
                        // Beräkna momsintäkter för varje transaktion
                        decimal VATAmount = transaction.Quantity * (item.PriceWithoutVAT * item.VAT);
                        totalVATSales += VATAmount;
                    }
                }

                return totalVATSales;
            }
        }

        // Retrieves total sales amount for a specific item by summing up the sales from all transactions
        public decimal GetTotalSalesForProduct(int productId)
        {
            using (var context = new ApplicationDbContext())
            {
                var transactions = context.ItemTransactions
                    .Where(t => t.TransactionType == "Försäljning" && t.ItemId == productId)
                    .ToList();
                decimal totalSales = 0;

                foreach (var transaction in transactions)
                {
                    var item = context.Items.FirstOrDefault(i => i.ItemId == transaction.ItemId);
                    if (item != null)
                    {
                        // Beräkna försäljningssumma för varje transaktion
                        decimal salesAmount = transaction.Quantity * item.PriceWithoutVAT;
                        totalSales += salesAmount;
                    }
                }

                return totalSales;
            }
        }




    }
}
