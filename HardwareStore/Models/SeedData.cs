using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            StoreDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();

            if (context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { ProductID = 1, Name = "Product", Price = 100M, Category = "category1", Description = "SomeProduct1" }
                    );
            }
            ;
        }
    }
}
