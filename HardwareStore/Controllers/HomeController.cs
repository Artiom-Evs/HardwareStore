using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HardwareStore.Models;
using HardwareStore.Models.ViewModels;

namespace HardwareStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoreRepository repository;
        public int PageSize { get; set; }

        public HomeController(IStoreRepository repository)
        {
            this.repository = repository;
            PageSize = 15;
        }

        public IActionResult Index(int productPage = 1) =>
            View(new ProductsListViewModel
            {
                Products = repository.Products
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PaginingInfo = new()
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                }
            });

    }
}
