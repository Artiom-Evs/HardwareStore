using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace HardwareStore.Models
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : 
            base(options) { }
    }
}
