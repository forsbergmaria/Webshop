using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemHasSize> ItemHasSize { get; set; }
        public DbSet<OrderContainsItem> OrderContainsItem { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    FirstName = "Anton",
                    LastName = "Kraft",
                    Id = "jfkdgjk8jd5509",
                    UserName = "username",
                    PasswordHash = "sdfghjklqwertyui12345678",
                    Email = "antonkraft25@gmail.com"
                }
                );

            modelBuilder.Entity<Category>().HasMany(c => c.Subcategories);
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Kläder"
                },

                new Category
                {
                    CategoryId = 2,
                    Name = "Kosttillskott"
                }
                );

            modelBuilder.Entity<Subcategory>().HasData(
                new Subcategory
                {
                    SubcategoryId = 1,
                    Name = "T-shirts",
                    CategoryId = 1
                },

                new Subcategory
                {
                    SubcategoryId = 2,
                    Name = "Proteinpulver",
                    CategoryId = 1
                }
                );

            modelBuilder.Entity<Item>().HasOne(i => i.Category);
            //modelBuilder.Entity<Item>().HasKey(fk => new { fk.ItemId });
            modelBuilder.Entity<Item>().HasMany(i => i.ProductImages);
            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    ItemId = 1,
                    ArticleNr = "1234",
                    Brand = "Bergslagen Sportcenter",
                    Name = "T-Shirt",
                    HasSize = true,
                    Color = "Svart",
                    Quantity = null,
                    Description = "En skön T-Shirt i bomullsmaterial",
                    CategoryId = 1,
                    SubcategoryId = 1,
                    IsPublished = true
                },
            new Item
            {
                ItemId = 2,
                ArticleNr = "1234",
                Name = "T-Shirt",
                Brand = "Bergslagen Sportcenter",
                HasSize = true,
                Color = "Vit",
                Quantity = null,
                Description = "En skön T - Shirt i bomullsmaterial",
                CategoryId = 1,
                SubcategoryId = 1,
                IsPublished = true
            },
            new Item
            {
                ItemId = 3,
                ArticleNr = "1231",
                Brand = "Tyngre",
                Name = "Vassle Kladdkaka",
                HasSize = false,
                Color = null,
                Quantity = 14,
                Description = "Maxa dina gainz med ett gott vassleproteinpulver från Tyngre!",
                CategoryId = 2,
                SubcategoryId = 2,
                IsPublished = true
            }
            );

            modelBuilder.Entity<Image>().HasData(
        new Image
        {
            ImageId = 1,
            ItemId = 3,
            Path = "\"C:\\Users\\maria\\Downloads\\Vassle_Kladdkaka.jpeg\""
        }
    );

            modelBuilder.Entity<Order>().HasData(
            new Order
            {
                OrderId = 1,
                CustomerFirstName = "Maria",
                CustomerLastName = "Forsberg",
                CustomerPhone = "0765696217",
                CustomerAddress = "Malmgatan 2A",
                CustomerZipCode = "73133",
                CustomerCity = "Köping"
            },
            new Order
            {
                OrderId = 2,
                CustomerFirstName = "Anton",
                CustomerLastName = "Kraft",
                CustomerPhone = "0767128320",
                CustomerAddress = "Malmgatan 2A",
                CustomerZipCode = "73133",
                CustomerCity = "Köping"
            }
                );

            modelBuilder.Entity<Size>().HasData(
                new Size
                {
                    SizeId = 1,
                    Name = "S"
                },
                new Size
                {
                    SizeId = 2,
                    Name = "M"
                },

                new Size
                {
                    SizeId = 3,
                    Name = "L"
                }
                );

            modelBuilder.Entity<OrderContainsItem>().HasOne(o => o.Orders);
            modelBuilder.Entity<OrderContainsItem>().HasOne(o => o.Items);
            modelBuilder.Entity<OrderContainsItem>().HasKey(vf => new { vf.OrderId, vf.ItemId });
            modelBuilder.Entity<OrderContainsItem>().HasData(
                new OrderContainsItem
                {
                    OrderId = 1,
                    ItemId = 2,
                    ItemQuantity = 1,
                },
                new OrderContainsItem
                {
                    OrderId = 1,
                    ItemId = 3,
                    ItemQuantity = 2,
                },
                new OrderContainsItem
                {
                    OrderId = 2,
                    ItemId = 2,
                    ItemQuantity = 1,
                }
                );

            modelBuilder.Entity<ItemHasSize>().HasOne(i => i.Items);
            modelBuilder.Entity<ItemHasSize>().HasOne(i => i.Sizes);
            modelBuilder.Entity<ItemHasSize>().HasKey(vf => new { vf.ItemId, vf.SizeId });
            modelBuilder.Entity<ItemHasSize>().HasData(
                    new ItemHasSize
                    {
                        ItemId = 1,
                        SizeId = 1,
                        Quantity = 18
                    },

                    new ItemHasSize
                    {
                        ItemId = 1,
                        SizeId = 2,
                        Quantity = 4
                    },

                    new ItemHasSize
                    {
                        ItemId = 1,
                        SizeId = 3,
                        Quantity = 0
                    },
                     new ItemHasSize
                     {
                         ItemId = 2,
                         SizeId = 1,
                         Quantity = 13
                     },

                    new ItemHasSize
                    {
                        ItemId = 2,
                        SizeId = 2,
                        Quantity = 0
                    },

                    new ItemHasSize
                    {
                        ItemId = 2,
                        SizeId = 3,
                        Quantity = 23
                    }
                );


        }
    }
}


