using CheckoutLogic.Models;

namespace CheckoutLogic.Repository;

internal static class Products
{
    internal static readonly Dictionary<string, Product> ProductPrices = new()
    {
        { "A", new Product("A", 50) },
        { "B", new Product("B", 30) },
        { "C", new Product("C", 20) },
        { "D", new Product("D", 15) }
    };
}
