using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = Models.Size;

namespace DataAccess.Data.Repositories
{
    public class SizeRepository
    {
        public bool SizeExistsByName(string name)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    bool exists = false;
                    var existingSize = context.Sizes.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
                    if (existingSize != null)
                    {
                        exists = true;
                    }

                    return exists;
                }
            }
            catch (NullReferenceException)
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
                return context.Sizes
                    .Where(s => s.SizeId == id).FirstOrDefault();
            }
        }

        public Size GetSizeByName(string name)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return context.Sizes
                        .Where(s => s.Name == name).FirstOrDefault();
                        
                }
            }
            catch (NullReferenceException)
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

        //Removes a Size from the database
        public void DeleteSize(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var size = context.Sizes.FirstOrDefault(c => c.SizeId == id);
                context.Sizes.Remove(size);
                context.SaveChanges();
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
        
        //Modify existing size and save it async
        public async Task<Size> ModifySizeAsync(Size size)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(size).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return size;
                }
            }
            catch(NullReferenceException)
            {
                throw;
            }
            catch (SqlException ex) 
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
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

        //Creates a new size and saves it async
        public async void AddSizeAsync(Size size)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Sizes.Add(size);
                await context.SaveChangesAsync();
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
