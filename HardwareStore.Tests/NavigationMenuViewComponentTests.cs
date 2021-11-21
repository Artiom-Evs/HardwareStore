using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using Xunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using HardwareStore.Components;
using HardwareStore.Models;

namespace HardwareStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void CanCreateListCategories()
        {
            Mock<IStoreRepository> mock = new();
            mock.Setup(p => p.Products).Returns(new Product[]
                {
                    new Product { ProductID = 1, Name = "p1", Category = "c2", Price = 100M },
                    new Product { ProductID = 2, Name = "p2", Category = "c1", Price = 200M },
                    new Product { ProductID = 3, Name = "p3", Category = "c3", Price = 300M },
                    new Product { ProductID = 4, Name = "p4", Category = "c1", Price = 400M },
                    new Product { ProductID = 5, Name = "p5", Category = "c2", Price = 500M }
                }.AsQueryable<Product>());

            NavigationMenuViewComponent viewComponent = new(mock.Object);

            string[] result = ((viewComponent.Invoke() as ViewViewComponentResult).ViewData.Model as IEnumerable<String>).ToArray();

            Assert.Equal(3, result.Length);
            Assert.True(result[0] == "c1");
            Assert.True(result[1] == "c2");
            Assert.True(result[2] == "c3");
        }

        [Fact]
        public void CanSelectCategory()
        {
            Mock<IStoreRepository> mock = new();
            mock.Setup(p => p.Products).Returns(new Product[]
                {
                    new Product { ProductID = 1, Name = "p1", Category = "c2", Price = 100M },
                    new Product { ProductID = 2, Name = "p2", Category = "c1", Price = 200M },
                    new Product { ProductID = 3, Name = "p3", Category = "c3", Price = 300M },
                    new Product { ProductID = 4, Name = "p4", Category = "c1", Price = 400M },
                    new Product { ProductID = 5, Name = "p5", Category = "c2", Price = 500M }
                }.AsQueryable<Product>());

            NavigationMenuViewComponent viewComponent = new(mock.Object);
            viewComponent.ViewComponentContext = new()
            {
                ViewContext = new()
                {
                    RouteData = new()
                }
            };
            viewComponent.RouteData.Values["category"] = "c2";

            string result = (string)(viewComponent.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            Assert.Equal("c2", result);
        }
    }
}
