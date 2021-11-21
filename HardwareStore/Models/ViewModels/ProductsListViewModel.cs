using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HardwareStore.Models;

namespace HardwareStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PaginingInfo PaginingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
