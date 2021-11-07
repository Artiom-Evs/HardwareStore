using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
    }
}
