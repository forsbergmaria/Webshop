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

        public Size GetSizeByName(string name)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Sizes.Include(i => i.Name)
                    .FirstOrDefault(i => i.Name == name);
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
    }
}
