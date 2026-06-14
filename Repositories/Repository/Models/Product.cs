namespace Repository.Models;

public class Product(string SKU, decimal UnitPrice)
{
    public string SKU { get; } = SKU;
    public decimal UnitPrice { get; } = UnitPrice;
}