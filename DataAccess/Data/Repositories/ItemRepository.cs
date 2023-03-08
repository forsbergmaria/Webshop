using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;

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
                return context.Items.ToList();
            }
        }

        //Returns a specific item from the database
        public Item GetItem(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Items.Include(i => i.ProductImages)
                    .Include(i => i.Description)
                    .Include(i => i.Brand).FirstOrDefault(i => i.ItemId == id);
            }
        }

        //Removes an item from the database
        public bool DeleteItem(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var item = context.Items.FirstOrDefault(c => c.ItemId == id);
                if (item != null)
                {
                    return false;
                }
                else
                {
                    context.Items.Remove(item);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        //Modify existing item
        public Item ModifyItem (Item item)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(item).State = EntityState.Modified;
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
            }
        }

        //Gets sizes of specific item

    }
}