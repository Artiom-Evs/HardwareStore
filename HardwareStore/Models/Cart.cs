using System.Collections.Generic;
using System.Linq;

namespace HardwareStore.Models
{
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new();

        public void AddLine(Product product, int quantity)
        {
            CartLine line = Lines.FirstOrDefault(l => l.Product.ProductID == product.ProductID);

            if (line == null)
            {
                Lines.Add(new CartLine   
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            CartLine line = Lines.First(l => l.Product.ProductID == product.ProductID);

            if (line != null)
            {
                Lines.Remove(line);
            }
        }

        public void Clear()
        {
            Lines.Clear();
        }

        public decimal GetTotalCost()
        {
            return Lines.Sum(l => l.Product.Price * l.Quantity);
        }
    }
}
