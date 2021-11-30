using HardwareStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace HardwareStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void CanLoadCart()
        {
            Product p1 = new Product { ProductID = 1, Name = "p1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "p2", Price = 200M };
            Mock<IStoreRepository> mockRepository = new();
            mockRepository.Setup(c => c.Products).Returns(new Product[] { p1, p2 }.AsQueryable<Product>());

            Cart testCart = new();
            testCart.AddLine(p1, 2);
            testCart.AddLine(p2, 5);

            Mock<ISession> mockSession = new();
            byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(testCart));
            mockSession.Setup(c => c.TryGetValue(It.IsAny<string>(), out data));

            Mock<HttpContext> mockContext = new();
            mockContext.Setup(c => c.Session).Returns(mockSession.Object);

            Pages.CartModel cartModel = new(mockRepository.Object)
            {
                PageContext = new PageContext(
                    new ActionContext
                    {
                        HttpContext = mockContext.Object,
                        RouteData = new RouteData(),
                        ActionDescriptor = new PageActionDescriptor()
                    })
            };

            cartModel.OnGet("myUrl");

            Assert.Equal(2, cartModel.Cart.Lines.Count);
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }
        [Fact]
        public void CanUpdateCart()
        {
            Product p1 = new Product { ProductID = 1, Name = "p1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "p2", Price = 200M };
            Mock<IStoreRepository> mockRepository = new();
            mockRepository.Setup(c => c.Products).Returns(new Product[] { p1, p2 }.AsQueryable<Product>());

            Cart testCart = new();

            Mock<ISession> mockSession = new();
            mockSession.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, val) =>
                {
                    testCart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val));
                });

            Mock<HttpContext> mockContext = new();
            mockContext.Setup(c => c.Session).Returns(mockSession.Object);

            Pages.CartModel cartModel = new(mockRepository.Object)
            {
                PageContext = new PageContext(
                    new ActionContext
                    {
                        HttpContext = mockContext.Object,
                        RouteData = new RouteData(),
                        ActionDescriptor = new PageActionDescriptor()
                    })
            };

            cartModel.OnPost(1, "myUrl");

            Assert.Single(cartModel.Cart.Lines);
            Assert.Equal(p1.Name, cartModel.Cart.Lines.First().Product.Name);
            Assert.Equal(1, cartModel.Cart.Lines.First().Quantity);
        }
    }
}
