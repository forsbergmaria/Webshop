using Microsoft.EntityFrameworkCore;
using Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories
{
    public class SizeRepository
    {
        //Returns a list of Sizes from the database
        public List<Size> GetAllSizes()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Sizes.ToList();
            }
        }

        //Returns a specific Size from the database
        public Size GetSize(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Sizes.Include(i => i.Name)
                    .FirstOrDefault(i => i.SizeId == id);
            }
        }

        //Removes a Size from the database
        public bool DeleteSize(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var size = context.Sizes.FirstOrDefault(c => c.SizeId == id);
                var itemsInSize = context.Items.Where(i => i.ItemId == id).ToList();
                if (size != null && itemsInSize != null)
                {
                    return false;
                }
                else
                {
                    context.Sizes.Remove(size);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        //Modify existing Size
        public Size ModifySize(Size size)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(size).State = EntityState.Modified;
                context.SaveChanges();
                return size;
            }
        }

        //Creates a new Size
        public void AddSize(Size size)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Sizes.Add(size);
                context.SaveChanges();
            }
        }

        public List<Size> GetAllSizesForOneItem(int itemId)
        {
            using (var context = new ApplicationDbContext())
            {
                var sizes = context.Sizes
                    .Join(
                        context.ItemHasSize,
                        size => size.SizeId,
                        itemSize => itemSize.SizeId,
                        (size, itemSize) => new { size, itemSize }
                    )
                    .Where(s => s.itemSize.ItemId == itemId)
                    .Select(s => s.size)
                    .ToList();

                return sizes;
            }
        }

        public void AssignSizeToItem(Item item, int sizeId)
        {
            using (var context = new ApplicationDbContext())
            {
                ItemHasSize itemHasSize = new ItemHasSize();
                itemHasSize.ItemId = item.ItemId;
                itemHasSize.SizeId = sizeId;
                context.ItemHasSize.Add(itemHasSize);
                context.SaveChanges();
            }
        }

        // Deletes a specific row with SizeId and ItemId from the database, based on the sizeId
        public void DeleteSizeFromItemHasSize(int sizeId)
        {
            using (var context = new ApplicationDbContext())
            {
                var itemHasSizes = context.ItemHasSize
                    .Where(i => i.SizeId == sizeId).ToList();
                foreach (var size in itemHasSizes)
                {
                    context.ItemHasSize.Remove(size);
                }
                context.SaveChanges();
            }
        }
    }
}
