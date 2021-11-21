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
            PageSize = 10;
        }

        public IActionResult Index(string category, int productPage = 1) =>
            View(new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PaginingInfo = new()
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(p => p.Category == category).Count()
                }, 
                CurrentCategory = category
            });
    }
}
