using Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace DataAccess.Data.Repositories
{
    public class StatisticsRepository
    {
        ItemRepository _itemRepository { get { return new ItemRepository(); } }


        // Gets the top n most sold items, based on a specific start and end date
        // "quantity" determines how many items to be returned
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

        // Returns a dictionary with the total number of items sold for each item that has been sold at least once. 
        // The method retrieves all sold transactions from the database, and then uses a dictionary to keep track of the sold item counts. 
        // It then looks up each item in the Items table to get its corresponding name and adds the item and its count to the dictionary. 
        // Finally, it returns the resulting dictionary with each item and its sold count
        public Dictionary<Item, int> GetMostSoldItems(int quantity, DateTime startDate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var soldTransactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning"
                    && t.TransactionDate.Date >= startDate && t.TransactionDate < endDate)
                    .Where(t => t.Quantity > 0).Take(quantity).ToList();

                var soldItemCountDict = new Dictionary<Item, int>();

                foreach (var transaction in soldTransactions)
                {
                    var item = _itemRepository.GetItem(transaction.ItemId);

                    if (item != null)
                    {
                        if (soldItemCountDict.ContainsKey(item))
                        {
                            soldItemCountDict[item] += transaction.Quantity;
                        }
                        else
                        {
                            soldItemCountDict[item] = transaction.Quantity;
                        }
                    }
                }

                return soldItemCountDict;
            }
        }

        // Returns a dictionary with the total number of items sold for each item that has been sold at least once. 
        // The method retrieves all sold transactions from the database, and then uses a dictionary to keep track of the sold item counts. 
        // It then looks up each item in the Items table to get its corresponding name and adds the item and its count to the dictionary. 
        // Finally, it returns the resulting dictionary with each item and its sold count
        public Dictionary<Item, int> GetLeastSoldItems(int quantity, DateTime startDate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var soldTransactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning"
                    && t.TransactionDate.Date >= startDate && t.TransactionDate < endDate)
                    .Where(t => t.Quantity > 0).Take(quantity).ToList();

                var soldItemCountDict = new Dictionary<Item, int>();

                foreach (var transaction in soldTransactions)
                {
                    var item = _itemRepository.GetItem(transaction.ItemId);

                    if (item != null)
                    {
                        if (soldItemCountDict.ContainsKey(item))
                        {
                            soldItemCountDict[item] -= transaction.Quantity;
                        }
                        else
                        {
                            soldItemCountDict[item] = -transaction.Quantity;
                        }
                    }
                }

                // Sorterar dictionary efter minsta till största försäljningsantal
                var sortedDict = soldItemCountDict.OrderBy(x => x.Value);

                return sortedDict.ToDictionary(x => x.Key, x => -x.Value);
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
        public decimal GetTotalSalesForItem(int productId)
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
                        decimal salesAmount = (decimal)(transaction.Quantity * item.PriceWithoutVAT);
                        totalSales += salesAmount;
                    }
                }

                return totalSales;
            }
        }

        // Returns a list of all items that never have been sold
        public List<Item> GetAllItemsNeverSold()
        {
            using (var context = new ApplicationDbContext())
            {
                var soldItems = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Select(t => t.ItemId).Distinct().ToList();

                var unsoldItems = _itemRepository.GetAllItems().Where(i => !soldItems.Contains(i.ItemId) && i.IsPublished == true).ToList();

                return unsoldItems;
            }
        }

        // Returns a list of top x items that never have been sold
        public List<Item> GetTopItemsThatHaveNeverBeenSold(int quantity)
        {
            using (var context = new ApplicationDbContext())
            {
                var soldItems = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Select(t => t.ItemId).Distinct().ToList();

                var unsoldItems = _itemRepository.GetAllItems().Where(i => !soldItems.Contains(i.ItemId) && i.IsPublished == true).Take(quantity).ToList();

                return unsoldItems;
            }
        }





    }
}
