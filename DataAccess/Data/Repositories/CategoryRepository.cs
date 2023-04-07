using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Text;

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

        public Category GetCategoryByName(string name)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Categories.Where(c => c.Name == name).FirstOrDefault();
            }
        }

        //Removes a category from the database and assigns belonging items to an undefined category
        public void DeleteCategory(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var category = context.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
                var undifined = GetCategoryByName("Odefinierad");

                var subcategories = context.Subcategories.Where(c => c.Categories.CategoryId == id).ToList();
                var items = context.Items.ToList();

                List<Item> itemsList = new List<Item>();

                foreach (var item in items)
                {
                    if (item.CategoryId == id)
                    {
                        itemsList.Add(item);
                    }
                }
                foreach (var item in itemsList)
                {
                    item.CategoryId = undifined.CategoryId;
                    item.Subcategory = null;
                }

                context.Remove(category);
                context.SaveChanges();
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
        public void DeleteSubcategory(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var undefined = GetCategoryByName("Odefinierad");
                var subcategory = context.Subcategories.Where(c => c.SubcategoryId == id).FirstOrDefault();
                var items = context.Items.ToList();

                List<Item> itemsList = new List<Item>();
                foreach (var item in items)
                {
                    itemsList.Add(item);
                }
                foreach (var item in itemsList)
                {
                    item.SubcategoryId = subcategory.SubcategoryId;
                    item.SubcategoryId = null;
                }

                context.Subcategories.Remove(subcategory);
                context.SaveChanges();
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