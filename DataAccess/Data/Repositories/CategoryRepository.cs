using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class CategoryRepository
    {

        //Returns a list of categories from the database
        public List<Category> GetAllCategories()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Categories.ToList();
            }
        }

        //Returns a specific category from the database
        public Category GetCategory(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Categories
                    .Include(c => c.Subcategories)
                    .FirstOrDefault(i => i.CategoryId == id);
            }
        }

        //Removes a category from the database
        public bool DeleteCategory(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var category = context.Categories.FirstOrDefault(c => c.CategoryId == id);
                var itemsInCategory = context.Items.Where(i => i.CategoryId == id).ToList();
                if (category != null && itemsInCategory != null)
                {
                    return false;
                }
                else
                {
                    context.Categories.Remove(category);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        //Modify existing category
        public Category ModifyCategory(Category category)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
                return category;
            }
        }

        //Creates a new category
        public void AddCategory(Category category)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        //Creates a new subcategory
        public void AddSubcategory(Subcategory subcategory, int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var subcat = new Subcategory
                {
                    CategoryId = id,
                    Name = subcategory.Name
                };
                context.Subcategories.Add(subcategory);
                context.SaveChanges();
            }
        }

        //Returns a list of subcategories from the database
        public List<Subcategory> GetAllSubcategories()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Subcategories.ToList();
            }
        }

        //Returns a specific subcategory from the database
        public Subcategory GetSubcategory(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Subcategories.Include(i => i.Name)
                    .FirstOrDefault(i => i.CategoryId == id);
            }
        }

        // Returns a list of subcategories linked with the provided categoryId
        public List<Subcategory> GetAllLinkedSubcategories(int categoryId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Subcategories.Where(c => c.CategoryId == categoryId).ToList();
            }
        }

        //Removes a subcategory from the database
        public bool DeleteSubcategory(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var subcategory = context.Subcategories.FirstOrDefault(c => c.SubcategoryId == id);
                var category = context.Items.Where(i => i.SubcategoryId == id).ToList();
                if (subcategory != null && category != null)
                {
                    return false;
                }
                else
                {
                    context.Subcategories.Remove(subcategory);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        //Modify existing category
        public Subcategory ModifySubcategory(Subcategory subcategory)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(subcategory).State = EntityState.Modified;
                context.SaveChanges();
                return subcategory;
            }
        }

        //Shows items conneted to a certain category
        public List<Item> ShowItemsPerCategory(int categoryId)
        {
            using (var context = new ApplicationDbContext())
            {
                var itemsInCategory = context.Items.Where(i => i.CategoryId == categoryId).ToList();
                return itemsInCategory;
            }
        }

    }
}