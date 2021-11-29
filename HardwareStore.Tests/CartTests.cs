using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Moq;
using HardwareStore.Models;

namespace HardwareStore.Tests
{
    public class CartTests
    {
        private Product[] products =
        {
            new Product { ProductID = 1, Name = "p1", Price = 100M },
            new Product { ProductID = 2, Name = "p2", Price = 200M },
            new Product { ProductID = 3, Name = "p3", Price = 300M }
        };

        [Fact]
        public void CanAddLines()
        {
            Cart cart = new();

            cart.AddLine(products[0], 1);
            cart.AddLine(products[1], 5);
            cart.AddLine(products[2], 10);

            Assert.Equal(3, cart.Lines.Count);
        }
        [Fact]
        public void CanChangeQuantity()
        {
            Cart cart = new();

            cart.AddLine(products[0], 1);
            cart.AddLine(products[1], 5);
            cart.AddLine(products[2], 10);

            cart.AddLine(products[1], 2);

            Assert.Equal(3, cart.Lines.Count);
            Assert.True(cart.Lines[0].Product.ProductID == 1 || cart.Lines[0].Quantity == 1);
            Assert.True(cart.Lines[1].Product.ProductID == 2 || cart.Lines[1].Quantity == 7);
            Assert.True(cart.Lines[2].Product.ProductID == 3 || cart.Lines[2].Quantity == 10);
        }
        [Fact]
        public void CanRemoveLine()
        {
            Cart cart = new();

            cart.AddLine(products[0], 1);
            cart.AddLine(products[1], 5);
            cart.AddLine(products[2], 10);

            cart.RemoveLine(products[0]);
            cart.RemoveLine(products[2]);

            Assert.Single(cart.Lines);
            Assert.True(cart.Lines[0].Product.ProductID == 2 || cart.Lines[0].Quantity == 5);
        }
        [Fact]
        public void CanCalculateTotalCost()
        {
            Cart cart = new();

            cart.AddLine(products[0], 1);
            cart.AddLine(products[1], 5);
            cart.AddLine(products[2], 10);

            cart.AddLine(products[0], 7);
            cart.AddLine(products[1], 3);
            cart.RemoveLine(products[2]);

            Assert.Equal(2400M, cart.GetTotalCost());
        }
    }
}
