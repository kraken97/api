using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task3.Models;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Task3
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider provider)
        {
            using (SqliteContext c = new SqliteContext(provider.GetRequiredService<DbContextOptions<SqliteContext>>()))
            {
                c.Database.Migrate();

                if (!c.Pages.Any())
                {

                    var seedFile1 = File.ReadAllText($"{Directory.GetCurrentDirectory()}/Data/seed.txt");
                     var seedFile2 = File.ReadAllText($"{Directory.GetCurrentDirectory()}/Data/seed2.txt");
                      var seedFile3 = File.ReadAllText($"{Directory.GetCurrentDirectory()}/Data/seed3.txt");
                    c.Pages.AddRange(new Page() { Description = "TestPage", Title = "Test page1", UrlName = "test1", Content = seedFile1 },
                    new Page() { Description = "TestPage2", Title = "Test page2", UrlName = "test2", Content = seedFile2 },
                    new Page() { Description = "TestPage3", Title = "Test page3", UrlName = "test3", Content = seedFile3 },
                    new Page() { Description = "TestPage4", Title = "Test page4", UrlName = "test4", Content = seedFile1 },
                    new Page() { Description = "TestPage5", Title = "Test page5", UrlName = "test5", Content = seedFile2 }
                    );
                     c.SaveChanges();
                }


                if (!c.NavLinks.Any())
                {

                    c.NavLinks.AddRange(new NavLink() { Title = "Nav to test 1 ", PageId = 1, Position = 1 },
                    new NavLink() { Title = "Nav2 ", PageId = 2, Position = 1 },
                    new NavLink() { Title = "Nav to test 3 ", PageId = 3, Position = 1 },
                    new NavLink() { Title = "Nav  ", PageId = 4, Position = 1 },
                    new NavLink() { Title = "Nav to test 5 ", PageId = 5, Position = 1 });  
                        c.SaveChanges();
                    c.NavLinks.AddRange(  new NavLink() { Title = "Nav to test 4 ",ParentLinkID=2, PageId = 4, Position = 1 },
                    new NavLink() { Title = "Nav to test 3 ", PageId = 3,ParentLinkID=2, Position = 1 });
                }

                if (!c.RelatedPages.Any())
                {
                    c.RelatedPages.AddRange(new RelatedPages() { Page1Id = 1, Page2Id = 2 },
                    new RelatedPages() { Page1Id = 1, Page2Id = 3 },
                    new RelatedPages() { Page1Id = 2, Page2Id = 3 },
                    new RelatedPages() { Page1Id = 4, Page2Id = 5 },
                    new RelatedPages() { Page1Id = 3, Page2Id = 4 },
                    new RelatedPages() { Page1Id = 1, Page2Id = 5 });
                }
                c.SaveChanges();
            }

        }
    }
}