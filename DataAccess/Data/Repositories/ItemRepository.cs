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

        // Returns a specific image
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

        // Returns a list of images from a specific item
        public List<Image> GetImagesByItemId(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var images = context.Images.Where(i => i.ItemId == id).ToList();
                return images;
            }
        }
    }
}