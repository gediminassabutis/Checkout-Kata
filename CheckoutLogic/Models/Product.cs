
namespace CheckoutLogic.Models;

public class Product(string SKU, double UnitPrice)
{
    public string SKU { get; } = SKU;
    public double UnitPrice { get; } = UnitPrice;
}