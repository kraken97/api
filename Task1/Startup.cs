using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Task1;
using Task1.Models;
using Task1.Services;

namespace Task1
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            //should delete this
            builder.AddUserSecrets();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();
            services.AddLogging();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<INavRepository, NavLinksRepository>();
            services.AddScoped<IRelPagesRepository, RelPagesRepository>();
            services.AddIdentity<User, IdentityRole>()
           .AddEntityFrameworkStores<SqliteContext>()
           .AddDefaultTokenProviders();
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            // services.Configure<MvcOptions>(options =>
            //                     {
            //                         options.Filters.Add(new RequireHttpsAttribute());
            //                     });

            services.AddDbContext<SqliteContext>(options =>
             options.UseSqlite($"Data Source={Directory.GetCurrentDirectory()}/movie.db"));
            services.Configure<AuthMessageSenderOptions>(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Index/Error");
            }   

            app.UseStaticFiles();
            app.UseIdentity();


                //should use secret manager 
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = "628489387332140",
                AppSecret = "1794886f2f68dc069ab0b4d4d00dc35f"
            });
            //should use secret manager 
         app.UseGoogleAuthentication(new GoogleOptions {
                ClientId = "154663659899-5lg38an5f234ji1atn30tpu9lfdp0d3k.apps.googleusercontent.com",
                ClientSecret = "qrcw8qmJNzUOEp17r66zsFx1"
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(    
                    name: "default",
                    template: "{controller=Pages}/{action=Index}/{id?}");
            });
            SeedData.Initialize(app.ApplicationServices);
        }
    }
}
