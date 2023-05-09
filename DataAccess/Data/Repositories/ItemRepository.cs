using DataAccess;
using DataAccess.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModels;
using Stripe;
using System.Web.Mvc;
using System.Web.WebPages;
using File = System.IO.File;

namespace Data
{
    public class ItemRepository
    {
        SizeRepository _sizeRepository { get { return new SizeRepository(); } }

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
                    .Include(i => i.ProductImages)
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

                 if (item.HasSize)
                    {
                      DeleteItemFromItemHasSize(id);
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

        // Removes all belonging images from the database
        public void DeleteAllImagesFromItem(int itemId)
        {
            using (var context = new ApplicationDbContext())
            {
                var images = context.Images.Where(i => i.ItemId == itemId).ToList();

                foreach (var image in images)
                {
                    context.Images.Remove(image);
                }
                context.SaveChanges();
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

        // Returns a specific image
        public Image GetImageById(int imageId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Images.Where(i => i.ImageId == imageId).First();
            }
        }

        // Deletes a specific row with SizeId and ItemId from the database, based on the itemId
        public void DeleteItemFromItemHasSize(int itemId) 
        { 
            using (var context = new ApplicationDbContext())
            {
                var itemHasSizes = context.ItemHasSize
                    .Where(i => i.ItemId == itemId).ToList();
                foreach (var size in itemHasSizes)
                {
                    context.ItemHasSize.Remove(size);
                }
                context.SaveChanges();
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
                var sizesList = new List<Size>();
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

        // Adds an item to the Stripe Dashboard through Stripe API
        public void AddItemToStripe(Item item)
        {
            using (var context = new ApplicationDbContext())
            {
				StripeConfiguration.ApiKey = "sk_test_51MnVSuJ9NmDaISNLt2DpWzyfEpec4JZF1Zf9gwPkecoDj2OYmXX9ThWfvXB2nEbadLp51BI6AuooidYslZ6yykDg00pjXolXbJ";

				var imageLinks = new List<string>();
				foreach (var image in item.ProductImages)
				{
					imageLinks.Add(image.Path);
				}
               
                    var productOptions = new ProductCreateOptions
                    {
                        Name = item.Name,
                        Description = item.Description,
                    };
                

				var productService = new ProductService();
				Product product = productService.Create(productOptions);

				var priceOptions = new PriceCreateOptions
				{
					Product = product.Id,
					UnitAmount = (long)((item.PriceWithoutVAT * item.VAT * 100)),
					Currency = "sek",
				};

				var priceService = new PriceService();
				Price price = priceService.Create(priceOptions);

                item.StripeItemId = product.Id;
                item.StripePriceId = price.Id;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
			}
        }

        // Returns a list of images from a specific item
        public List<Image> GetImagesByItemId(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var images = context.Images.Where(i => i.ItemId == id).ToList();
                return images;
            }
        }

        // Adds an item transaction to the database
        public void AddItemTransaction(Item item, string transactionType, int quantity, string? sizeName)
        {
            using (var context = new ApplicationDbContext())
            {
                if (item.HasSize == false)
                {
                    var transaction = new ItemTransaction
                    {
                        ItemId = item.ItemId,
                        Quantity = quantity,
                        TransactionType = transactionType,
                        TransactionDate = DateTime.Now
                    };

                    context.ItemTransactions.Add(transaction);
                    context.SaveChanges();
                }
                else
                {
                    
                    var size = context.Sizes.Where(s => s.Name == sizeName).FirstOrDefault();
                    var transaction = new TransactionWithSizes
                    {
                        ItemId = item.ItemId,
                        Quantity = quantity,
                        TransactionType = transactionType,
                        TransactionDate = DateTime.Now,
                        SizeId = size.SizeId
                    };

                    context.ItemTransactions.Add(transaction);
                    context.SaveChanges();
                }
            }
        }
    }
}