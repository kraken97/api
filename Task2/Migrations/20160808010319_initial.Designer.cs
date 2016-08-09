using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Task2.Models;

namespace Task2.Migrations
{
    [DbContext(typeof(SqliteContext))]
    [Migration("20160808010319_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Task2.Models.NavLink", b =>
                {
                    b.Property<int>("NavLinkId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("PageId");

                    b.Property<int?>("ParentLinkID");

                    b.Property<int?>("Position");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("NavLinkId");

                    b.HasIndex("PageId");

                    b.ToTable("NavLinks");
                });

            modelBuilder.Entity("Task2.Models.Page", b =>
                {
                    b.Property<int>("PageId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("Title")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("UrlName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 15);

                    b.HasKey("PageId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("Task2.Models.RelatedPages", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Page1Id");

                    b.Property<int?>("Page2Id");

                    b.HasKey("ID");

                    b.HasIndex("Page1Id");

                    b.HasIndex("Page2Id");

                    b.ToTable("RelatedPages");
                });

            modelBuilder.Entity("Task2.Models.NavLink", b =>
                {
                    b.HasOne("Task2.Models.Page", "Page")
                        .WithMany()
                        .HasForeignKey("PageId");
                });

            modelBuilder.Entity("Task2.Models.RelatedPages", b =>
                {
                    b.HasOne("Task2.Models.Page", "Page1")
                        .WithMany()
                        .HasForeignKey("Page1Id");

                    b.HasOne("Task2.Models.Page", "Page2")
                        .WithMany()
                        .HasForeignKey("Page2Id");
                });
        }
    }
}
