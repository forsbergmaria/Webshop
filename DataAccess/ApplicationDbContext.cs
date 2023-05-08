using Microsoft.AspNetCore.Identity;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test");
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemTransaction> ItemTransactions { get; set; }
        public DbSet<TransactionWithSizes> TransactionsWithSizes { get; set; }
        public DbSet<OrderContainsItem> OrderContainsItem { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
        public DbSet<ShippingStatus> ShippingStatuses { get; set; }
        public override DbSet<IdentityUserRole<string>> UserRoles { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = "jdigru",
                        NormalizedName = "HUVUDADMINISTRATÖR",
                        Name = "Huvudadministratör"
                    }
                );

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

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        UserId = "jfkdgjk8jd5509",
                        RoleId = "jdigru"
                    }
                );

            modelBuilder.Entity<Category>().HasMany(c => c.Subcategories)
                .WithOne(c => c.Categories)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Kläder",
                    IsPublished = true
                },

                new Category
                {
                    CategoryId = 2,
                    Name = "Kosttillskott",
                    IsPublished = true
                }
                );

            modelBuilder.Entity<Subcategory>()
                .HasData(
                new Subcategory
                {
                    SubcategoryId = 1,
                    Name = "T-shirts",
                    CategoryId = 1,
                    IsPublished = true
                },

                new Subcategory
                {
                    SubcategoryId = 2,
                    Name = "Proteinpulver",
                    CategoryId = 1,
                    IsPublished= true
                }
                );

            modelBuilder.Entity<Item>().HasOne(i => i.Category);
            modelBuilder.Entity<Item>().HasOne(i => i.Subcategory);
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
                    PriceWithoutVAT = 149.25m,
                    VAT = 1.25m,
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
                PriceWithoutVAT = 149.25m,
                VAT = 1.25m,
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
                Description = "Maxa dina gainz med ett gott vassleproteinpulver från Tyngre!",
                CategoryId = 2,
                SubcategoryId = 2,
                IsPublished = true
            }
            ) ;

            modelBuilder.Entity<Image>().HasData(
        new Image
        {
            ImageId = 1,
            ItemId = 3,
            Path = "\"C:\\Users\\maria\\Downloads\\Vassle_Kladdkaka.jpeg\""
        }
    );

            modelBuilder.Entity<ShippingStatus>().HasData(
        new ShippingStatus
        {
            StatusId = 1,
            Name = "Ohanterad"
        },

        new ShippingStatus
        {
            StatusId = 2,
            Name = "Pågående"
        },

        new ShippingStatus
        {
            StatusId = 3,
            Name = "Slutförd"
        }
    );

            modelBuilder.Entity<Order>().HasOne(s => s.ShippingStatus);
            modelBuilder.Entity<Order>().HasData(
            new Order
            {
                OrderId = 1,
                OrderDate = DateTime.Parse("2023-03-08 10:00"),
                CustomerName = "Maria Forsberg",
                CustomerPhone = "0765696217",
                CustomerAddress = "Malmgatan 2A",
                CustomerZipCode = "73133",
                CustomerCity = "Köping",
                ShippingStatusId = 1,
            },
            new Order
            {
                OrderId = 2,
                OrderDate = DateTime.Parse("2023-03-10 13:44"),
                CustomerName = "Anton Kraft",
                CustomerPhone = "0767128320",
                CustomerAddress = "Malmgatan 2A",
                CustomerZipCode = "73133",
                CustomerCity = "Köping",
                ShippingStatusId = 2,
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

            modelBuilder.Entity<ItemHasSize>().HasOne(s => s.Item);
            modelBuilder.Entity<ItemHasSize>().HasOne(s => s.Size);
            modelBuilder.Entity<ItemHasSize>().HasKey(vf => new { vf.ItemId, vf.SizeId });
            modelBuilder.Entity<ItemHasSize>().HasData(
                new ItemHasSize
                {
                    ItemId = 36,
                    SizeId = 1
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
                    StripeItemId = "prod_NYcwS1YSGIhUW1",
                    ItemQuantity = 1,
                    Total = 199
                },
                new OrderContainsItem
                {
                    OrderId = 1,
                    ItemId = 3,
                    StripeItemId = "prod_NYcwS1YSGIhUW1",
                    ItemQuantity = 2,
                    Total = 199
                },
                new OrderContainsItem
                {
                    OrderId = 2,
                    ItemId = 2,
                    StripeItemId = "prod_NYcwS1YSGIhUW1",
                    ItemQuantity = 1,
                    Total = 199
                }
                );

            modelBuilder.Entity<ItemTransaction>().HasOne(i => i.Items);
            modelBuilder.Entity<ItemTransaction>().HasData(
                    new ItemTransaction
                    {
                        TransactionId = 1,
                        ItemId = 3,
                        Quantity = 16,
                        TransactionType = "In",
                        TransactionDate = DateTime.Now
                    }
                );

			modelBuilder.Entity<TransactionWithSizes>().HasOne(i => i.Items);
            modelBuilder.Entity<TransactionWithSizes>().HasOne(i => i.Sizes);
            modelBuilder.Entity<TransactionWithSizes>().HasData(
                    new TransactionWithSizes
                    {
                        TransactionId = 2,
                        ItemId = 1,
                        SizeId = 1,
                        Quantity = 18,
                        TransactionType = "In",
                        TransactionDate = DateTime.Parse("2023-03-11")
                    },

                    new TransactionWithSizes
                    {
                        TransactionId = 3,
                        ItemId = 1,
                        SizeId = 2,
                        Quantity = 4,
                        TransactionType = "In",
                        TransactionDate = DateTime.Parse("2023-03-11")
                    },

                    new TransactionWithSizes
                    {
                        TransactionId = 4,
                        ItemId = 1,
                        SizeId = 3,
                        Quantity = 0,
						TransactionType = "In",
						TransactionDate = DateTime.Parse("2023-03-12")
					},
                     new TransactionWithSizes
                     {
                         TransactionId = 5,
                         ItemId = 2,
                         SizeId = 1,
                         Quantity = 13,
						 TransactionType = "In",
						 TransactionDate = DateTime.Parse("2023-03-11")
					 },

                    new TransactionWithSizes
                    {
                        TransactionId = 6,
                        ItemId = 2,
                        SizeId = 2,
                        Quantity = 0,
						TransactionType = "In",
						TransactionDate = DateTime.Parse("2023-03-12")
					},

                    new TransactionWithSizes
                    {
                        TransactionId = 7,
                        ItemId = 2,
                        SizeId = 3,
                        Quantity = 23,
						TransactionType = "In",
						TransactionDate = DateTime.Parse("2023-03-12")
					}
                );


        }
    }
}


