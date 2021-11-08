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

            var result = ((controller.Index() as ViewResult).ViewData.Model as IEnumerable<Product>).ToArray();

            Assert.True(result.Length == 3);
            Assert.True(result[0].ProductID == 1 && result[0].Name == "p1");
            Assert.True(result[1].ProductID == 2 && result[1].Name == "p2");
            Assert.True(result[2].ProductID == 3 && result[2].Name == "p3");
        }
    }
}
