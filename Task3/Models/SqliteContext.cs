using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Models
{
    public class SqliteContext : DbContext
    {
        public SqliteContext (DbContextOptions<SqliteContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Page>().Property(e => e.AddedDate).HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')").ValueGeneratedOnAdd();
            

        }

        public DbSet<Page> Pages { get; set; }

        public DbSet<RelatedPages> RelatedPages { get; set; }

        public DbSet<NavLink> NavLinks { get; set; }
    }
}
