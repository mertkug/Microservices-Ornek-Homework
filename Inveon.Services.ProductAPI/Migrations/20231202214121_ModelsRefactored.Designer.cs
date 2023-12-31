﻿// <auto-generated />
using Inveon.Services.ProductAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inveon.Services.ProductAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231202214121_ModelsRefactored")]
    partial class ModelsRefactored
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Inveon.Services.ProductAPI.Models.Colour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Img")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Colours");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Img = "greenimg",
                            Name = "green",
                            ProductId = 1,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 2,
                            Img = "redimg",
                            Name = "red",
                            ProductId = 1,
                            Quantity = 2
                        });
                });

            modelBuilder.Entity("Inveon.Services.ProductAPI.Models.Label", b =>
                {
                    b.Property<int>("LabelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LabelId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LabelId");

                    b.ToTable("Label");

                    b.HasData(
                        new
                        {
                            LabelId = 1,
                            Name = "Trending"
                        });
                });

            modelBuilder.Entity("Inveon.Services.ProductAPI.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoverImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryName = "Appetizer",
                            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            HoverImageUrl = "hovered.jpg",
                            ImageUrl = "samosa.jpg",
                            Name = "Green Dress For Woman",
                            Price = 15.0
                        });
                });

            modelBuilder.Entity("Inveon.Services.ProductAPI.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Ratings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Count = 100,
                            ProductId = 1,
                            Rate = 3.2999999999999998
                        });
                });

            modelBuilder.Entity("LabelProduct", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("LabelId")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "LabelId");

                    b.HasIndex("LabelId");

                    b.ToTable("LabelProduct");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            LabelId = 1
                        });
                });

            modelBuilder.Entity("Inveon.Services.ProductAPI.Models.Colour", b =>
                {
                    b.HasOne("Inveon.Services.ProductAPI.Models.Product", "Product")
                        .WithMany("Colours")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Inveon.Services.ProductAPI.Models.Rating", b =>
                {
                    b.HasOne("Inveon.Services.ProductAPI.Models.Product", "Product")
                        .WithOne("Rating")
                        .HasForeignKey("Inveon.Services.ProductAPI.Models.Rating", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("LabelProduct", b =>
                {
                    b.HasOne("Inveon.Services.ProductAPI.Models.Label", null)
                        .WithMany()
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveon.Services.ProductAPI.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inveon.Services.ProductAPI.Models.Product", b =>
                {
                    b.Navigation("Colours");

                    b.Navigation("Rating")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
