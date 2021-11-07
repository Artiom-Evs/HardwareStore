using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext dbContext;

        public EFStoreRepository(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Product> Products =>
            dbContext.Products;
    }
}
