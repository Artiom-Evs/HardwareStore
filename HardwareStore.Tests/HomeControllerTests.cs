using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using HardwareStore.Controllers;
using HardwareStore.Models;
using HardwareStore.Models.ViewModels;

namespace HardwareStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CanUseRepository()
        {
            Mock<IStoreRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID = 1, Name = "p1", Price = 100M, Category = "c1", Description = "d1"},
                new Product{ProductID = 2, Name = "p2", Price = 100M, Category = "c2", Description = "d1"},
                new Product{ProductID = 3, Name = "p3", Price = 100M, Category = "c13", Description = "d1"}
            }.AsQueryable<Product>());

            HomeController controller = new(mock.Object);

            ProductsListViewModel result = (controller.Index(null) as ViewResult).ViewData.Model as ProductsListViewModel;
            Product[] products = result.Products.ToArray();

            Assert.True(products.Length == 3);
            Assert.True(products[0].ProductID == 1 && products[0].Name == "p1");
            Assert.True(products[1].ProductID == 2 && products[1].Name == "p2");
            Assert.True(products[2].ProductID == 3 && products[2].Name == "p3");
        }

        [Fact]
        public void CanSeparateToPages()
        {
            Mock<IStoreRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID = 1, Name = "p1", Price = 100M, Category = "c1", Description = "d1"},
                new Product{ProductID = 2, Name = "p2", Price = 200M, Category = "c2", Description = "d2"},
                new Product{ProductID = 3, Name = "p3", Price = 300M, Category = "c13", Description = "d3"},
                new Product{ ProductID = 4, Name = "p4", Price = 400M, Category = "c1", Description = "d4"},
                new Product{ProductID = 5, Name = "p5", Price = 500M, Category = "c2", Description = "d5"}
            }.AsQueryable<Product>());

            HomeController controller = new(mock.Object)
            {
                PageSize = 2
            };

            ProductsListViewModel result = (controller.Index(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel;
            Product[] products = result.Products.ToArray();

            Assert.True(products.Length == 2);
            Assert.True(products[0].ProductID == 3 && products[0].Name == "p3");
            Assert.True(products[1].ProductID == 4 && products[1].Name == "p4");
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            Mock<IStoreRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID = 1, Name = "p1", Price = 100M, Category = "c1", Description = "d1"},
                new Product{ProductID = 2, Name = "p2", Price = 200M, Category = "c2", Description = "d2"},
                new Product{ProductID = 3, Name = "p3", Price = 300M, Category = "c13", Description = "d3"},
                new Product{ ProductID = 4, Name = "p4", Price = 400M, Category = "c1", Description = "d4"},
                new Product{ProductID = 5, Name = "p5", Price = 500M, Category = "c2", Description = "d5"}
            }.AsQueryable<Product>());

            HomeController controller = new(mock.Object)
            {
                PageSize = 2
            };

            ProductsListViewModel result = (controller.Index(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel;
            
            Assert.Equal(2, result.PaginingInfo.CurrentPage);
            Assert.Equal(2, result.PaginingInfo.ItemsPerPage);
            Assert.Equal(5, result.PaginingInfo.TotalItems);
            Assert.Equal(3, result.PaginingInfo.TotalPages);

        }

        [Fact]
        public void CanFilterProducts()
        {
            Mock<IStoreRepository> mock = new();
            mock.Setup(p => p.Products).Returns(
                new Product[]
                {
                    new Product { ProductID = 1, Name = "p1", Category = "c1", Price = 100M },
                    new Product { ProductID = 2, Name = "p2", Category = "c2", Price = 200M },
                    new Product { ProductID = 3, Name = "p3", Category = "c3", Price = 300M },
                    new Product { ProductID = 4, Name = "p4", Category = "c1", Price = 400M },
                    new Product { ProductID = 5, Name = "p5", Category = "c2", Price = 500M }
                }.AsQueryable<Product>());

            HomeController controller = new(mock.Object);

            var result = ((controller.Index("c2") as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();

            Assert.Equal(2, result.Length);
            Assert.True(result[0].ProductID == 2 && result[0].Name == "p2");
            Assert.True(result[1].ProductID == 5 && result[1].Name == "p5");
        }

        [Fact]
        public void CanGetProductsByCategory()
        {
            Mock<IStoreRepository> mock = new();
            mock.Setup(p => p.Products).Returns(
                new Product[]
                {
                    new Product { ProductID = 1, Name = "p1", Category = "c1", Price = 100M },
                    new Product { ProductID = 2, Name = "p2", Category = "c2", Price = 200M },
                    new Product { ProductID = 3, Name = "p3", Category = "c3", Price = 300M },
                    new Product { ProductID = 4, Name = "p4", Category = "c1", Price = 400M },
                    new Product { ProductID = 5, Name = "p5", Category = "c2", Price = 500M }
                }.AsQueryable<Product>());

            HomeController controller = new(mock.Object);

            var result0 = ((controller.Index(null) as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();
            var result1 = ((controller.Index("c1") as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();
            var result2 = ((controller.Index("c2") as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();
            var result3 = ((controller.Index("c3") as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();
            
            Assert.Equal(5, result0.Length);
            Assert.Equal(2, result1.Length);
            Assert.Equal(2, result2.Length);
            Assert.Single(result3);
        }
    }
}
