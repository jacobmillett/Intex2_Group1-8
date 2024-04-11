using Microsoft.AspNetCore.Mvc;

namespace AuroraBricks.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(BrixProduct product, int quantity)
        {
            CartLine? line = Lines
                .Where(p => p.Product.ProductId == product.ProductId)
                .FirstOrDefault();

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

        public virtual void RemoveLine(BrixProduct product) =>
            Lines.RemoveAll(l => l.Product.ProductId
                == product.ProductId);

        public decimal ComputeTotalValue() =>
            (decimal)Lines.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => Lines.Clear();
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public BrixProduct Product { get; set; } = new();
        public int Quantity { get; set; }
    }
}
