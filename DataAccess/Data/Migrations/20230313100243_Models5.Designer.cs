﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AdminPanel.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230313100243_Models5")]
    partial class Models5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            Name = "Kläder"
                        },
                        new
                        {
                            CategoryId = 2,
                            Name = "Kosttillskott"
                        });
                });

            modelBuilder.Entity("Models.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageId"));

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImageId");

                    b.HasIndex("ItemId");

                    b.ToTable("Images");

                    b.HasData(
                        new
                        {
                            ImageId = 1,
                            ItemId = 3,
                            Path = "\"C:\\Users\\maria\\Downloads\\Vassle_Kladdkaka.jpeg\""
                        });
                });

            modelBuilder.Entity("Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemId"));

                    b.Property<string>("ArticleNr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasSize")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PriceWithoutVAT")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("SubcategoryId")
                        .HasColumnType("int");

                    b.Property<decimal>("VAT")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ItemId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("Items");

                    b.HasData(
                        new
                        {
                            ItemId = 1,
                            ArticleNr = "1234",
                            Brand = "Bergslagen Sportcenter",
                            CategoryId = 1,
                            Color = "Svart",
                            Description = "En skön T-Shirt i bomullsmaterial",
                            HasSize = true,
                            IsPublished = true,
                            Name = "T-Shirt",
                            PriceWithoutVAT = 149.25m,
                            SubcategoryId = 1,
                            VAT = 1.25m
                        },
                        new
                        {
                            ItemId = 2,
                            ArticleNr = "1234",
                            Brand = "Bergslagen Sportcenter",
                            CategoryId = 1,
                            Color = "Vit",
                            Description = "En skön T - Shirt i bomullsmaterial",
                            HasSize = true,
                            IsPublished = true,
                            Name = "T-Shirt",
                            PriceWithoutVAT = 149.25m,
                            SubcategoryId = 1,
                            VAT = 1.25m
                        },
                        new
                        {
                            ItemId = 3,
                            ArticleNr = "1231",
                            Brand = "Tyngre",
                            CategoryId = 2,
                            Description = "Maxa dina gainz med ett gott vassleproteinpulver från Tyngre!",
                            HasSize = false,
                            IsPublished = true,
                            Name = "Vassle Kladdkaka",
                            PriceWithoutVAT = 0m,
                            SubcategoryId = 2,
                            VAT = 0m
                        });
                });

            modelBuilder.Entity("Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("CustomerAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerCity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerFirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerLastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            OrderId = 1,
                            CustomerAddress = "Malmgatan 2A",
                            CustomerCity = "Köping",
                            CustomerFirstName = "Maria",
                            CustomerLastName = "Forsberg",
                            CustomerPhone = "0765696217",
                            CustomerZipCode = "73133",
                            OrderDate = new DateTime(2023, 3, 8, 10, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            OrderId = 2,
                            CustomerAddress = "Malmgatan 2A",
                            CustomerCity = "Köping",
                            CustomerFirstName = "Anton",
                            CustomerLastName = "Kraft",
                            CustomerPhone = "0767128320",
                            CustomerZipCode = "73133",
                            OrderDate = new DateTime(2023, 3, 10, 13, 44, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Models.OrderContainsItem", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("ItemQuantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("OrderContainsItem");

                    b.HasData(
                        new
                        {
                            OrderId = 1,
                            ItemId = 2,
                            ItemQuantity = 1
                        },
                        new
                        {
                            OrderId = 1,
                            ItemId = 3,
                            ItemQuantity = 2
                        },
                        new
                        {
                            OrderId = 2,
                            ItemId = 2,
                            ItemQuantity = 1
                        });
                });

            modelBuilder.Entity("Models.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SizeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SizeId");

                    b.ToTable("Sizes");

                    b.HasData(
                        new
                        {
                            SizeId = 1,
                            Name = "S"
                        },
                        new
                        {
                            SizeId = 2,
                            Name = "M"
                        },
                        new
                        {
                            SizeId = 3,
                            Name = "L"
                        });
                });

            modelBuilder.Entity("Models.StockTransaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionId");

                    b.HasIndex("ItemId");

                    b.ToTable("StockTransactions");

                    b.HasData(
                        new
                        {
                            TransactionId = 1,
                            ItemId = 3,
                            Quantity = 16,
                            TransactionDate = new DateTime(2023, 3, 13, 11, 2, 43, 581, DateTimeKind.Local).AddTicks(5616),
                            TransactionType = "In"
                        });
                });

            modelBuilder.Entity("Models.StockTransactionSizes", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"));

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ItemId", "SizeId");

                    b.HasIndex("SizeId");

                    b.ToTable("StockTransactionSizes");

                    b.HasData(
                        new
                        {
                            ItemId = 1,
                            SizeId = 1,
                            Quantity = 18,
                            TransactionDate = new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TransactionId = 1,
                            TransactionType = "In"
                        },
                        new
                        {
                            ItemId = 1,
                            SizeId = 2,
                            Quantity = 4,
                            TransactionDate = new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TransactionId = 2,
                            TransactionType = "In"
                        },
                        new
                        {
                            ItemId = 1,
                            SizeId = 3,
                            Quantity = 0,
                            TransactionDate = new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TransactionId = 3,
                            TransactionType = "In"
                        },
                        new
                        {
                            ItemId = 2,
                            SizeId = 1,
                            Quantity = 13,
                            TransactionDate = new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TransactionId = 4,
                            TransactionType = "In"
                        },
                        new
                        {
                            ItemId = 2,
                            SizeId = 2,
                            Quantity = 0,
                            TransactionDate = new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TransactionId = 5,
                            TransactionType = "In"
                        },
                        new
                        {
                            ItemId = 2,
                            SizeId = 3,
                            Quantity = 23,
                            TransactionDate = new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TransactionId = 6,
                            TransactionType = "In"
                        });
                });

            modelBuilder.Entity("Models.Subcategory", b =>
                {
                    b.Property<int>("SubcategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubcategoryId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubcategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Subcategories");

                    b.HasData(
                        new
                        {
                            SubcategoryId = 1,
                            CategoryId = 1,
                            Name = "T-shirts"
                        },
                        new
                        {
                            SubcategoryId = 2,
                            CategoryId = 1,
                            Name = "Proteinpulver"
                        });
                });

            modelBuilder.Entity("Models.Admin", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Admin");

                    b.HasData(
                        new
                        {
                            Id = "jfkdgjk8jd5509",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "a88066c6-8e9b-41b5-b8a8-92c7e65ac84f",
                            Email = "antonkraft25@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PasswordHash = "sdfghjklqwertyui12345678",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "6d1084d9-78fb-4565-be7b-b969248e730f",
                            TwoFactorEnabled = false,
                            UserName = "username",
                            FirstName = "Anton",
                            LastName = "Kraft"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Image", b =>
                {
                    b.HasOne("Models.Item", "Items")
                        .WithMany("ProductImages")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Models.Item", b =>
                {
                    b.HasOne("Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Subcategory", "Subcategory")
                        .WithMany()
                        .HasForeignKey("SubcategoryId");

                    b.Navigation("Category");

                    b.Navigation("Subcategory");
                });

            modelBuilder.Entity("Models.OrderContainsItem", b =>
                {
                    b.HasOne("Models.Item", "Items")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Order", "Orders")
                        .WithMany("OrderContainsItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Items");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Models.StockTransaction", b =>
                {
                    b.HasOne("Models.Item", "Items")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Models.StockTransactionSizes", b =>
                {
                    b.HasOne("Models.Item", "Items")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Size", "Sizes")
                        .WithMany()
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Items");

                    b.Navigation("Sizes");
                });

            modelBuilder.Entity("Models.Subcategory", b =>
                {
                    b.HasOne("Models.Category", "Categories")
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Models.Category", b =>
                {
                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("Models.Item", b =>
                {
                    b.Navigation("ProductImages");
                });

            modelBuilder.Entity("Models.Order", b =>
                {
                    b.Navigation("OrderContainsItems");
                });
#pragma warning restore 612, 618
        }
    }
}
