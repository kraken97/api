using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Task1.Models;

namespace mvc.Migrations
{
    [DbContext(typeof(SqliteContext))]
    [Migration("20160804164708_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Task1.Models.Movie", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.Property<decimal>("Price");

                    b.Property<string>("Rating");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Movie");
                });
        }
    }
}
