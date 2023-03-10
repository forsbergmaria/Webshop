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
                return context.Categories.Include(i => i.Name)
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
        public void AddSubcategory(Subcategory subcategory)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Subcategories.Add(subcategory);
                context.SaveChanges();
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