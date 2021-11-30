using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Models;

namespace HardwareStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(this.Configuration["ConnectionStrings:HardwareStoreConnection"]);
            });
            services.AddScoped<IStoreRepository, EFStoreRepository>();
            services.AddRazorPages();
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controller = "Home", Action = "Index" });
                endpoints.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", Action = "Index", productPage = 1 });
                endpoints.MapControllerRoute("category", "{category}", new { Controller = "Home", Action = "Index", productPage = 1 });
                endpoints.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", Action = "Index", productPage = 1 });
                endpoints.MapControllerRoute("home", "/", new { Controller = "Home", Action = "Index" });
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

            SeedData.EnsurePopulated(app);
        }
    }
}
