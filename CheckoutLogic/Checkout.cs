using CheckoutLogic.Models;
using CheckoutLogic.Repository;

namespace CheckoutLogic;

public class Checkout : ICheckout
{
    internal List<Product> _basket = [];

    public int GetTotalPrice()
    {
        throw new NotImplementedException();
    }

    public void Scan(string item)
    {
        if (!Products.ProductPrices.TryGetValue(item, out Product? value))
        {
            throw new ArgumentException($"Unknown SKU: {item}");
        }

        _basket.Add(value);
    }
}
