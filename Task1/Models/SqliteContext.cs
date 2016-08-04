using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication16.Models
{
    public class SqliteContext : DbContext
    {
        public SqliteContext (DbContextOptions<SqliteContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           //optionsBuilder.UseSqlite($"Data Source={Directory.GetCurrentDirectory()}/my.db");

        }
        public DbSet<Movie> Movie { get; set; }
    }
}
