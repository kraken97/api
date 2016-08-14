using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task3.Models;

namespace Task3.Models
{
    public class SqliteContext : IdentityDbContext<User>
 
    {
        
        public DbSet<Page> Pages { get; set; }

        public DbSet<RelatedPages> RelatedPages { get; set; }

        public DbSet<NavLink> NavLinks { get; set; }
        public SqliteContext(DbContextOptions<SqliteContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
             mb.Entity<Page>().Property(e => e.AddedDate).HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')").ValueGeneratedOnAdd();
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

}
