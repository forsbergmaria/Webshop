using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Data
{
    public class ItemRepository
    {

        //Checks if any Items exist in the database
        public bool ItemsExist()
        {
            using (var context = new ApplicationDbContext())
            {
                var allItems = context.Items.ToList();

                if (allItems.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //Returns a list of items from the database
        public List<Item> GetAllItems()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Items
                    .Include(c => c.Category).
                    Include(i => i.ProductImages).
                    Include(c => c.Subcategory).ToList();
            }
        }

        //Returns a specific item from the database
        public Item GetItem(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Items
                    .Include(i => i.Category)
                    .Include(i => i.Subcategory)
                    .FirstOrDefault(i => i.ItemId == id);
            }
        }

        //Removes an item and it's images from the database
        public bool DeleteItem(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var images = context.Images.Where(i => i.ItemId == id).ToList();
                var item = context.Items.FirstOrDefault(c => c.ItemId == id);
                if (item == null)
                {
                    return false;
                }
                else
                {   
                    foreach (var image in images)
                    {
                        context.Images.Remove(image);
                    }
                    context.Items.Remove(item);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        // Deletes a specific image from the directory
        public void DeleteImageFromDirectory(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        // Deletes a specific image from the database
        public void DeleteImageFromDB(int imageId)
        {
            using (var context = new ApplicationDbContext())
            {
                var image = context.Images.Where(i => i.ImageId == imageId).First();
                context.Images.Remove(image);
                context.SaveChanges();
            }
        }

        public Image GetImageById(int imageId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Images.Where(i => i.ImageId == imageId).First();
            }
        }

        //Modify existing item
        public Item ModifyItem (Item item)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();

                var items = item.ProductImages.ToList();
                foreach (var productImage in items)
                {
                    var img = new Image
                    {
                        ItemId = item.ItemId,
                        Path = productImage.Path
                    };

                    item.ProductImages.Add(img);
                }
                context.SaveChanges();
                return item;
            }
        }


        //Creates a new item
        public void AddItem (Item item)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Items.Add(item);
                context.SaveChanges();

                var items = item.ProductImages.ToList();
                foreach (var productImage in items)
                {
                    var img = new Image
                    {
                        ItemId = item.ItemId,
                        Path = productImage.Path
                    };

                    item.ProductImages.Add(img);
                }   
                context.SaveChanges();
            }
        }

        public List<Image> GetImagesByItemId(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var images = context.Images.Where(i => i.ItemId == id).ToList();
                return images;
            }
        }

        public int GetTotalSalesByDate(DateTime startDate, DateTime endDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var transactions = context.ItemTransactions.Where(t => t.TransactionDate.Date >= startDate
                && t.TransactionDate.Date <= endDate).ToList();

                return transactions.Count();
            }
        }

        public List<Item> GetTopFiveMostSoldItems()
        {
            using (var context = new ApplicationDbContext())
            {
                var topFiveItems = context.Items
                    .Join(context.ItemTransactions.Where(t => t.TransactionType == "Försäljning"),
                        item => item.ItemId,
                        transaction => transaction.ItemId,
                        (item, transaction) => new { Item = item, Quantity = transaction.Quantity })
                    .GroupBy(x => x.Item)
                    .OrderByDescending(g => g.Sum(x => x.Quantity))
                    .Take(5)
                    .Select(g => g.Key)
                    .AsEnumerable() // Hämtar data från databasen och konverterar det till en samling på klienten
                    .ToList(); // Konverterar samlingen till en lista

                return topFiveItems;
            }
        }

        public Dictionary<int, int> GetSoldItemCountForEachItem()
        {
            using (var context = new ApplicationDbContext())
            {
                // Hämta sålda transaktioner från databasen
                var soldTransactions = context.ItemTransactions.Where(t => t.TransactionType == "Försäljning")
                    .Where(t => t.Quantity > 0).ToList();

                // Skapa en dictionary för att hålla reda på antalet sålda produkter per ItemId
                var soldItemCountDict = new Dictionary<int, int>();

                // Iterera över varje såld transaktion och räkna antalet sålda produkter för varje ItemId
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